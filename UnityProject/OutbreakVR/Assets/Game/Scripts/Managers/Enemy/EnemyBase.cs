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
        /// Is the enemy active to attack
        /// </summary>
        [SerializeField]
        protected bool m_bIsEnemyReady = false;

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
            m_bIsEnemyReady = true;
        }

        /// <summary>
        /// Deactivates use of enemy
        /// </summary>
        public virtual void deactivateEnemy()
        {
            m_bIsEnemyReady = false;
        }

        public virtual void Update()
        {

        }
    }
}