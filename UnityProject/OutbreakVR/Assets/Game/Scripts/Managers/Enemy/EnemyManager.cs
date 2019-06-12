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
        /// Sets the singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

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
        public static EnemyBase GetEnemyFromPool(ENEMY_TYPE a_Enemytype)
        {
            EnemyPool l_EnemyPool = s_Instance.getPoolOfEnemyType(a_Enemytype);
            EnemyBase l_Enemy = null;
            if (l_EnemyPool != null)
            {
                l_Enemy = l_EnemyPool.getObject();
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
        /// Return enemy back into its respective pool
        /// </summary>
        /// <param name="a_Enemy"></param>
        private void returnToPool(EnemyBase a_Enemy)
        {
            EnemyPool l_EnemyPool = getPoolOfEnemyType(a_Enemy.getEnemyType());
            if (l_EnemyPool != null)
            {
                l_EnemyPool.returnToPool(a_Enemy);
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
        /// Static function to Pause or Unpause enemy movement/ action
        /// </summary>
        /// <param name="a_bIsPaused"></param>
        public static void PauseAllEnemies(bool a_bIsPaused)
        {
            s_Instance.pauseAllEnemies(a_bIsPaused);
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
    }
}