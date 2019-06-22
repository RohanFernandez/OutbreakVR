using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class EnemyBase : MonoBehaviour, IEnemy
    {
        /// <summary>
        /// The type of the enemy
        /// Can be identified
        /// </summary>
        [SerializeField]
        protected ENEMY_TYPE m_EnemyType;

        /// <summary>
        /// The ID to refer this enemy
        /// </summary>
        [SerializeField]
        private string m_strEnemyID = string.Empty;

        /// <summary>
        /// Is the enemy active to attack
        /// </summary>
        [SerializeField]
        protected bool m_bIsActivated = false;

        /// <summary>
        /// The radius inside, if the player comes within this radius the enemy will start attacking
        /// </summary>
        [SerializeField]
        protected float m_fAttackRadius = 15.0f;

        /// <summary>
        /// Returns the tyoe of the enemy
        /// </summary>
        /// <returns></returns>
        public ENEMY_TYPE getEnemyType()
        {
            return m_EnemyType;
        }

        /// <summary>
        /// Returns the type of attack the enemy will possess
        /// </summary>
        /// <returns></returns>
        public abstract ENEMY_ATTACK_TYPE getEnemyAttackType();

        /// <summary>
        /// Activates use of enemy
        /// </summary>
        public virtual void activateEnemy()
        {
            m_bIsActivated = true;
        }

        /// <summary>
        /// Deactivates use of enemy
        /// </summary>
        public virtual void deactivateEnemy()
        {
            m_bIsActivated = false;
        }

        public virtual void Update()
        {

        }

        public virtual void onReturnedToPool()
        {
            deactivateEnemy();
        }

        public virtual void onRetrievedFromPool()
        {
            activateEnemy();
        }

        /// <summary>
        /// Pauses enemy movement or action
        /// </summary>
        public virtual void pauseEnemy()
        {

        }

        /// <summary>
        /// Unpauses enemy movement and action
        /// </summary>
        public virtual void unpauseEnemy()
        {

        }

        public string getID()
        {
            return m_strEnemyID;
        }

        /// <summary>
        /// Sets ID
        /// </summary>
        /// <param name="a_strID"></param>
        /// <returns></returns>
        public void setID(string a_strID)
        {
            m_strEnemyID = a_strID;
        }
    }
}