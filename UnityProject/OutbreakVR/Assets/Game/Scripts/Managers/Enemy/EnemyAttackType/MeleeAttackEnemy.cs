using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class MeleeAttackEnemy : NonStaticEnemy
    {
        /// <summary>
        /// Damage inflicted on player on strike
        /// </summary>
        [SerializeField]
        private int m_iStrikeDamage = 10;

        /// <summary>
        /// Ref to the audio source
        /// </summary>
        [SerializeField]
        private UnpooledAudioSource m_ManagedAudioSource = null;

        /// <summary>
        /// Distance between the player and enemy below which the enemy will start striking
        /// </summary>
        private float m_fStrikeDistance = 2.0f;

        public override ENEMY_ATTACK_TYPE getEnemyAttackType()
        {
            return ENEMY_ATTACK_TYPE.MELEE;
        }

        /// <summary>
        /// Activates use of enemy
        /// </summary>
        public override void activateEnemy()
        {
            base.activateEnemy();
        }

        /// <summary>
        /// deactivates use of enemy
        /// </summary>
        public override void deactivateEnemy()
        {
            base.deactivateEnemy();
        }

        /// <summary>
        /// update action called when the enemy is in the idle state 
        /// </summary>
        protected override void onIdleStateUpdate()
        {
            base.onIdleStateUpdate();
        }

        /// <summary>
        /// update action called when the enemy is in the alert state 
        /// </summary>
        protected override void onAlertStateUpdate()
        {
            base.onAlertStateUpdate();
        }

        void OnDrawGizmoSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,m_fAttackRadius) ;
        }

        /// <summary>
        /// Called on killed
        /// </summary>
        protected override void onKilled()
        {
            base.onKilled();
        }

        /// <summary>
        /// on enemy strike attack attempt to hit the player
        /// </summary>
        public override void onStrikeAttack()
        {
            Vector3 l_v3PlayerPosition = PlayerManager.GetPosition();

            float l_fDistance = Vector3.Distance(transform.position, l_v3PlayerPosition);

            Vector3 l_v3EnemyToPlayerDirection = Vector3.Normalize(l_v3PlayerPosition - transform.position);
            float l_v3EnemyToPlayerDot = Vector3.Dot(l_v3EnemyToPlayerDirection, transform.forward);

            if (l_fDistance <= m_fStrikeDistance &&
                l_v3EnemyToPlayerDot > 0.6f)
            {
                PlayerManager.InflictDamage(m_iStrikeDamage);
            }
        }
    }
}