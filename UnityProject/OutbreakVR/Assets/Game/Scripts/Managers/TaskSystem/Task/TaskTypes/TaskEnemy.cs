using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskEnemy : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_ENEMY_TYPE = "ItemType";
        private const string ATTRIBUTE_IS_RETURN_ALL = "IsReturnAll";
        private const string ATTRIBUTE_POSITION = "Position";
        #endregion ATTRIBUTE_KEY

        /// <summary>
        /// The enemy type to set in the environment
        /// </summary>
        private ENEMY_TYPE m_EnemyType;

        /// <summary>
        /// If true returns all items back into the pool
        /// </summary>
        private bool m_bIsReturnAllItems = false;

        /// <summary>
        /// The position to spawn the item
        /// </summary>
        private Vector3 m_v3Position = Vector3.zero;

        public override void onInitialize()
        {
            base.onInitialize();

            string l_strEnemyType = getString(ATTRIBUTE_ENEMY_TYPE);
            m_bIsReturnAllItems = getBool(ATTRIBUTE_IS_RETURN_ALL);
            m_v3Position = getVec3(ATTRIBUTE_POSITION);

            if (!string.IsNullOrEmpty(l_strEnemyType))
            {
                m_EnemyType = (ENEMY_TYPE)System.Enum.Parse(typeof(ENEMY_TYPE), l_strEnemyType);
            }
        }

        public override void onExecute()
        {
            base.onExecute();

            if (m_bIsReturnAllItems)
            {
                EnemyManager.ReturnAllToPool();
            }
            else
            {
                EnemyBase l_Enemy = EnemyManager.GetEnemyFromPool(m_EnemyType);
                l_Enemy.transform.position = m_v3Position;
                l_Enemy.gameObject.SetActive(true);
            }
            onComplete();
        }
    }
}