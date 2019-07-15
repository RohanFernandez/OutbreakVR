using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum ENEMY_TYPE
    {
        SECURITY_OFFICER
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
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_PAUSED_TOGGLED, onGamePauseToggled);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, onGameplayEnded);

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
        public static EnemyBase GetEnemyFromPool(ENEMY_TYPE a_Enemytype, string a_strID)
        {
            EnemyPool l_EnemyPool = s_Instance.getPoolOfEnemyType(a_Enemytype);
            EnemyBase l_Enemy = null;
            if (l_EnemyPool != null)
            {
                l_Enemy = l_EnemyPool.getObject();
                l_Enemy.setID(a_strID);
                l_Enemy.gameObject.SetActive(true);

                if (s_Instance.m_bIsEnemiesPaused)
                {
                    l_Enemy.pauseEnemy();
                }
                else
                {
                    l_Enemy.unpauseEnemy();
                }
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
                    l_EnemyPool.returnToPool(l_lstAcitveEnemies[l_iActiveIndex]);
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
                l_EnemyPool.Value.returnAll();
            }
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
            EventHash l_hash = EventManager.GetEventHashtable();
            l_hash.Add(GameEventTypeConst.ID_OBJECTIVE_TRIGGER_ID, ENEMY_OBJECTIVE_ID);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_LEVEL_OBJECTIVE_TRIGGERED, l_hash);
        }
    }
}