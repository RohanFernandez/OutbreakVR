using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ProximityBomb : StaticEnemy
    {
        /// <summary>
        /// The time after which the player is detected within distance that the bomb will blast
        /// </summary>
        [SerializeField]
        private float m_fTimeToDetonateOnAlert = 2.0f;

        /// <summary>
        /// The time passed since enemy entered alert mode
        /// </summary>
        private float m_fCurrTimeInAlertMode = 0.0f;

        /// <summary>
        /// The blast damage max, lerp to the max attack radius 
        /// </summary>
        [SerializeField]
        private int m_iBlastDamageMax = 20;

        [SerializeField]
        private bool m_bIsPlayerWithingProximityBlast = false;

        public override void activateEnemy()
        {
            base.activateEnemy();
            m_bIsPlayerWithingProximityBlast = false;
        }

        public override void deactivateEnemy()
        {
            base.deactivateEnemy();
            m_bIsPlayerWithingProximityBlast = false;
        }

        protected override void onStateChanged(ENEMY_STATE l_OldNavState, ENEMY_STATE a_NavState)
        {
            base.onStateChanged(l_OldNavState, a_NavState);

            switch (a_NavState)
            {
                case ENEMY_STATE.IDLE:
                    m_actNavStateUpdate = onIdleStateUpdate;
                    break;

                case ENEMY_STATE.ALERT:
                    m_fCurrTimeInAlertMode = 0.0f;
                    m_actNavStateUpdate = onAlertStateUpdate;
                    break;

                case ENEMY_STATE.NONE:
                    m_actNavStateUpdate = null;
                    break;

                case ENEMY_STATE.DEAD:
                    m_actNavStateUpdate = null;

                    //TODO:: Add blast anim

                    Vector3 l_v3PlayerPos = PlayerManager.GetPosition();
                    m_RayDetector.origin = transform.position;
                    m_RayDetector.direction = (l_v3PlayerPos - transform.position).normalized;
                    RaycastHit l_RaycastHit;
                    if (Physics.Raycast(m_RayDetector, out l_RaycastHit, m_AttackLayerMask) &&
                        l_RaycastHit.collider != null &&
                        l_RaycastHit.collider.tag.Equals(GameConsts.TAG_PLAYER, System.StringComparison.OrdinalIgnoreCase) &&
                        m_bIsPlayerWithingProximityBlast)
                    {
                        float l_fDistanceFromPlayer = Vector3.Distance(transform.position, l_v3PlayerPos);
                        float l_fDamageMult = Mathf.Lerp(0.0f, m_fAttackRadius, l_fDistanceFromPlayer / m_fAttackRadius);
                        PlayerManager.InflictDamage((int)(m_iBlastDamageMax * l_fDamageMult));
                    }
                    break;
            }
        }

        private void onIdleStateUpdate()
        {
            
        }

        private void onAlertStateUpdate()
        {
            m_fCurrTimeInAlertMode += Time.deltaTime;
            if (m_fCurrTimeInAlertMode > m_fTimeToDetonateOnAlert)
            {
                onKilled();
            }
        }

        /// <summary>
        /// To check when the player collider enters this this trigger
        /// </summary>
        private void OnTriggerEnter(Collider a_Collider)
        {
            PlayerController l_PlayerController = a_Collider.GetComponent<PlayerController>();
            if (l_PlayerController != null && NavState == ENEMY_STATE.IDLE)
            {
                NavState = ENEMY_STATE.ALERT;
                m_bIsPlayerWithingProximityBlast = true;
            }
        }

        /// <summary>
        /// To check when the player collider enters this this trigger
        /// </summary>
        private void OnTriggerExit(Collider a_Collider)
        {
            PlayerController l_PlayerController = a_Collider.GetComponent<PlayerController>();
            if (l_PlayerController != null && NavState == ENEMY_STATE.IDLE)
            {
                m_bIsPlayerWithingProximityBlast = false;
            }
        }
    }
}