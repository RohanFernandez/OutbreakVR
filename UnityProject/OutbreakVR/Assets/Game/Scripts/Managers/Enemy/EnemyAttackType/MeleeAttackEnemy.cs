using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class MeleeAttackEnemy : NonStaticEnemy
    {
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
        /// Updates movements
        /// </summary>
        public override void Update()
        {
            base.Update();

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
    }
}