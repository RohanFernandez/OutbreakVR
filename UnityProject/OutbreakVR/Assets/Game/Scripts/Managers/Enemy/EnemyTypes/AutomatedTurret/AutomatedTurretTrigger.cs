﻿using System.Collections;
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
        public void onDeactivate()
        {
            m_colTrigger.enabled = false;
            SoundManager.PlayAudio(SoundConst.AUD_SRC_TURRET_TRIG_ACTIVATE, SoundConst.AUD_CLIP_TURRET_TRIG_ACTIVATE, false, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_SFX);
        }

        /// <summary>
        /// on player entering the trigger
        /// On triggerring the turret will start working
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider a_Other)
        {
            if (a_Other.gameObject.layer == LayerMask.NameToLayer(GameConsts.LAYER_NAME_PLAYER))
            {
                if (m_TurretDeactivator != null)
                {
                    onDeactivate();
                    m_TurretDeactivator.activateTurret();
                    m_TurretDeactivator.onLeverActivate();
                }
            }
        }
    }
}