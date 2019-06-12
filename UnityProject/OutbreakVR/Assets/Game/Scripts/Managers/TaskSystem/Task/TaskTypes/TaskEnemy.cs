using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskEnemy : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_ENEMY_TYPE = "ItemType";
        private const string ATTRIBUTE_POSITION = "Position";
        private const string ATTRIBUTE_CODE = "Code";

        private const string ATTRIBUTE_VALUE_CODE_RETURN_ALL = "ReturnAll";
        private const string ATTRIBUTE_VALUE_CODE_PAUSE_ALL = "PauseAll";
        private const string ATTRIBUTE_VALUE_CODE_UNPAUSE_ALL = "UnpauseAll";
        #endregion ATTRIBUTE_KEY

        /// <summary>
        /// The enemy type to set in the environment
        /// </summary>
        private ENEMY_TYPE m_EnemyType;

        /// <summary>
        /// code of instructions
        /// </summary>
        private string m_strCode = string.Empty;

        /// <summary>
        /// The position to spawn the item
        /// </summary>
        private Vector3 m_v3Position = Vector3.zero;

        public override void onInitialize()
        {
            base.onInitialize();

            string l_strEnemyType = getString(ATTRIBUTE_ENEMY_TYPE);
            m_strCode = getString(ATTRIBUTE_CODE);
            m_v3Position = getVec3(ATTRIBUTE_POSITION);

            if (!string.IsNullOrEmpty(l_strEnemyType))
            {
                m_EnemyType = (ENEMY_TYPE)System.Enum.Parse(typeof(ENEMY_TYPE), l_strEnemyType);
            }
        }

        public override void onExecute()
        {
            base.onExecute();

            if (string.IsNullOrEmpty(m_strCode))
            {
                EnemyBase l_Enemy = EnemyManager.GetEnemyFromPool(m_EnemyType);
                l_Enemy.transform.position = m_v3Position;
                l_Enemy.gameObject.SetActive(true);
            }
            else
            {
                switch (m_strCode)
                {
                    case ATTRIBUTE_VALUE_CODE_RETURN_ALL:
                        {
                            EnemyManager.ReturnAllToPool();
                            break;
                        }
                    case ATTRIBUTE_VALUE_CODE_PAUSE_ALL:
                        {
                            EnemyManager.PauseAllEnemies(true);
                            break;
                        }
                    case ATTRIBUTE_VALUE_CODE_UNPAUSE_ALL:
                        {
                            EnemyManager.PauseAllEnemies(false);
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }
            }
            onComplete();
        }
    }
}