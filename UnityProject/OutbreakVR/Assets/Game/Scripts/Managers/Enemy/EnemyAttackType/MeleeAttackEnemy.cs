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

        /// <summary>
        /// True if dead
        /// </summary>
        private bool m_bIsDead = false;

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
            m_bIsDead = false;
        }

        /// <summary>
        /// deactivates use of enemy
        /// </summary>
        public override void deactivateEnemy()
        {
            base.deactivateEnemy();
        }

        /// <summary>
        /// Updates movements
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (m_bIsDead)
            {
                return;
            }

            if (m_bIsActivated)
            {
                float l_fDistanceToPlayer = Vector3.Distance(PlayerManager.GetPosition(), transform.position);
                if (l_fDistanceToPlayer < m_fAttackRadius)
                {
                    if (l_fDistanceToPlayer <= m_fStrikeDistance)
                    {
                        m_Animator.SetTrigger(ANIM_TRIGGER_ATTACK);
                    }
                    else
                    {
                        m_Animator.SetTrigger(ANIM_TRIGGER_WALK);
                        m_NavMeshAgent.SetDestination(PlayerManager.GetPosition());
                    }
                }
                else
                {
                    m_Animator.SetTrigger(ANIM_TRIGGER_IDLE);
                }
            }
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
            m_bIsDead = true;
            base.onKilled();
            m_Animator.SetTrigger(ANIM_TRIGGER_DIE);
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