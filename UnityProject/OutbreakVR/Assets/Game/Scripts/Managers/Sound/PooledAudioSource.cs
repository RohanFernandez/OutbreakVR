﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class PooledAudioSource : ManagedAudioSourceBase, IReusable
    {
        /// <summary>
        /// unique audio src ID
        /// </summary>
        [SerializeField]
        protected string m_strAudioSrcID = string.Empty;
        public string AudioSrcID
        {
            get { return m_strAudioSrcID; }
            set { m_strAudioSrcID = value; }
        }

        /// <summary>
        /// Action to be called on complete
        /// </summary>
        private System.Action m_actionOnComplete = null;

        /// <summary>
        /// Plays managed audio src
        /// </summary>
        /// <param name="m_strAudioSrcId"></param>
        /// <param name="a_AudData"></param>
        /// <param name="a_bIsLoop"></param>
        /// <param name="a_fVolume"></param>
        /// <param name="a_AudSrcType"></param>
        /// <param name="a_actionOnComplete"></param>
        public void play(string a_strAudioSrcId, AudioData a_AudData, bool a_bIsLoop, float a_fVolume,
            AUDIO_SRC_TYPES a_AudSrcType, System.Action a_actionOnComplete = null)
        {
            gameObject.name = a_strAudioSrcId;
            m_strAudioSrcID = a_strAudioSrcId;

            m_AudSrc.clip = a_AudData.AudClip;
            m_AudSrc.loop = a_bIsLoop;
            m_fVolume = a_fVolume;

            if (a_AudSrcType == AUDIO_SRC_TYPES.AUD_SRC_MUSIC)
            {
                m_AudSrc.volume = SoundManager.IsMusicOn ? m_fVolume : 0.0f;
            }
            else if (a_AudSrcType == AUDIO_SRC_TYPES.AUD_SRC_SFX)
            {
                m_AudSrc.volume = SoundManager.IsSFXOn ? m_fVolume : 0.0f;
            }
            
            m_AudSrcType = a_AudSrcType;
            m_actionOnComplete = a_actionOnComplete;
            m_AudSrc.Play();
        }

        private void Update()
        {
            if (!m_AudSrc.isPlaying)
            {
                SoundManager.ReturnAudSrcToPool(this);

                if (m_actionOnComplete != null)
                {
                    m_actionOnComplete();
                }
            }
        }

        public virtual void onReturnedToPool()
        {
            
        }

        public virtual void onRetrievedFromPool()
        {
            
        }
    }
}