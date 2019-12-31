using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class AutomatedTurretTrigger : MonoBehaviour
    {
        /// <summary>
        /// The turret lever
        /// </summary>
        [SerializeField]
        private AutomatedTurretDeactivator m_TurretDeactivator = null;

        /// <summary>
        /// The trigger collider of this game object
        /// </summary>
        [SerializeField]
        private Collider m_colTrigger = null;

        /// <summary>
        /// Activating the turret trigger
        /// </summary>
        public void onActivate()
        {
            m_colTrigger.enabled = true;
        }

        /// <summary>
        /// Deactivating the turret trigger
        /// </summary>
        private void onDeactivate()
        {
            m_colTrigger.enabled = false;
            m_TurretDeactivator.onLeverActivate();
            SoundManager.PlayAudio(GameConsts.AUD_SRC_TURRET_TRIG_ACTIVATE, GameConsts.AUD_CLIP_TURRET_TRIG_ACTIVATE, false, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_SFX);
        }

        /// <summary>
        /// on player entering the trigger
        /// On triggerring the turret will start working
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider a_Other)
        {
            if (a_Other.tag.Equals(GameConsts.TAG_PLAYER, System.StringComparison.OrdinalIgnoreCase))
            {
                if (m_TurretDeactivator != null)
                {
                    onDeactivate();
                }
            }
        }
    }
}