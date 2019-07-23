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
        /// Action called on update, will update the current state the enemy is in
        /// </summary>
        private System.Action m_actNavStateUpdate = null;

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

            if (m_actNavStateUpdate != null)
            {
                m_actNavStateUpdate();
            }
        }

        /// <summary>
        /// update action called when the enemy is in the idle state 
        /// </summary>
        protected virtual void onIdleStateUpdate()
        {
            Vector3 l_v3PlayerPos = PlayerManager.GetPosition();
            m_NavMeshAgent.CalculatePath(l_v3PlayerPos, m_NavMeshPath);
            if ((m_NavMeshPath.status == UnityEngine.AI.NavMeshPathStatus.PathComplete) &&
                (m_NavMeshAgent.remainingDistance <= m_fAttackRadius))
            {
                NavState = NON_STATIC_ENEMY_STATE.ALERT;
            }
            else
            {
                m_Animator.SetTrigger(ANIM_TRIGGER_IDLE);
                //EnemyManager.SetPatrolDestination(this, null);
            }
        }

        /// <summary>
        /// update action called when the enemy is in the alert state 
        /// </summary>
        protected virtual void onAlertStateUpdate()
        {
            Vector3 l_v3PlayerPos = PlayerManager.GetPosition();
            m_NavMeshAgent.CalculatePath(l_v3PlayerPos, m_NavMeshPath);
            if ((m_NavMeshPath.status == UnityEngine.AI.NavMeshPathStatus.PathComplete))
            {
                if (m_NavMeshAgent.remainingDistance <= m_fStrikeDistance)
                {
                    m_Animator.SetTrigger(ANIM_TRIGGER_ATTACK);
                }
                else if (m_NavMeshAgent.remainingDistance <= m_fAttackRadius)
                {
                    m_Animator.SetTrigger(ANIM_TRIGGER_WALK);
                    m_NavMeshAgent.SetDestination(l_v3PlayerPos);
                }
                else
                {
                    NavState = NON_STATIC_ENEMY_STATE.IDLE;
                }
            }
            else
            {
                NavState = NON_STATIC_ENEMY_STATE.IDLE;
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

        /// <summary>
        /// On enemy state changed
        /// </summary>
        protected override void onStateChanged(NON_STATIC_ENEMY_STATE a_NavState)
        {
            switch (a_NavState)
            {
                case NON_STATIC_ENEMY_STATE.IDLE:
                    {
                        m_actNavStateUpdate = onIdleStateUpdate;
                        m_NavMeshAgent.SetDestination(PlayerManager.GetPosition());
                        break;
                    }
                case NON_STATIC_ENEMY_STATE.ALERT:
                    {
                        m_actNavStateUpdate = onAlertStateUpdate;
                        
                        break;
                    }
                case NON_STATIC_ENEMY_STATE.DEAD:
                    {
                        m_actNavStateUpdate = null;
                        m_Animator.SetTrigger(ANIM_TRIGGER_DIE);
                        break;
                    }
                case NON_STATIC_ENEMY_STATE.NONE:
                    {
                        m_actNavStateUpdate = null;
                        break;
                    }
            }
        }
    }
}