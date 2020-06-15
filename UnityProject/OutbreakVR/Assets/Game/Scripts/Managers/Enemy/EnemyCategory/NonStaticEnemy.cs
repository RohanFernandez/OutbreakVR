using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class NonStaticEnemy : EnemyBase
    {

        protected const string ANIM_TRIGGER_WALK        = "walk";       //"Walk";
        protected const string ANIM_TRIGGER_ATTACK      = "Attack_1";   //"Attack";
        protected const string ANIM_TRIGGER_DIE         = "Die";
        protected const string ANIM_TRIGGER_IDLE        = "Idle";

        ///Hit animation
        protected const string ANIM_TRIGGER_HIT_HEAD            = "Hit_head";
        protected const string ANIM_TRIGGER_HIT_BODY            = "Hit_body";
        protected const string ANIM_TRIGGER_HIT_RIGHT_SHOULDER  = "Hit_shoulder_R";
        protected const string ANIM_TRIGGER_HIT_LEFT_SHOULDER   = "Hit_shoulder_L";

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

        /// <summary>
        /// Time that will be spent in the idle state and then transitioning to patrol
        /// </summary>
        [SerializeField]
        protected float m_fMaxIdleTime = 5.0f;

        /// <summary>
        /// Current time passed that is spent in the idle state
        /// </summary>
        protected float m_fCurrIdleTimeCounter = 0.0f;

        /// <summary>
        /// max Time in alert modeafter which alert of that enemy will go off
        /// </summary>
        [SerializeField]
        protected float m_fMaxAlertTime = 0.0f;

        /// <summary>
        /// Time in alert mode, will start since last scene the player
        /// </summary>
        protected float m_fCurrAlertTimeCounter = 0.0f;

        /// <summary>
        /// Stopping distance when patrolling towards a patrol point
        /// </summary>
        [SerializeField]
        protected float m_fPatrolStoppingDistance = 2.0f;

        /// <summary>
        /// The speed of the nav mesh agent while patrolling
        /// </summary>
        [SerializeField]
        protected float m_fPatrollingNavigationSpeed = 0.75f;

        /// <summary>
        /// Stopping distance when attacking an enemy
        /// </summary>
        [SerializeField]
        protected float m_fAlertStoppingDistance = 1.0f;

        /// <summary>
        /// The speed of the nav mesh agent while in alert
        /// </summary>
        [SerializeField]
        protected float m_fAlertNavigationSpeed = 1.0f;

        /// <summary>
        /// The next patrol point the enemy is traversing to
        /// </summary>
        private EnemyPatrolPoint m_NextPatrolDestination = null;

        /// <summary>
        /// The last patrol point the enemy was at
        /// </summary>
        private EnemyPatrolPoint m_LastPatrolDestination = null;

        /// <summary>
        /// Radius under which will go into alert mode attempting to reach the player to attack.
        /// </summary>
        [SerializeField]
        protected float m_fAlertRadius = 15.0f;

        /// <summary>
        /// Radius under which will go into alert mode attempting to reach the player to attack.
        /// This radius is when a gunshot is firedby the player
        /// </summary>
        [SerializeField]
        protected float m_fExtendedAlertRadius = 23.0f;

        /// <summary>
        /// List of all the enemy hit colliders
        /// </summary>
        [SerializeField]
        private List<EnemyHitCollider> m_lstEnemyHitColliders = null;

        /// <summary>
        /// List of all the enemy rigidbody colliders
        /// </summary>
        [SerializeField]
        private List<Rigidbody> m_lstRagdollRigidbodies = null;

        /// <summary>
        /// resets all values of the hit colliders
        /// </summary>
        private void resetHitColliders()
        {
            int l_iHitColliderCount = m_lstEnemyHitColliders.Count;
            for (int l_iHitColliderIndex = 0; l_iHitColliderIndex < l_iHitColliderCount; l_iHitColliderIndex++)
            {
                m_lstEnemyHitColliders[l_iHitColliderIndex].reset();
            }
        }

        /// <summary>
        /// The enemy hit collision on damage inflicted
        /// </summary>
        private ENEMY_HIT_COLLISION m_EnemyHitCollision;

        #region TEMP_ADDED
        private float TEMP_SUFFER_STATE_TIME = 1.3f;
        private float m_fTimePassedInSufferState = 0.0f;
        #endregion TEMP_ADDED

        /// <summary>
        /// called on damage inflicted on the enemy but still alive
        /// </summary>
        /// <param name="a_iDamage"></param>
        protected override void onDamageInflictedNotKilled(int a_iDamage, ENEMY_HIT_COLLISION a_EnemyHitCollision = ENEMY_HIT_COLLISION.HIT_COLLISION_DEFAULT)
        {
            base.onDamageInflictedNotKilled(a_iDamage);
            m_EnemyHitCollision = a_EnemyHitCollision;

            if (NavState != ENEMY_STATE.DEAD)
            {
                NavState = ENEMY_STATE.DAMAGE_INFLICTED;
            }
        }

        public override void activateEnemy()
        {
            base.activateEnemy();

            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_WEAPON_FIRED, onPlayerWeaponFired);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_ENEMY_ALERT_STARTED, onAnotherEnemyAlertStarted);

            if (m_NavMeshPath == null) { m_NavMeshPath = new UnityEngine.AI.NavMeshPath(); }

            toggleRagdoll(false);
            resetHitColliders();

            m_LastPatrolDestination = null;
            m_NavMeshAgent.Warp(transform.position);
            NavState = ENEMY_STATE.IDLE;
        }

        public override void deactivateEnemy()
        {
            base.deactivateEnemy();
            toggleRagdoll(false);

            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_WEAPON_FIRED, onPlayerWeaponFired);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_ENEMY_ALERT_STARTED, onAnotherEnemyAlertStarted);

            NavState = ENEMY_STATE.NONE;
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
        /// called on killed fire event 
        /// </summary>
        protected override void onKilled()
        {
            base.onKilled();
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
            else
            {
                m_LastPatrolDestination = null;
            }

            m_NextPatrolDestination = PatrolManager.GetNextPatrolPoint(this, l_CurrentPatrolPoint, m_LastPatrolDestination);
            m_LastPatrolDestination = l_CurrentPatrolPoint;

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
        protected override void onStateChanged(ENEMY_STATE l_OldNavState, ENEMY_STATE a_NavState)
        {
            base.onStateChanged(l_OldNavState, a_NavState);
            switch (a_NavState)
            {
                case ENEMY_STATE.IDLE:
                    {
                        stopNavigation();
                        m_fCurrAlertTimeCounter = 0.0f;
                        m_fCurrIdleTimeCounter = 0.0f;
                        m_actNavStateUpdate = onIdleStateUpdate;
                        break;
                    }
                case ENEMY_STATE.PATROL:
                    {
                        m_NavMeshAgent.speed = m_fPatrollingNavigationSpeed;
                        m_NavMeshAgent.stoppingDistance = m_fPatrolStoppingDistance;
                        startNavigation();
                        m_Animator.SetTrigger(ANIM_TRIGGER_WALK);
                        patrolToPoint(m_NextPatrolDestination != null);
                        m_actNavStateUpdate = onPatrolStateUpdate;
                        break;
                    }
                case ENEMY_STATE.ALERT:
                    {
                        startNavigation();
                        m_NavMeshAgent.speed = m_fAlertNavigationSpeed;
                        m_NavMeshAgent.stoppingDistance = m_fAlertStoppingDistance;
                        m_fCurrAlertTimeCounter = m_fMaxAlertTime;
                        m_NextPatrolDestination = null;
                        m_actNavStateUpdate = onAlertStateUpdate;
                        break;
                    }
                case ENEMY_STATE.DEAD:
                    {
                        stopNavigation();
                        toggleRagdoll(true);
                        m_NextPatrolDestination = null;
                        m_actNavStateUpdate = null;
                        break;
                    }
                case ENEMY_STATE.DAMAGE_INFLICTED:
                    {
                        stopNavigation();
                        setDestination(transform.position);
                        m_actNavStateUpdate = null;
                        NavState = ENEMY_STATE.SUFFER;
                        break;
                    }
                case ENEMY_STATE.SUFFER:
                    {
                        stopNavigation();
                        setDestination(transform.position);
                        m_fTimePassedInSufferState = 0.0f;

                        string l_strAnimTrigger = ANIM_TRIGGER_HIT_BODY;
                        switch (m_EnemyHitCollision)
                        {
                            case ENEMY_HIT_COLLISION.HIT_COLLISION_HEAD:
                                {
                                    l_strAnimTrigger = ANIM_TRIGGER_HIT_HEAD;
                                    break;
                                }
                            case ENEMY_HIT_COLLISION.HIT_COLLISION_LEFT_SHOULDER:
                                {
                                    l_strAnimTrigger = ANIM_TRIGGER_HIT_LEFT_SHOULDER;
                                    break;
                                }
                            case ENEMY_HIT_COLLISION.HIT_COLLISION_RIGHT_SHOULDER:
                                {
                                    l_strAnimTrigger = ANIM_TRIGGER_HIT_RIGHT_SHOULDER;
                                    break;
                                }
                            case ENEMY_HIT_COLLISION.HIT_COLLISION_LEFT_ARM:
                                {
                                    l_strAnimTrigger = ANIM_TRIGGER_HIT_LEFT_SHOULDER;
                                    break;
                                }
                            case ENEMY_HIT_COLLISION.HIT_COLLISION_RIGHT_ARM:
                                {
                                    l_strAnimTrigger = ANIM_TRIGGER_HIT_RIGHT_SHOULDER;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }

                        m_Animator.SetTrigger(l_strAnimTrigger);
                        m_actNavStateUpdate = onSuffferStateUpdate;
                        break;
                    }
                case ENEMY_STATE.NONE:
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
                NavState = ENEMY_STATE.ALERT;
            }
            else if (m_fCurrIdleTimeCounter > m_fMaxIdleTime)
            {
                NavState = ENEMY_STATE.PATROL;
            }
        }

        /// <summary>
        /// update action called when the enemy is in the alert state 
        /// </summary>
        protected virtual void onAlertStateUpdate()
        {
        }

        /// <summary>
        /// update action called when the enemy is in the suffer state 
        /// </summary>
        protected virtual void onSuffferStateUpdate()
        {
            m_fTimePassedInSufferState += Time.deltaTime;
            if (m_fTimePassedInSufferState > TEMP_SUFFER_STATE_TIME)
            {
                NavState = ENEMY_STATE.ALERT;
            }
        }

        /// <summary>
        /// update action called when the enemy is in the patrol state 
        /// </summary>
        protected virtual void onPatrolStateUpdate()
        {
            if (isPlayerDetected())
            {
                NavState = ENEMY_STATE.ALERT;
            }
            else
            {
                if ((m_NavMeshAgent.remainingDistance != Mathf.Infinity) &&
                    (m_NavMeshAgent.remainingDistance <= (m_NavMeshAgent.stoppingDistance + 0.5f)))
                {
                    NavState = ENEMY_STATE.IDLE;
                }
            }
        }

        /// <summary>
        /// can the player be detected
        /// is the player within range and raycast to player is true
        /// </summary>
        /// <returns></returns>
        protected bool isPlayerDetected()
        {
            bool l_bIsDetected = false;

            RaycastHit l_RaycastHit;
            m_RayDetector.origin = transform.position;
            Vector3 l_v3PlayerPos =  PlayerManager.GetPosition();
            Vector3 l_v3EnemyToPlayerDir = (l_v3PlayerPos - transform.position).normalized;
            m_RayDetector.direction = l_v3EnemyToPlayerDir;

            if (Physics.Raycast(m_RayDetector, out l_RaycastHit, m_fAlertRadius, m_AttackLayerMask) &&
                Vector3.Dot(transform.forward, m_RayDetector.direction) >= 0.25f &&
                l_RaycastHit.collider.gameObject.layer == LayerMask.NameToLayer(GameConsts.LAYER_NAME_PLAYER))
            {
                l_bIsDetected = true;
            }

            return l_bIsDetected;
        }

        /// <summary>
        /// called on enemy starts to attack
        /// </summary>
        protected virtual void onAttack()
        {

        }

        /// <summary>
        /// Callback called on player's weapon fired
        /// </summary>
        protected virtual void onPlayerWeaponFired(EventHash a_EventHash)
        {
            alertEnemyOnPlayerProximity();
        }

        /// <summary>
        /// Callback called when another enemy has been alerted, alert this enemy as well
        /// used to force the enemy to the alert state
        /// </summary>
        protected virtual void onAnotherEnemyAlertStarted(EventHash a_EventHash)
        {
            bool l_bIsEnemyAlertForced = (bool)a_EventHash[GameEventTypeConst.ID_FORCED_ENEMY_ALERT];

            if (!l_bIsEnemyAlertForced)
            {
                EnemyBase l_EnemyBase = (EnemyBase)a_EventHash[GameEventTypeConst.ID_ENEMY_BASE];
                if (l_EnemyBase == this)
                {
                    return;
                }
            }

            alertEnemyOnPlayerProximity();
        }


        /// <summary>
        /// Alerts the enemy if the player is within a certain distance from the player
        /// called to force detect on players gunshots
        /// </summary>
        private void alertEnemyOnPlayerProximity()
        {
            Vector3 l_v3PlayerPos = PlayerManager.GetPosition();

            if ((NavState != ENEMY_STATE.ALERT) &&
                (NavState != ENEMY_STATE.SUFFER) &&
                (NavState != ENEMY_STATE.DAMAGE_INFLICTED) &&
                (NavState != ENEMY_STATE.DEAD))
            {
                if (m_NavMeshAgent.isActiveAndEnabled)
                {
                    m_NavMeshAgent.CalculatePath(l_v3PlayerPos, m_NavMeshPath);
                    if (m_NavMeshPath.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                    {
                        float l_fDistanceToPlayer = PatrolManager.GetNavDistanceToTarget(m_NavMeshPath);
                        if (l_fDistanceToPlayer < m_fExtendedAlertRadius)
                        {
                            NavState = ENEMY_STATE.ALERT;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Toggles the ragdoll effect
        /// </summary>
        /// <param name="a_bIsEnabled"></param>
        private void toggleRagdoll(bool a_bIsEnabled)
        {
            bool l_bEnableComponents = !a_bIsEnabled;
            m_Animator.enabled = l_bEnableComponents;
            int l_iRagdollRigidBodyCount = m_lstRagdollRigidbodies.Count;
            for (int l_iRagdollRigidBodyIndex = 0; l_iRagdollRigidBodyIndex < l_iRagdollRigidBodyCount; l_iRagdollRigidBodyIndex++)
            {
                m_lstRagdollRigidbodies[l_iRagdollRigidBodyIndex].isKinematic = l_bEnableComponents;
            }
        }
    }
}