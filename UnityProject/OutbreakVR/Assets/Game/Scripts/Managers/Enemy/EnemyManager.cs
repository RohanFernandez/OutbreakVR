using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum ENEMY_TYPE
    {
        SECURITY_OFFICER = 0,
        AUTOMATED_TURRET = 1,
        PROXIMITY_BOMB = 2,
    }

    public enum ENEMY_ATTACK_TYPE
    {
        MELEE,
        STATIC,
        RANGED
    }

    public class EnemyManager : AbsComponentHandler, IReuseManager
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static EnemyManager s_Instance = null;

        /// <summary>
        /// List of all enemy prefabs
        /// Should be unique
        /// </summary>
        [SerializeField]
        private List<EnemyBase> m_lstEnemyPrefabs = null;

        /// <summary>
        /// List of all enemies that are alerted at the moment
        /// </summary>
        [SerializeField]
        private List<EnemyBase> m_lstAlertedEnemies = null;

        /// <summary>
        /// The list of all enemy dependants in the scene
        /// </summary>
        [SerializeField]
        private List<EnemyDependantBase> m_lstEnemyDependantBase = null;

        /// <summary>
        /// Dictionary of enemy type to Pool
        /// </summary>
        private Dictionary<ENEMY_TYPE, EnemyPool> m_dictEnemyPools = null;

        /// <summary>
        /// Are all enemies movement or action paused
        /// </summary>
        [SerializeField]
        private bool m_bIsEnemiesPaused = false;

        /// <summary>
        /// The objective id sent on enemy killed
        /// </summary>
        public const string ENEMY_OBJECTIVE_ID = "ENEMY_OBJECTIVE_ID";

        [SerializeField]
        private PatrolManager m_PatrolManager = null;

        [SerializeField]
        private EnemyDamageIndicatorManager m_EnemyDamageIndicatorManager = null;

        /// <summary>
        /// Sets the singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            m_PatrolManager = new PatrolManager();
            m_PatrolManager.initialize();

            m_EnemyDamageIndicatorManager.initialize();

            m_lstEnemyDependantBase = new List<EnemyDependantBase>(10);

            m_lstAlertedEnemies = new List<EnemyBase>(10);

            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_PAUSED_TOGGLED, onGamePauseToggled);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, onGameplayEnded);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_ENEMY_ALERT_STARTED, onEnemyAlertStarted);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_ENEMY_ALERT_ENDED, onEnemyAlertEnded);

            int l_iEnemyPrefabCount = m_lstEnemyPrefabs.Count;
            m_dictEnemyPools = new Dictionary<ENEMY_TYPE, EnemyPool>(l_iEnemyPrefabCount);

            for (int l_iEnemyPrefabIndex = 0; l_iEnemyPrefabIndex < l_iEnemyPrefabCount; l_iEnemyPrefabIndex++)
            {
                EnemyBase l_CurrentEnemyPrefab = m_lstEnemyPrefabs[l_iEnemyPrefabIndex];
                m_dictEnemyPools.Add(l_CurrentEnemyPrefab.getEnemyType(), new EnemyPool(l_CurrentEnemyPrefab, this.gameObject ));
            }
        }

        /// <summary>
        /// Destroys the singleton instance
        /// Sets it to null
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAME_PAUSED_TOGGLED, onGamePauseToggled);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, onGameplayEnded);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_ENEMY_ALERT_STARTED, onEnemyAlertStarted);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_ENEMY_ALERT_ENDED, onEnemyAlertEnded);
            m_PatrolManager.destroy();
            m_EnemyDamageIndicatorManager.destroy();
            s_Instance = null;
        }

        /// <summary>
        /// Returns enemy pool of given type if exists in the dictionary
        /// </summary>
        /// <param name="a_EnemyType"></param>
        /// <returns></returns>
        private EnemyPool getPoolOfEnemyType(ENEMY_TYPE a_EnemyType)
        {
            EnemyPool l_EnemyPool = null;
            if (!m_dictEnemyPools.TryGetValue(a_EnemyType, out l_EnemyPool))
            {
                Debug.LogError("EnemyManager::getPoolOfEnemyType:: Enemy Pool of type '"+ a_EnemyType.ToString() + "' does not exist in the registered enemy pool dictionary.");
            }
            return l_EnemyPool;
        }

        /// <summary>
        /// returns enemy of type from its pool
        /// </summary>
        /// <param name="a_Enemytype"></param>
        /// <returns></returns>
        public static EnemyBase GetEnemyFromPool(ENEMY_TYPE a_Enemytype, string a_strID, Vector3 a_v3Pos, Quaternion a_quatRot)
        {
            EnemyPool l_EnemyPool = s_Instance.getPoolOfEnemyType(a_Enemytype);
            EnemyBase l_Enemy = null;
            if (l_EnemyPool != null)
            {
                l_Enemy = l_EnemyPool.getObject();
                l_Enemy.setID(a_strID);
                l_Enemy.transform.position = a_v3Pos;
                l_Enemy.transform.rotation = a_quatRot;
                l_Enemy.activateEnemy();

                if (s_Instance.m_bIsEnemiesPaused)
                {
                    l_Enemy.pauseEnemy();
                }
                else
                {
                    l_Enemy.unpauseEnemy();
                }

                ToggleAcitvateEnemyDependant(true, a_strID);
            }
            return l_Enemy;
        }

        /// <summary>
        /// Returns active enemy back into the pool
        /// </summary>
        /// <param name="a_Enemy"></param>
        public static void ReturnActiveEnemyToPool(ENEMY_TYPE a_EnemyType, string a_strEnemyID)
        {
            EnemyPool l_EnemyPool = s_Instance.getPoolOfEnemyType(a_EnemyType);
            List<EnemyBase> l_lstAcitveEnemies = l_EnemyPool.getActiveList();
            int l_iActiveEnemyCount = l_lstAcitveEnemies.Count;

            for (int l_iActiveIndex = 0; l_iActiveIndex < l_iActiveEnemyCount; l_iActiveIndex++)
            {
                if (l_lstAcitveEnemies[l_iActiveIndex].getID().Equals(a_strEnemyID, System.StringComparison.OrdinalIgnoreCase))
                {
                    s_Instance.returnEnemyToPool(l_EnemyPool, l_lstAcitveEnemies[l_iActiveIndex]);
                    break;
                }
            }
        }

        /// <summary>
        /// Returns all enemies back into its respective pools
        /// </summary>
        public void returnAllToPool()
        {
            foreach (KeyValuePair<ENEMY_TYPE, EnemyPool> l_EnemyPool in m_dictEnemyPools)
            {
                List<EnemyBase> l_lstActiveEnemyBase = l_EnemyPool.Value.getActiveList();
                int l_iActiveEnemyCount = l_lstActiveEnemyBase.Count;
                for (int l_iEnemyIndex = l_iActiveEnemyCount - 1; l_iEnemyIndex >= 0; l_iEnemyIndex--)
                {
                    returnEnemyToPool(l_EnemyPool.Value, l_lstActiveEnemyBase[l_iEnemyIndex]);
                }
            }
        }

        /// <summary>
        /// Returns given enemy to enemy pool
        /// </summary>
        /// <param name="a_EnemyPool"></param>
        /// <param name="a_EnemyBase"></param>
        private void returnEnemyToPool(EnemyPool a_EnemyPool, EnemyBase a_EnemyBase)
        {
            a_EnemyBase.deactivateEnemy();
            a_EnemyPool.returnToPool(a_EnemyBase);
        }

        public static void ReturnAllToPool()
        {
            s_Instance.returnAllToPool();
        }

        /// <summary>
        /// Pauses or Unpauses enemy movement/ action
        /// </summary>
        /// <param name="a_bIsPaused"></param>
        private void pauseAllEnemies(bool a_bIsPaused)
        {
            if (a_bIsPaused == m_bIsEnemiesPaused)
            {
                return;
            }
            s_Instance.m_bIsEnemiesPaused = a_bIsPaused;
            foreach (KeyValuePair<ENEMY_TYPE, EnemyPool> l_EnemyPool in m_dictEnemyPools)
            {
                List<EnemyBase> l_lstActiveEnemies = l_EnemyPool.Value.getActiveList();
                int l_iActiveEnemyCount = l_lstActiveEnemies.Count;

                for (int l_iEnemyIndex = 0; l_iEnemyIndex < l_iActiveEnemyCount; l_iEnemyIndex++)
                {
                    EnemyBase l_EnemyBase = l_lstActiveEnemies[l_iEnemyIndex];
                    if (a_bIsPaused)
                    {
                        l_EnemyBase.pauseEnemy();
                    }
                    else
                    {
                        l_EnemyBase.unpauseEnemy();
                    }
                }
            }
        }

        /// <summary>
        /// event called on game is paused / unpaused
        /// </summary>
        /// <param name="a_EventHash"></param>
        public void onGamePauseToggled(EventHash a_EventHash)
        {
            bool a_bIsGamePaused = (bool)a_EventHash[GameEventTypeConst.ID_GAME_PAUSED];
            pauseAllEnemies(a_bIsGamePaused);
        }

        /// <summary>
        /// Event called on gameplay ended
        /// </summary>
        /// <param name="a_EventHash"></param>
        public void onGameplayEnded(EventHash a_EventHash)
        {
            returnAllToPool();
        }

        /// <summary>
        /// Called on an enemy is killed
        /// </summary>
        /// <param name="a_EnemyBase"></param>
        public static void OnEnemyKilled(EnemyBase a_EnemyBase)
        {
            ObjectiveManager.TriggerObjective(ENEMY_OBJECTIVE_ID);
        }

        /// <summary>
        /// Callback called on an enemy being alerted
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onEnemyAlertStarted(EventHash a_EventHash)
        {
            bool l_bIsForcedEnemyAlert =  (bool)a_EventHash[GameEventTypeConst.ID_FORCED_ENEMY_ALERT];

            int l_iOldAlertedEnemyCount = m_lstAlertedEnemies.Count;
            int l_iNewAlertedEnemyCount = l_iOldAlertedEnemyCount;

            ///if an enemy has actually been alerted by finding the player then alert, else if it is only forced then enemy will be null
            if (!l_bIsForcedEnemyAlert)
            {
                EnemyBase l_AlertedEnemy = (EnemyBase)a_EventHash[GameEventTypeConst.ID_ENEMY_BASE];
                if (l_AlertedEnemy != null)
                {
                    m_lstAlertedEnemies.Add(l_AlertedEnemy);
                }

                l_iNewAlertedEnemyCount = m_lstAlertedEnemies.Count;
            }
            onEnemyAlertListChanged(l_iOldAlertedEnemyCount, l_iNewAlertedEnemyCount);
        }

        /// <summary>
        /// Callback called on an enemy's state changed from alert state
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onEnemyAlertEnded(EventHash a_EventHash)
        {
            int l_iOldAlertedEnemyCount = m_lstAlertedEnemies.Count;
            int l_iNewAlertedEnemyCount = l_iOldAlertedEnemyCount;

            EnemyBase l_AlertedEnemy = (EnemyBase)a_EventHash[GameEventTypeConst.ID_ENEMY_BASE];
            if (l_AlertedEnemy != null)
            {
                m_lstAlertedEnemies.Remove(l_AlertedEnemy);
            }

            l_iNewAlertedEnemyCount = m_lstAlertedEnemies.Count;
            onEnemyAlertListChanged(l_iOldAlertedEnemyCount, l_iNewAlertedEnemyCount);
        }

        /// <summary>
        /// Called on the list of enemy list changed, fire event 
        /// </summary>
        private void onEnemyAlertListChanged(int a_iOldEnemyAlertedCount, int a_iNewEnemyAlertedCount)
        {
            if (a_iOldEnemyAlertedCount != a_iNewEnemyAlertedCount)
            {
                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_OLD_ENEMY_ALERT_COUNT, a_iOldEnemyAlertedCount);
                l_EventHash.Add(GameEventTypeConst.ID_NEW_ENEMY_ALERT_COUNT, a_iNewEnemyAlertedCount);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_ENEMY_ALERT_COUNT_CHANGED, l_EventHash);
            }
        }

        /// <summary>
        /// Registers/ Unregisters the enemy dependant base from the list
        /// </summary>
        /// <param name="a_bIsRegister"></param>
        /// <param name="a_EnemyDependantBase"></param>
        public static void RegisterUnregisterEnemyDependant(bool a_bIsRegister, EnemyDependantBase a_EnemyDependantBase)
        {
            if(s_Instance == null) { return; }

            if (a_bIsRegister)
            {
                s_Instance.m_lstEnemyDependantBase.Add(a_EnemyDependantBase);
            }
            else
            {
                s_Instance.m_lstEnemyDependantBase.Remove(a_EnemyDependantBase);
            }
        }

        /// <summary>
        /// Activates/ Deactivates the enemy dependant with Enemy ID if exist
        /// </summary>
        /// <param name="a_bIsActivated"></param>
        /// <param name="a_strEnemyID"></param>
        public static void ToggleAcitvateEnemyDependant(bool a_bIsActivated, string a_strEnemyID)
        {
            int l_iEnemyDependantCount = s_Instance.m_lstEnemyDependantBase.Count;
            for (int l_iEnemyDependantIndex = 0; l_iEnemyDependantIndex < l_iEnemyDependantCount; l_iEnemyDependantIndex++)
            {
                if (s_Instance.m_lstEnemyDependantBase[l_iEnemyDependantIndex].EnemyID.Equals(a_strEnemyID, System.StringComparison.OrdinalIgnoreCase))
                {
                    s_Instance.m_lstEnemyDependantBase[l_iEnemyDependantIndex].onActivate();
                    break;
                }
            }
        }

        /// <summary>
        /// Gets active enemy base with ID from the pool of a_EnemyType
        /// </summary>
        /// <param name="a_EnemyType"></param>
        /// <param name="a_strEnemyID"></param>
        /// <returns></returns>
        public static EnemyBase GetActiveEnemyWithID(ENEMY_TYPE a_EnemyType, string a_strEnemyID)
        {
            EnemyPool l_EnemyPool = null;
            if (s_Instance.m_dictEnemyPools.TryGetValue(a_EnemyType, out l_EnemyPool))
            {
                List<EnemyBase> l_lstActiveEnemyBase = l_EnemyPool.getActiveList();
                int l_iActiveEnemyCount = l_lstActiveEnemyBase.Count;
                for (int l_iEnemyIndex = l_iActiveEnemyCount - 1; l_iEnemyIndex >= 0; l_iEnemyIndex--)
                {
                    if (l_lstActiveEnemyBase[l_iEnemyIndex].getID().Equals(a_strEnemyID, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return l_lstActiveEnemyBase[l_iEnemyIndex];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Forces all enemy to be alerted if their conditions marching proximity to the player are true
        /// </summary>
        public static void ForceAllEnemyAlertOnProximity()
        {
            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_ENEMY_BASE, null);
            l_EventHash.Add(GameEventTypeConst.ID_FORCED_ENEMY_ALERT, true);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_ENEMY_ALERT_STARTED, l_EventHash);
        }
    }
}