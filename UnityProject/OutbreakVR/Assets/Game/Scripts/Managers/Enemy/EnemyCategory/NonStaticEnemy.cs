using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum NON_STATIC_ENEMY_STATE
    {
        NONE,
        IDLE,
        ALERT,
        DEAD
    }

    public abstract class NonStaticEnemy : EnemyBase
    {

        protected const string ANIM_TRIGGER_WALK      = "Walk";
        protected const string ANIM_TRIGGER_ATTACK    = "Attack";
        protected const string ANIM_TRIGGER_DIE       = "Die";
        protected const string ANIM_TRIGGER_IDLE      = "Idle";

        /// <summary>
        /// The nav mesh agent of this body to manage movement
        /// </summary>
        [SerializeField]
        protected UnityEngine.AI.NavMeshAgent m_NavMeshAgent = null;

        /// <summary>
        /// The nav mesh path of this body to manage movement towards the player
        /// </summary>
        protected UnityEngine.AI.NavMeshPath m_NavMeshPath = null;

        [SerializeField]
        protected Animator m_Animator = null;

        /// <summary>
        /// The state of the enemy
        /// </summary>
        private NON_STATIC_ENEMY_STATE m_NavState = NON_STATIC_ENEMY_STATE.NONE;
        protected NON_STATIC_ENEMY_STATE NavState
        {
            get { return m_NavState; }
            set
            {
                if (m_NavState == value)
                {
                    return;
                }
                m_NavState = value;
                onStateChanged(m_NavState);
            }
        }

        public override void activateEnemy()
        {
            base.activateEnemy();
            if (m_NavMeshPath == null) { m_NavMeshPath = new UnityEngine.AI.NavMeshPath();}

            m_NavMeshAgent.Warp(transform.position);
            NavState = NON_STATIC_ENEMY_STATE.IDLE;
        }

        public override void deactivateEnemy()
        {
            base.deactivateEnemy();
            NavState = NON_STATIC_ENEMY_STATE.NONE;
        }

        public override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// Starts navigation of the enemy
        /// </summary>
        public void startNavigation()
        {
            m_NavMeshAgent.isStopped = false;
        }

        /// <summary>
        /// Stops navigation of the enemy
        /// </summary>
        public void stopNavigation()
        {
            m_NavMeshAgent.isStopped = true;
        }

        /// <summary>
        /// Pauses enemy movement or action
        /// </summary>
        public override void pauseEnemy()
        {
            base.pauseEnemy();
            stopNavigation();
        }

        /// <summary>
        /// Unpauses enemy movement and action
        /// </summary>
        public override void unpauseEnemy()
        {
            base.unpauseEnemy();
            startNavigation();
        }

        /// <summary>
        /// On enemy state changed
        /// </summary>
        protected virtual void onStateChanged(NON_STATIC_ENEMY_STATE a_NavState)
        {

        }

        /// <summary>
        /// called on killed fire event 
        /// </summary>
        protected override void onKilled()
        {
            base.onKilled();
            m_NavState = NON_STATIC_ENEMY_STATE.DEAD;
        }
    }
}