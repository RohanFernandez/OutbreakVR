using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class StealthTurret : MonoBehaviour, IColliderEventReceiver
    {
        private enum SHOOT_STATE
        { 
            START_DELAY             =   0,
            SHOOT                   =   1,
            PAUSE_BETWEEN_ROUND     =   2
        }

        private SHOOT_STATE m_CurrentShootState;
        private SHOOT_STATE CurrentShootState
        {
            get { return m_CurrentShootState; }
            set {
                m_CurrentShootState = value;
                onChangeState(value);
            }
        }

        [SerializeField]
        private LayerMask m_AttackLayerMask;

        private float m_fTimeInCurrentState = 0.0f;

        [SerializeField]
        private float m_fDelayOnStart = 1.0f;

        [SerializeField]
        private float m_fTimeBetweenEachShot = 0.5f;

        [SerializeField]
        private int m_iShotsInOneRound = 4;

        private int m_iBulletsFiredInCurrentRound = 0;

        [SerializeField]
        private float m_fTimeBetweenEachRound = 5.0f;

        private float m_fTimeSinceLastShot = 0.0f;

        private Ray m_GunRay = new Ray();

        private bool m_bIsPlayerinShootingRange = false;

        [SerializeField]
        private ColliderTriggerEngager m_ColliderTriggerEngager = null;

        [SerializeField]
        private List<GameObject> m_lstDefaultShootPositions = null;

        [SerializeField]
        private UnpooledAudioSource m_AudSrc = null;

        [SerializeField]
        private string m_strAudClipOnFire = string.Empty;

        private int m_iDefaultShootPositionsCount = 0;

        [SerializeField]
        private int m_iDamageOnShot = 10;

        [SerializeField]
        private ParticleSystem m_GunTracerParticleSystem = null;

        void OnEnable()
        {
            CurrentShootState = SHOOT_STATE.START_DELAY;
            m_bIsPlayerinShootingRange = false;
            m_ColliderTriggerEngager.setColliderReceiver(this);
            m_iDefaultShootPositionsCount = m_lstDefaultShootPositions.Count;
        }

        private Action m_actStateUpdate = null;

        void Update()
        {
            m_fTimeInCurrentState += Time.deltaTime;
            if (m_actStateUpdate != null)
            {
                m_actStateUpdate();
            }
        }

        void onChangeState(SHOOT_STATE a_NewState)
        {
            m_fTimeInCurrentState = 0.0f;
            switch(a_NewState)
            {
                case SHOOT_STATE.START_DELAY:
                    {
                        m_actStateUpdate = delayStateUpdate;
                        break;
                    }
                case SHOOT_STATE.SHOOT:
                    {
                        m_fTimeSinceLastShot = 0.0f;
                        m_iBulletsFiredInCurrentRound = 0;
                        m_actStateUpdate = shootStateUpdate;
                        break;
                    }
                case SHOOT_STATE.PAUSE_BETWEEN_ROUND:
                    {
                        m_actStateUpdate = pauseBetweenRoundStateUpdate;
                        break;
                    }
            } 
        }

        private void delayStateUpdate()
        {
            if (m_fTimeInCurrentState > m_fDelayOnStart)
            {
                CurrentShootState = SHOOT_STATE.SHOOT;
            }
        }

        private void shootStateUpdate()
        {
            m_fTimeSinceLastShot += Time.deltaTime;
            if (m_fTimeSinceLastShot > m_fTimeBetweenEachShot)
            {
                m_fTimeSinceLastShot = 0.0f;
                fireWeapon();
                ++m_iBulletsFiredInCurrentRound;
            }

            if (m_iBulletsFiredInCurrentRound >= m_iShotsInOneRound)
            {
                CurrentShootState = SHOOT_STATE.PAUSE_BETWEEN_ROUND;
            }
        }

        private void pauseBetweenRoundStateUpdate()
        {
            if (m_fTimeInCurrentState > m_fTimeBetweenEachRound)
            {
                CurrentShootState = SHOOT_STATE.SHOOT;
            }
        }

        private void fireWeapon()
        {
            m_GunRay.origin = transform.position;
            Vector3 l_v3PlayerPosition =  PlayerManager.GetPosition();
            if (m_bIsPlayerinShootingRange)
            {
                m_GunRay.direction = (l_v3PlayerPosition - transform.position).normalized;
            }
            else
            {
                m_GunRay.direction = (m_lstDefaultShootPositions[UnityEngine.Random.Range(0, m_iDefaultShootPositionsCount)].transform.position - transform.position).normalized;
            }

            m_AudSrc.play(m_strAudClipOnFire, false, 1.0f);

            RaycastHit l_RaycastHit;
            if (Physics.Raycast(m_GunRay, out l_RaycastHit, m_AttackLayerMask))
            {
                if (l_RaycastHit.collider != null)
                {
                    if (l_RaycastHit.collider.gameObject.layer == LayerMask.NameToLayer(GameConsts.LAYER_NAME_PLAYER))
                    {
                        PlayerManager.InflictDamage(m_iDamageOnShot, DAMAGE_INFLICTION_TYPE.GUNFIRE);
                    }

                    EffectsBase l_EffectsBase = EffectsManager.getEffectsBase();
                    l_EffectsBase.transform.SetPositionAndRotation(l_RaycastHit.transform.position, Quaternion.LookRotation(l_RaycastHit.normal));
                    m_GunTracerParticleSystem.transform.LookAt(l_RaycastHit.transform);
                }
            }
        }

        public void onTriggerEnterCallback(Collider a_Collider)
        {
            if (a_Collider.gameObject.layer == LayerMask.NameToLayer(GameConsts.LAYER_NAME_PLAYER))
            {
                m_bIsPlayerinShootingRange = true;
            }
        }

        public void onTriggerExitCallback(Collider a_Collider)
        {
            if (a_Collider.gameObject.layer == LayerMask.NameToLayer(GameConsts.LAYER_NAME_PLAYER))
            {
                m_bIsPlayerinShootingRange = false;
            }
        }
    }
}