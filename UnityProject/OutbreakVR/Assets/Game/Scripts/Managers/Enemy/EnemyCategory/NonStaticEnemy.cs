using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum NON_STATIC_ENEMY_STATE
    {
        NONE,
        IDLE,
        PATROL,
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
        public UnityEngine.AI.NavMeshAgent NavMeshAgent
        {
            get { return m_NavMeshAgent; }
        }
        
        /// <summary>
        /// Path of this agent
        /// </summary>
        protected UnityEngine.AI.NavMeshPath m_NavMeshPath = null;
        public UnityEngine.AI.NavMeshPath NavMeshPath
        {
            get { return m_NavMeshPath; }
        }

        [SerializeField]
        protected Animator m_Animator = null;

        [SerializeField]
        protected EnemyRangeDetector m_RangeDetector = null;

        /// <summary>
        /// Returns all patrol points of the enemy range detector which are within range
        /// </summary>
        /// <returns></returns>
        public List<EnemyPatrolPoint> getPatrolPointsWithinRange()
        {
            return m_RangeDetector.m_lstPatrolPointsWithinRange;
        }

        /// <summary>
        /// Time that will be spent in the idle state and then transitioning to patrol
        /// </summary>
        [SerializeField]
        private float m_fMaxIdleTime = 5.0f;

        /// <summary>
        /// Current time passed that is spent in the idle state
        /// </summary>
        [SerializeField]
        private float m_fCurrIdleTimeCounter = 0.0f;

        /// <summary>
        /// max Time in alert modeafter which alert of that enemy will go off
        /// </summary>
        [SerializeField]
        private float m_fMaxAlertTime = 0.0f;

        /// <summary>
        /// Time in alert mode, will start since last scene the player
        /// </summary>
        [SerializeField]
        private float m_fCurrAlertTimeCounter = 0.0f;

        /// <summary>
        /// Stopping distance when patrolling towards a patrol point
        /// </summary>
        [SerializeField]
        private float m_fPatrolStoppingDistance = 2.0f;

        /// <summary>
        /// Stopping distance when attacking an enemy
        /// </summary>
        [SerializeField]
        private float m_fAlertStoppingDistance = 1.0f;

        /// <summary>
        /// The state of the enemy
        /// </summary>
        private NON_STATIC_ENEMY_STATE m_NavState = NON_STATIC_ENEMY_STATE.NONE;

        /// <summary>
        /// The next patrol point the enemy is traversing to
        /// </summary>
        private EnemyPatrolPoint m_NextPatrolDestination = null;

        /// <summary>
        /// Action called on update, will update the current state the enemy is in
        /// </summary>
        private System.Action m_actNavStateUpdate = null;

        /// <summary>
        /// Radius under which will go into alert mode attempting to reach the player to attack.
        /// </summary>
        [SerializeField]
        private float m_fAlertRadius = 15.0f;

        /// <summary>
        /// The ray that detects from the transform to the player to check if the player is in the line of sight
        /// </summary>
        private Ray m_RayDetector = new Ray();

        protected NON_STATIC_ENEMY_STATE NavState
        {
            get { return m_NavState; }
            set
            {
                Debug.LogError("onStateChanged =  OldState: " + NavState.ToString() + "  NewState: " + value.ToString());
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

            if (m_NavMeshPath == null) { m_NavMeshPath = new UnityEngine.AI.NavMeshPath(); }
            m_RangeDetector.onActivated();

            m_NavMeshAgent.Warp(transform.position);
            StartCoroutine(coOnActivated());
        }

        private IEnumerator coOnActivated()
        {
            yield return new WaitForSeconds(0.1f);
            NavState = NON_STATIC_ENEMY_STATE.IDLE;
        }

        public override void deactivateEnemy()
        {
            base.deactivateEnemy();
            m_RangeDetector.onDeactivated();

            NavState = NON_STATIC_ENEMY_STATE.NONE;
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
        /// called on killed fire event 
        /// </summary>
        protected override void onKilled()
        {
            base.onKilled();
            m_NavState = NON_STATIC_ENEMY_STATE.DEAD;
        }

        /// <summary>
        /// Patrols to the next patrol point
        /// a_bIsNextPatrolPointReached is the agent currently at an enemy patrol position
        /// </summary>
        protected void patrolToPoint(bool a_bIsNextPatrolPointReached)
        {
            EnemyPatrolPoint l_CurrentPatrolPoint = null;
            if (a_bIsNextPatrolPointReached)
            {
                l_CurrentPatrolPoint = m_NextPatrolDestination;    
            }
            m_NextPatrolDestination = PatrolManager.GetNextPatrolPoint(this, l_CurrentPatrolPoint);

            if (m_NextPatrolDestination == null)
            {
                setDestination(transform.position);
            }
            else
            {
                setDestination(m_NextPatrolDestination.transform.position);
            }
        }

        /// <summary>
        /// The target to set the destination of the nav mesh agent
        /// Can be the player's position or a patrol point
        /// </summary>
        /// <param name="a_v3TargetPosition"></param>
        protected void setDestination(Vector3 a_v3TargetPosition)
        {
            m_NavMeshAgent.SetDestination(a_v3TargetPosition);
        }

        /// <summary>
        /// On enemy state changed
        /// </summary>
        protected virtual void onStateChanged(NON_STATIC_ENEMY_STATE a_NavState)
        {
            switch (a_NavState)
            {
                case NON_STATIC_ENEMY_STATE.IDLE:
                    {
                        m_fCurrAlertTimeCounter = 0.0f;
                        m_fCurrIdleTimeCounter = 0.0f;
                        m_NavMeshAgent.ResetPath();
                        m_Animator.ResetTrigger(ANIM_TRIGGER_WALK);
                        m_Animator.SetTrigger(ANIM_TRIGGER_IDLE);
                        m_actNavStateUpdate = onIdleStateUpdate;
                        break;
                    }
                case NON_STATIC_ENEMY_STATE.PATROL:
                    {
                        m_NavMeshAgent.stoppingDistance = m_fPatrolStoppingDistance;
                        m_Animator.ResetTrigger(ANIM_TRIGGER_IDLE);
                        m_Animator.SetTrigger(ANIM_TRIGGER_WALK);
                        patrolToPoint(m_NextPatrolDestination != null);
                        m_actNavStateUpdate = onPatrolStateUpdate;
                        break;
                    }
                case NON_STATIC_ENEMY_STATE.ALERT:
                    {
                        m_NavMeshAgent.ResetPath();
                        m_NavMeshAgent.stoppingDistance = m_fAlertStoppingDistance;
                        m_fCurrAlertTimeCounter = m_fMaxAlertTime;
                        m_NextPatrolDestination = null;
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
                        m_NextPatrolDestination = null;
                        m_actNavStateUpdate = null;
                        break;
                    }
            }
        }

        /// <summary>
        /// update action called when the enemy is in the idle state 
        /// </summary>
        protected virtual void onIdleStateUpdate()
        {
            m_fCurrIdleTimeCounter += Time.deltaTime;

            if (isPlayerDetected())
            {
                NavState = NON_STATIC_ENEMY_STATE.ALERT;
            }
            else if (m_fCurrIdleTimeCounter > m_fMaxIdleTime)
            {
                NavState = NON_STATIC_ENEMY_STATE.PATROL;
            }
        }

        /// <summary>
        /// update action called when the enemy is in the alert state 
        /// </summary>
        protected virtual void onAlertStateUpdate()
        {
            m_fCurrAlertTimeCounter = isPlayerDetected() ? m_fMaxAlertTime : (m_fCurrAlertTimeCounter - Time.deltaTime);
            
            if (m_fCurrAlertTimeCounter <= 0.0f)
            {
                NavState = NON_STATIC_ENEMY_STATE.IDLE;
            }
            else
            {
                setDestination(PlayerManager.GetPosition());
                m_Animator.ResetTrigger(ANIM_TRIGGER_IDLE);

                if (Vector3.Distance(PlayerManager.GetPosition(), transform.position) <= m_fAttackRadius)
                {    
                    m_Animator.ResetTrigger(ANIM_TRIGGER_WALK);
                    m_Animator.SetTrigger(ANIM_TRIGGER_ATTACK);
                }
                else
                {
                    m_Animator.ResetTrigger(ANIM_TRIGGER_ATTACK);
                    m_Animator.SetTrigger(ANIM_TRIGGER_WALK);
                }
            }
        }

        /// <summary>
        /// update action called when the enemy is in the patrol state 
        /// </summary>
        protected virtual void onPatrolStateUpdate()
        {
            if (isPlayerDetected())
            {
                NavState = NON_STATIC_ENEMY_STATE.ALERT;
            }
        }

        private void OnTriggerEnter(Collider a_Collider)
        {
            EnemyPatrolPoint l_EnemyPatrolPoint = a_Collider.GetComponent<EnemyPatrolPoint>();
            if (l_EnemyPatrolPoint != null 
                && l_EnemyPatrolPoint == m_NextPatrolDestination 
                && NavState == NON_STATIC_ENEMY_STATE.PATROL)
            {
                NavState = NON_STATIC_ENEMY_STATE.IDLE;
            }
        }

        /// <summary>
        /// can the player be detected
        /// is the player within range and raycast to player is true
        /// </summary>
        /// <returns></returns>
        private bool isPlayerDetected()
        {
            bool l_bIsDetected = false;

            RaycastHit l_RaycastHit;
            m_RayDetector.origin = transform.position;
            Vector3 l_v3PlayerPos =  PlayerManager.GetPosition();
            m_RayDetector.direction = (l_v3PlayerPos - transform.position).normalized;

            if (Physics.Raycast(m_RayDetector, out l_RaycastHit, m_fAlertRadius))
            {
                if (l_RaycastHit.collider.tag.Equals(GameConsts.TAG_PLAYER, System.StringComparison.OrdinalIgnoreCase) &&
                    Vector3.Dot(transform.forward, m_RayDetector.direction) >= 0.5f)
                {
                    l_bIsDetected = true;
                }
            }

            return l_bIsDetected;
        }

        /// <summary>
        /// called on enemy starts to attack
        /// </summary>
        protected virtual void onAttack()
        {

        }
    }
}