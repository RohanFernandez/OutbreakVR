using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskEnemy : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_ENEMY_TYPE = "EnemyType";
        private const string ATTRIBUTE_POSITION = "Position";
        private const string ATTRIBUTE_ROTATION = "Rotation";
        private const string ATTRIBUTE_ENEMY_ID = "Enemy_ID";
        private const string ATTRIBUTE_CODE = "Code";

        private const string ATTRIBUTE_VALUE_CODE_RETURN_ALL = "ReturnAll";
        private const string ATTRIBUTE_VALUE_CODE_DEACTIVATE = "Deactivate";
        private const string ATTRIBUTE_VALUE_CODE_ACTIVATE = "Activate";
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
        /// <summary>
        /// The rotation to spawn the item
        /// </summary>
        private Vector3 m_v3Rotation = Vector3.zero;

        /// <summary>
        /// ID of the gameobject
        /// </summary>
        private string m_strEnemyID = string.Empty;

        public override void onInitialize()
        {
            base.onInitialize();

            string l_strEnemyType = getString(ATTRIBUTE_ENEMY_TYPE);
            m_strCode = getString(ATTRIBUTE_CODE);
            m_v3Position = getVec3(ATTRIBUTE_POSITION);
            m_v3Rotation = getVec3(ATTRIBUTE_ROTATION);
            m_strEnemyID = getString(ATTRIBUTE_ENEMY_ID);

            if (!string.IsNullOrEmpty(l_strEnemyType))
            {
                m_EnemyType = (ENEMY_TYPE)System.Enum.Parse(typeof(ENEMY_TYPE), l_strEnemyType);
            }
        }

        public override void onExecute()
        {
            base.onExecute();

            switch (m_strCode)
            {
                case ATTRIBUTE_VALUE_CODE_RETURN_ALL:
                    {
                        EnemyManager.ReturnAllToPool();
                        break;
                    }
                case ATTRIBUTE_VALUE_CODE_DEACTIVATE:
                    {
                        EnemyManager.ReturnActiveEnemyToPool(m_EnemyType, m_strEnemyID);
                        break;
                    }
                case ATTRIBUTE_VALUE_CODE_ACTIVATE:
                    {
                        EnemyBase l_Enemy = EnemyManager.GetEnemyFromPool(m_EnemyType, m_strEnemyID, m_v3Position, Quaternion.Euler(m_v3Rotation));
                        break;
                    }
                default:
                    {
                        break;
                    }

            }

            onComplete();
        }
    }
}