using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ChainsawWeaponBase : MeleeWeaponBase
    {
        /// <summary>
        /// The damage inflicted on the enemy per second
        /// </summary>
        [SerializeField]
        private int m_iDamagePerInterval = 10;
        public override int getWeaponDamagePerInstance()
        {
            return m_iDamagePerInterval;
        }

        /// <summary>
        /// Damage after Interval
        /// </summary>
        [SerializeField]
        private float m_fTimeBetweenDamageMin = 0.35f;

        /// <summary>
        /// Damage after Interval
        /// </summary>
        [SerializeField]
        private float m_fTimeBetweenDamageMax = 0.6f;

        /// <summary>
        /// Damage after Interval
        /// </summary>
        [SerializeField]
        private float m_fTimeBetweenDamageCurrent = 0.5f;

        [SerializeField]
        private Transform m_transformBlade = null;

        private float m_fTimePassedSinceLastDamageInfliction = 0.0f;

        /// <summary>
        /// List of enemies currently in trigger
        /// </summary>
        private Dictionary<int, Collider> m_dictTriggeredColliders = new Dictionary<int, Collider>(10);

        private Stack<int> m_stackRemoveColliders = new Stack<int>(5);

        /// <summary>
        /// the animator that controls the chainsaw
        /// </summary>
        [SerializeField]
        private Animator m_Animator = null;

        /// <summary>
        /// Audio id to be played on idle
        /// </summary>
        [SerializeField]
        private string m_strSawNonRotateIdleAudID = string.Empty;

        /// <summary>
        /// Audio id to be played on rotating but not cutting anybody
        /// </summary>
        [SerializeField]
        private string m_strSawRotateIdleAudID = string.Empty;

        /// <summary>
        /// Audio id to be played on saw start
        /// </summary>
        [SerializeField]
        private string m_strSawStartAudID = string.Empty;

        /// <summary>
        /// Audio id to be played on saw stop
        /// </summary>
        [SerializeField]
        private string m_strSawStopAudID = string.Empty;

        /// <summary>
        /// Audio id to be played on loop while saw is cutting
        /// </summary>
        [SerializeField]
        private string m_strSawCutLoopAudID = string.Empty;

        [SerializeField]
        private UnpooledAudioSource m_UnpooledAudSrc = null;

        private float m_fTimeInCurrentState = 0.0f;

        [SerializeField]
        private float m_fTimeInStartState = 1.0f;

        [SerializeField]
        private float m_fTimeInStopState = 1.0f;

        private Action m_ActionStateUpdate = null;

        private enum CHAINSAW_STATE
        { 
            NONE            =   0,   
            STATIC          =   1,
            ROTATE_START    =   2,
            ROTATE_STOP     =   3,
            ROTATE_IDLE     =   4,
            ROTATE_CUTTING  =   5,
        }

        /// <summary>
        /// On new weapon is selected
        /// </summary>
        public override void onWeaponSelected()
        {
            base.onWeaponSelected();
            CurrentChainsawState = CHAINSAW_STATE.NONE;
            CurrentChainsawState = CHAINSAW_STATE.STATIC;
            if (m_Animator != null)
            {
                m_Animator.SetBool(ANIM_STATE_SHOOT, false);
                m_Animator.SetTrigger(ANIM_STATE_IDLE_HANDS);
            }
        }

        /// <summary>
        /// The current state of the saw
        /// </summary>
        [SerializeField]
        private CHAINSAW_STATE m_CurrentChainsawState = CHAINSAW_STATE.NONE;
        private CHAINSAW_STATE CurrentChainsawState
        {
            get { return m_CurrentChainsawState; }
            set { 
                if(m_CurrentChainsawState != value)
                {
                    m_CurrentChainsawState = value;
                    setState(m_CurrentChainsawState);
                }
            }
                    
        }

        /// <summary>
        /// On the chainsaw blade's collider is triggerd by an enemy
        /// </summary>
        /// <param name="a_Collider"></param>
        protected override void OnTriggerEnter(Collider a_Collider)
        {
            base.OnTriggerEnter(a_Collider);
            if (GeneralUtils.IsLayerInLayerMask(WeaponManager.GunHitInteractionLayer, a_Collider.gameObject.layer))
            {
                int l_iColliderInstanceID = a_Collider.gameObject.GetInstanceID();
                int l_iEnemyHitColliderID = (LayerMask.NameToLayer(GameConsts.LAYER_NAME_ENEMY_HIT_COLLIDER));
                int l_iSmashableHitColliderID = (LayerMask.NameToLayer(GameConsts.LAYER_NAME_SMASHABLE));

                if ((l_iEnemyHitColliderID == a_Collider.gameObject.layer) ||
                   (l_iSmashableHitColliderID == a_Collider.gameObject.layer))
                {
                    EnemyHitCollider l_EnemyHitCollider = a_Collider.GetComponent<EnemyHitCollider>();
                    SmashableHitCollider l_SmashableHitCollider = null;
                    if (l_EnemyHitCollider == null)
                    {
                        l_SmashableHitCollider = a_Collider.GetComponent<SmashableHitCollider>();
                    }

                    if ((l_EnemyHitCollider != null) || (l_SmashableHitCollider != null))
                    { 
                        if (!m_dictTriggeredColliders.ContainsKey(l_iColliderInstanceID))
                        {
                            m_dictTriggeredColliders.Add(l_iColliderInstanceID, a_Collider);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// On trigger exit from the enemy or default
        /// </summary>
        /// <param name="a_Collider"></param>
        protected override void OnTriggerExit(Collider a_Collider)
        {
            base.OnTriggerExit(a_Collider);
            int l_iInstanceID = a_Collider.gameObject.GetInstanceID();
            if (m_dictTriggeredColliders.ContainsKey(l_iInstanceID))
            {
                m_dictTriggeredColliders.Remove(l_iInstanceID);
            }
        }

        /// <summary>
        /// Inflicts damage every second
        /// </summary>
        private void Update()
        {
            m_fTimeInCurrentState += Time.deltaTime;

            foreach (KeyValuePair<int, Collider> l_dictItem in m_dictTriggeredColliders)
            {
                if (!l_dictItem.Value.enabled || !l_dictItem.Value.gameObject.activeInHierarchy)
                {
                    m_stackRemoveColliders.Push(l_dictItem.Key);
                }
            }

            int l_iID = 0;
            while (m_stackRemoveColliders.Count > 0)
            {
                l_iID = m_stackRemoveColliders.Pop();
                m_dictTriggeredColliders.Remove(l_iID);
            }

            if (m_ActionStateUpdate != null)
            {
                m_ActionStateUpdate();
            }
        }

        /// <summary>
        /// clears the dictionary that includes all the enemies
        /// </summary>
        private void clearRegisteredColliders()
        {
            m_stackRemoveColliders.Clear();
            m_dictTriggeredColliders.Clear();
        }

        private void OnEnable()
        {
            clearRegisteredColliders();
        }

        private void OnDisable()
        {
            clearRegisteredColliders();
        }

        public override void startShootingAnim()
        {
            if (CurrentChainsawState == CHAINSAW_STATE.STATIC ||
                CurrentChainsawState == CHAINSAW_STATE.ROTATE_STOP)
            {
                CurrentChainsawState = CHAINSAW_STATE.ROTATE_START;
                m_Animator.SetBool(ANIM_STATE_SHOOT, true);
            }
        }

        public override void stopShootingAnim()
        {
            if (CurrentChainsawState != CHAINSAW_STATE.STATIC)
            {
                CurrentChainsawState = CHAINSAW_STATE.ROTATE_STOP;
                m_Animator.SetBool(ANIM_STATE_SHOOT, false);
            }
        }

        /// <summary>
        /// The game is paused/unpaused
        /// play animation of the arm monitor
        /// </summary>
        /// <param name="a_IsPaused"></param>
        public override void onGamePauseToggled(bool a_IsPaused)
        {
            base.onGamePauseToggled(a_IsPaused);

            if (m_Animator != null)
            {
                m_Animator.SetTrigger(a_IsPaused ? ANIM_STATE_OPEN_MENU : ANIM_STATE_CLOSE_MENU);
            }

            m_UnpooledAudSrc.stop();
        }

        private void setState(CHAINSAW_STATE a_CurrentChainsawState)
        {
            m_fTimeInCurrentState = 0.0f;
            switch (a_CurrentChainsawState)
            {
                case CHAINSAW_STATE.NONE:
                    {
                        m_ActionStateUpdate = null;
                        break;
                    }
                case CHAINSAW_STATE.STATIC:
                    {
                        m_ActionStateUpdate = null;
                        m_UnpooledAudSrc.play(m_strSawNonRotateIdleAudID, true, 1.0f);
                        break;
                    }
                case CHAINSAW_STATE.ROTATE_IDLE:
                    {
                        m_ActionStateUpdate = rotateIdleStateUpdate;
                        m_UnpooledAudSrc.play(m_strSawRotateIdleAudID, true, 1.0f);
                        break;
                    }
                case CHAINSAW_STATE.ROTATE_START:
                    {
                        m_ActionStateUpdate = rotateStartStateUpdate;
                        m_UnpooledAudSrc.play(m_strSawStartAudID, true, 1.0f);
                        break;
                    }
                case CHAINSAW_STATE.ROTATE_STOP:
                    {
                        m_ActionStateUpdate = rotateStopStateUpdate;
                        m_UnpooledAudSrc.play(m_strSawStopAudID, true, 1.0f);
                        break;
                    }
                case CHAINSAW_STATE.ROTATE_CUTTING:
                    {
                        m_ActionStateUpdate = rotateCuttingStateUpdate;
                        m_UnpooledAudSrc.play(m_strSawCutLoopAudID, true, 1.0f);
                        break;
                    }
            }
        }


        private void rotateIdleStateUpdate()
        {
            if (m_dictTriggeredColliders.Count > 0)
            {
                CurrentChainsawState = CHAINSAW_STATE.ROTATE_CUTTING;
            }
        }

        private void rotateStartStateUpdate()
        {
            if (m_fTimeInCurrentState > m_fTimeInStartState)
            {
                CurrentChainsawState = CHAINSAW_STATE.ROTATE_IDLE;
            }
        }

        private void rotateStopStateUpdate()
        {
            if (m_fTimeInCurrentState > m_fTimeInStopState)
            {
                CurrentChainsawState = CHAINSAW_STATE.STATIC;
            }
        }

        private void rotateCuttingStateUpdate()
        {
            if (m_dictTriggeredColliders.Count == 0)
            {
                CurrentChainsawState = CHAINSAW_STATE.ROTATE_IDLE;
            }
            else
            {
                if (m_fTimePassedSinceLastDamageInfliction == 0.0f)
                {
                    m_fTimeBetweenDamageCurrent = UnityEngine.Random.Range(m_fTimeBetweenDamageMin, m_fTimeBetweenDamageMax);
                }

                m_fTimePassedSinceLastDamageInfliction += Time.deltaTime;
                if (m_fTimePassedSinceLastDamageInfliction > m_fTimeBetweenDamageCurrent)
                {
                    m_fTimePassedSinceLastDamageInfliction = 0.0f;
                    foreach (KeyValuePair<int, Collider> l_dictItem in m_dictTriggeredColliders)
                    {
                        WeaponManager.OnWeaponHitItem(l_dictItem.Value, this, m_transformBlade.position, m_transformBlade.position, true);
                    }
                }
            }
        }
    }
}