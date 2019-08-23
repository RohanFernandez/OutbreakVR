using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class AutomatedTurret : StaticEnemy
    {
        [SerializeField]
        private Transform m_transGunBarrel = null;

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
        private float m_fLeftMaxY = 0.0f;

        /// <summary>
        /// The right max the turret will rotate in Y axis
        /// </summary>
        [SerializeField]
        private float m_fRightMaxY = 0.0f;

        /// <summary>
        /// The damage inflicted on the player on shoot
        /// </summary>
        [SerializeField]
        private int m_iDamageOnHit = 10;

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
                case ENEMY_STATE.IDLE:
                    m_actNavStateUpdate = onIdleStateUpdate;
                    break;

                case ENEMY_STATE.ALERT:
                    m_actNavStateUpdate = onAlertStateUpdate;
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
            Vector3 l_v3TurretToPlayerDir = (PlayerManager.GetPosition() - transform.position).normalized;
            m_RayDetector.direction = l_v3TurretToPlayerDir;
            if (Physics.Raycast(m_RayDetector, out l_RaycastHit, m_fAttackRadius, m_AttackLayerMask) &&
                l_RaycastHit.collider != null &&
                l_RaycastHit.collider.tag.Equals(GameConsts.TAG_PLAYER, System.StringComparison.OrdinalIgnoreCase))
            {
                PlayerController l_PlayerController = l_RaycastHit.collider.GetComponent<PlayerController>();

                m_transGunBarrel.forward = l_v3TurretToPlayerDir;
                m_fTimeSinceLastShot += Time.deltaTime;
                if (m_fTimeSinceLastShot > m_fTimeBetweenShots)
                {
                    m_fTimeSinceLastShot = 0.0f;
                    fireAtPlayer();
                }
            }
            else
            {
                NavState = ENEMY_STATE.IDLE;
            }
        }

        /// <summary>
        /// Update function on idle update
        /// </summary>
        protected virtual void onIdleStateUpdate()
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
            m_transGunBarrel.localRotation = Quaternion.Euler(m_transGunBarrel.rotation.x, Mathf.LerpAngle(m_fLeftMaxY, m_fRightMaxY, l_fRotationTime), m_transGunBarrel.rotation.z);

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
        /// Sets the left max and right max will rotate
        /// </summary>
        /// <param name="a_fLeftMax"></param>
        /// <param name="a_fRightMax"></param>
        public void setLeftRightMaxYAngle(float a_fLeftMax, float a_fRightMax)
        {
            m_fLeftMaxY = a_fLeftMax;
            m_fRightMaxY = a_fRightMax;
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
            NavState = ENEMY_STATE.NONE;
        }
    }
}