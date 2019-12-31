using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class AutomatedTurret : StaticEnemy
    {
        [SerializeField]
        private Transform m_transGunMiddle = null;

        [SerializeField]
        private Transform m_transGunTop = null;

        private float l_fRotationTime = 0.0f;
        private float l_fRotationTimeMultiplier = 1.0f;

        /// <summary>
        /// The time after a shot is fired that the next shot is fired
        /// </summary>
        [SerializeField]
        private float m_fTimeBetweenShots = 0.3f;

        /// <summary>
        /// The time passed since last shot
        /// </summary>
        private float m_fTimeSinceLastShot = 0.0f;

        /// <summary>
        /// The turret rotation speed
        /// </summary>
        private const float ROTATION_SPEED = 0.25f;

        /// <summary>
        /// The left max the turret will rotate in Y axis
        /// </summary>
        [SerializeField]
        private float m_fLeftMaxZ = 0.0f;

        /// <summary>
        /// The right max the turret will rotate in Y axis
        /// </summary>
        [SerializeField]
        private float m_fRightMaxZ = 0.0f;

        /// <summary>
        /// The damage inflicted on the player on shoot
        /// </summary>
        [SerializeField]
        private int m_iDamageOnHit = 10;

        /// <summary>
        /// The muzzle flash 1 of the turret
        /// </summary>
        [SerializeField]
        private ParticleSystem m_particleMuzzleFlash1 = null;

        /// <summary>
        /// The muzzle flash 2 of the turret
        /// </summary>
        [SerializeField]
        private ParticleSystem m_particleMuzzleFlash2 = null;

        /// <summary>
        /// The unpooled audio source of the turret
        /// </summary>
        [SerializeField]
        private UnpooledAudioSource m_AudioSoure = null;

        /// <summary>
        /// The material on the turret when deactivated
        /// </summary>
        [SerializeField]
        private Material m_matTurretDeactivated = null;

        /// <summary>
        /// The material on the turret when activated
        /// </summary>
        [SerializeField]
        private Material m_matTurretActivated = null;

        /// <summary>
        /// List of mesh renderers to set the mats to activate/deactivates
        /// </summary>
        [SerializeField]
        private List<MeshRenderer> m_lstMeshRenderers = null;

        /// <summary>
        /// The rotation time of the turret top on getting disabled
        /// </summary>
        private float m_fRotTime = 0.0f;

        /// <summary>
        /// Activates the enemy on start
        /// </summary>
        public override void activateEnemy()
        {
            base.activateEnemy();
            NavState = ENEMY_STATE.NONE;
            toggleTurretActivate(false);
        }

        /// <summary>
        /// On turret trigger activated 
        /// </summary>
        public void onTurretTriggeredToActivate()
        {
            NavState = ENEMY_STATE.PATROL;
            toggleTurretActivate(true);
        }

        /// <summary>
        /// callback called on state changed
        /// </summary>
        /// <param name="l_OldNavState"></param>
        /// <param name="a_NavState"></param>
        protected override void onStateChanged(ENEMY_STATE l_OldNavState, ENEMY_STATE a_NavState)
        {
            base.onStateChanged(l_OldNavState, a_NavState);

            switch (a_NavState)
            {
                case ENEMY_STATE.PATROL:
                    m_actNavStateUpdate = onPatrolStateUpdate;
                    break;

                case ENEMY_STATE.ALERT:
                    m_actNavStateUpdate = onAlertStateUpdate;
                    break;

                case ENEMY_STATE.IDLE:
                    m_fRotTime = 0.0f;
                    m_actNavStateUpdate = onIdleStateUpdate;
                    break;

                case ENEMY_STATE.NONE:
                    m_actNavStateUpdate = null;
                    break;

                case ENEMY_STATE.DEAD:
                    m_actNavStateUpdate = null;
                    break;
            }
        }

        /// <summary>
        /// Update function on alert update
        /// </summary>
        protected virtual void onAlertStateUpdate()
        {
            RaycastHit l_RaycastHit;
            m_RayDetector.origin = transform.position;
            Vector3 l_v3PlayerPos = PlayerManager.GetPosition();
            Vector3 l_v3TurretToPlayerDir = (l_v3PlayerPos - transform.position).normalized;
            m_RayDetector.direction = l_v3TurretToPlayerDir;
            if (Physics.Raycast(m_RayDetector, out l_RaycastHit, m_fAttackRadius, m_AttackLayerMask) &&
                l_RaycastHit.collider != null &&
                l_RaycastHit.collider.tag.Equals(GameConsts.TAG_PLAYER, System.StringComparison.OrdinalIgnoreCase))
            {
                PlayerController l_PlayerController = l_RaycastHit.collider.GetComponent<PlayerController>();

                Quaternion l_quatCurrentLook = m_transGunMiddle.rotation;
                Quaternion l_quatEndLook = Quaternion.LookRotation(l_v3TurretToPlayerDir); 

                Quaternion l_quatTurretRot = Quaternion.Slerp(l_quatCurrentLook, l_quatEndLook, 1.0f/*((360.0f - Quaternion.Angle(l_quatCurrentLook, l_quatEndLook)) / 360.0f) + Time.deltaTime*/);

                Quaternion l_quatMidRot = Quaternion.Euler(0.0f, l_quatTurretRot.eulerAngles.y, 0.0f);
                m_transGunMiddle.rotation = l_quatMidRot;

                Quaternion l_quatTopRot = Quaternion.Euler(l_quatTurretRot.eulerAngles.x, l_quatTurretRot.eulerAngles.y, 0.0f);
                m_transGunTop.rotation = l_quatTopRot;

                m_fTimeSinceLastShot += Time.deltaTime;
                if (m_fTimeSinceLastShot > m_fTimeBetweenShots)
                {
                    m_fTimeSinceLastShot = 0.0f;
                    fireAtPlayer();

                    m_AudioSoure.play(GameConsts.AUD_CLIP_TURRET_FIRE, false, 1.0f);

                    if (m_particleMuzzleFlash1 != null)
                    {
                        if (!m_particleMuzzleFlash1.isPlaying)
                        {
                            m_particleMuzzleFlash1.Play();
                        }
                    }

                    if (m_particleMuzzleFlash2 != null)
                    {
                        if (!m_particleMuzzleFlash2.isPlaying)
                        {
                            m_particleMuzzleFlash2.Play();
                        }
                    }
                }
            }
            else
            {
                NavState = ENEMY_STATE.PATROL;

                if (m_particleMuzzleFlash1 != null)
                {
                    m_particleMuzzleFlash1.Stop();
                }

                if (m_particleMuzzleFlash2 != null)
                {
                    m_particleMuzzleFlash2.Stop();
                }
            }
        }

        /// <summary>
        /// Update function on patrol update
        /// </summary>
        protected virtual void onPatrolStateUpdate()
        {
            if (l_fRotationTime > 1.0f)
            {
                l_fRotationTimeMultiplier = -1.0f;
            }
            else if (l_fRotationTime < 0.0f)
            {
                l_fRotationTimeMultiplier = 1.0f;
            }

            l_fRotationTime += l_fRotationTimeMultiplier * Time.deltaTime * ROTATION_SPEED;
            m_transGunMiddle.rotation = Quaternion.Euler(m_transGunMiddle.rotation.y, Mathf.LerpAngle(m_fLeftMaxZ, m_fRightMaxZ, l_fRotationTime), m_transGunMiddle.rotation.z);

            RaycastHit l_RaycastHit;
            m_RayDetector.origin = transform.position;
            m_RayDetector.direction = (PlayerManager.GetPosition() - transform.position).normalized;

            if (Physics.Raycast(m_RayDetector, out l_RaycastHit, m_fAttackRadius, m_AttackLayerMask) &&
                l_RaycastHit.collider != null &&
                l_RaycastHit.collider.tag.Equals(GameConsts.TAG_PLAYER, System.StringComparison.OrdinalIgnoreCase))
            {
                NavState = ENEMY_STATE.ALERT;
            }
        }

        /// <summary>
        /// Update function on idle update
        /// </summary>
        protected virtual void onIdleStateUpdate()
        {
            if (Vector3.Dot(m_transGunTop.forward, Vector3.down) < 0.7f)
            {
                Quaternion l_quatCurrentLook = m_transGunTop.rotation;
                Quaternion l_quatEndLook = Quaternion.LookRotation(Vector3.down);

                m_fRotTime += Time.deltaTime * 0.1f;
                Quaternion l_quatCurrentTopRot = Quaternion.Slerp(l_quatCurrentLook, l_quatEndLook, m_fRotTime);

                m_transGunTop.rotation = Quaternion.Euler(l_quatCurrentTopRot.eulerAngles.x, m_transGunTop.eulerAngles.y, m_transGunTop.eulerAngles.z);
            }
            else
            {
                m_fRotTime = 0.0f;
                NavState = ENEMY_STATE.DEAD;
            }
        }

        /// <summary>
        /// Sets the left max and right max will rotate
        /// </summary>
        /// <param name="a_fLeftMax"></param>
        /// <param name="a_fRightMax"></param>
        public void setLeftRightMaxYAngle(float a_fLeftMax, float a_fRightMax)
        {
            m_fLeftMaxZ = a_fLeftMax;
            m_fRightMaxZ = a_fRightMax;
        }

        /// <summary>
        /// Called when the gun fires at the enemy
        /// </summary>
        private void fireAtPlayer()
        {
            PlayerManager.InflictDamage(m_iDamageOnHit);
        }

        /// <summary>
        /// The turret is switched off
        /// </summary>
        public void onSwitchedOff()
        {
            NavState = ENEMY_STATE.IDLE;
            toggleTurretActivate(false);
        }

        /// <summary>
        /// Toggle turret materials to activate/deactivate
        /// </summary>
        /// <param name="a_bIsTurretActivate"></param>
        private void toggleTurretActivate(bool a_bIsTurretActivate)
        {
            Material l_matToSet = a_bIsTurretActivate ? m_matTurretActivated : m_matTurretDeactivated;

            Material[] l_arrMaterials = new Material[] { l_matToSet };
            int l_iMeshRendererCount = m_lstMeshRenderers.Count;
            for (int l_iMeshRendererIndex = 0; l_iMeshRendererIndex < l_iMeshRendererCount; l_iMeshRendererIndex++)
            {
                m_lstMeshRenderers[l_iMeshRendererIndex].materials = l_arrMaterials;
            }
        }
    }
}