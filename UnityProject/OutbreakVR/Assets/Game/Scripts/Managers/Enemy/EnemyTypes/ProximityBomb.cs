using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace ns_Mashmo
{
    public class ProximityBomb : StaticEnemy
    {
        /// <summary>
        /// The time after which the player is detected within distance that the bomb will blast
        /// </summary>
        [SerializeField]
        private float m_fDetonationTimeAfterAlert = 2.0f;

        /// <summary>
        /// The time passed since enemy entered alert mode
        /// </summary>
        private float m_fCurrTimeInAlertMode = 0.0f;

        /// <summary>
        /// The blast damage max, lerp to the max attack radius 
        /// </summary>
        [SerializeField]
        private int m_iBlastDamageMax = 20;

        /// <summary>
        /// The animator to open flaps on player detected
        /// </summary>
        [SerializeField]
        private Animator m_Animator = null;

        /// <summary>
        /// The particle system to be played on blast
        /// </summary>
        [SerializeField]
        private ParticleSystem m_BlastParticleSystem = null;

        /// <summary>
        /// The body of the proximity bomb
        /// </summary>
        [SerializeField]
        private GameObject m_ProximityBombBody = null;

        /// <summary>
        /// Audio source to play blast and armed audio
        /// </summary>
        [SerializeField]
        private UnpooledAudioSource m_AudioSrc = null;

        /// <summary>
        /// THe anim trigger to start detonation of the bomb
        /// </summary>
        private const string ANIM_TRIGGER_START_DETONATE = "StartDetonate";

        /// <summary>
        /// THe anim trigger to set the idle animation of the bomb
        /// </summary>
        private const string ANIM_TRIGGER_IDLE = "Idle";

        public override void activateEnemy()
        {
            base.activateEnemy();
            m_ProximityBombBody.SetActive(true);
            NavState = ENEMY_STATE.IDLE;
            m_AudioSrc.stop();
        }

        public override void deactivateEnemy()
        {
            base.deactivateEnemy();
            NavState = ENEMY_STATE.NONE;
            m_AudioSrc.stop();
        }

        protected override void onStateChanged(ENEMY_STATE l_OldNavState, ENEMY_STATE a_NavState)
        {
            base.onStateChanged(l_OldNavState, a_NavState);

            switch (a_NavState)
            {
                case ENEMY_STATE.IDLE:
                    m_Animator.SetTrigger(ANIM_TRIGGER_IDLE);
                    m_actNavStateUpdate = null;
                    break;

                case ENEMY_STATE.ALERT:

                    m_AudioSrc.play(GameConsts.AUD_CLIP_PROXIMITY_BOMB_ARMED, false, 1.0f);

                    m_Animator.SetTrigger(ANIM_TRIGGER_START_DETONATE);
                    m_fCurrTimeInAlertMode = 0.0f;
                    m_actNavStateUpdate = onAlertStateUpdate;
                    break;

                case ENEMY_STATE.NONE:
                    m_actNavStateUpdate = null;
                    break;

                case ENEMY_STATE.DEAD:
                    m_actNavStateUpdate = null;

                    if (m_BlastParticleSystem != null)
                    {
                        m_BlastParticleSystem.Play();
                    }

                    // on blast the body object should disappear
                    m_ProximityBombBody.SetActive(false);

                    m_AudioSrc.play(GameConsts.AUD_CLIP_PROXIMITY_BOMB_BLAST ,false, 1.0f);

                    ///This manages if the bomb is shot it will blast, alerting all enemies without the bomb going into the alert state
                    EnemyManager.ForceAllEnemyAlertOnProximity();

                    Vector3 l_v3PlayerPos = PlayerManager.GetPosition();
                    m_RayDetector.origin = transform.position;
                    m_RayDetector.direction = (l_v3PlayerPos - transform.position).normalized;
                    RaycastHit l_RaycastHit;
                    if (Physics.Raycast(m_RayDetector, out l_RaycastHit, m_fMaxDamagePlayerDamageRadius, m_AttackLayerMask) &&
                        l_RaycastHit.collider != null)
                    {
                        float l_fDistanceFromPlayer = Vector3.Distance(transform.position, l_v3PlayerPos);
                        float l_fDamageMult = Mathf.Lerp(0.0f, m_fMaxDamagePlayerDamageRadius, l_fDistanceFromPlayer / m_fMaxDamagePlayerDamageRadius);
                        PlayerManager.InflictDamage((int)(m_iBlastDamageMax * l_fDamageMult));
                    }
                    break;
            }
        }

        private void onAlertStateUpdate()
        {
            m_fCurrTimeInAlertMode += Time.deltaTime;
            if (m_fCurrTimeInAlertMode > m_fDetonationTimeAfterAlert)
            {
                onKilled();
            }
        }

        /// <summary>
        /// To check when the player collider enters this this trigger
        /// </summary>
        private void OnTriggerEnter(Collider a_Collider)
        {
            if (GeneralUtils.IsLayerInLayerMask(m_AttackLayerMask, a_Collider.gameObject.layer) && NavState == ENEMY_STATE.IDLE)
            {
                NavState = ENEMY_STATE.ALERT;
            }
        }
    }
}