using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UnpooledAudioSource : ManagedAudioSourceBase
    {
        /// <summary>
        /// Starts playing on enabled, else will have to manually call the play function
        /// </summary>
        [SerializeField]
        private bool m_bIsPlayOnEnabled = false;

        /// <summary>
        /// The ID of the audio clip to play
        /// </summary>
        [SerializeField]
        private string m_strAudClipID = string.Empty;

        /// <summary>
        /// Start audio source from playing
        /// </summary>
        private void OnEnable()
        {
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_AUDIO_MODE_TOGGLED, onAudioModeToggled);

            if (m_bIsPlayOnEnabled)
            {
                play(m_strAudClipID, m_AudSrc.loop, m_fVolume);
            }
        }

        /// <summary>
        /// Stop audio source from playing
        /// </summary>
        private void OnDisable()
        {
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_AUDIO_MODE_TOGGLED, onAudioModeToggled);
        }

        /// <summary>
        /// Callback called on either SFX audio mode is turned on/off
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onAudioModeToggled(EventHash a_EventHash)
        {
            AUDIO_SRC_TYPES l_AudSrcType = (AUDIO_SRC_TYPES)a_EventHash[GameEventTypeConst.ID_AUDIO_SRC_TYPE];
            bool l_bIsAudioModeActivated = (bool)a_EventHash[GameEventTypeConst.ID_IS_AUDIO_MODE_ACTIVATED];

            if (l_AudSrcType == m_AudSrcType)
            {
                if (m_AudSrc.clip != null)
                {
                    mute(!l_bIsAudioModeActivated);
                }
            }
        }

        /// <summary>
        /// Start playing audio clip with ID
        /// </summary>
        public void play(string a_strAudClipID, bool a_bIsLoop, float a_fVolume)
        {
            AudioData l_AudioData = SoundManager.GetAudDataWithID(a_strAudClipID);

            if (l_AudioData != null)
            {
                m_AudSrc.clip = l_AudioData.AudClip;
            }

            m_AudSrc.loop = a_bIsLoop;
            m_fVolume = a_fVolume;

            if (m_AudSrcType == AUDIO_SRC_TYPES.AUD_SRC_MUSIC)
            {
                m_AudSrc.volume = SoundManager.IsMusicOn ? m_fVolume : 0.0f;
            }
            else if (m_AudSrcType == AUDIO_SRC_TYPES.AUD_SRC_SFX)
            {
                m_AudSrc.volume = SoundManager.IsSFXOn ? m_fVolume : 0.0f;
            }

            if (l_AudioData != null)
            {
                m_AudSrc.Play();
            }
        }
    }
}