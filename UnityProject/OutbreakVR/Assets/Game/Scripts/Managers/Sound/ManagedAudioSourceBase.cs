using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ManagedAudioSourceBase : MonoBehaviour
    {
        /// <summary>
        /// The reference of the audio src
        /// </summary>
        [SerializeField]
        protected AudioSource m_AudSrc = null;
        public AudioSource AudSrc
        {
            get { return m_AudSrc; }
        }

        /// <summary>
        /// Is the audio an effect of music
        /// </summary>
        [SerializeField]
        protected AUDIO_SRC_TYPES m_AudSrcType;
        public AUDIO_SRC_TYPES AudSrcType
        {
            get { return m_AudSrcType; }
        }

        /// <summary>
        /// The volume of the currently playing audio
        /// </summary>
        [SerializeField]
        protected float m_fVolume = 0.0f;
        public float Volume
        {
            get { return m_fVolume; }
        }

        /// <summary>
        /// Toggle mute/ unmute
        /// </summary>
        /// <param name="a_bIsMute"></param>
        public virtual void mute(bool a_bIsMute)
        {
            m_AudSrc.volume = a_bIsMute ? 0.0f : m_fVolume;
        }

        /// <summary>
        /// Stops playing audio
        /// </summary>
        public virtual void stop()
        {
            m_AudSrc.Stop();
        }
    }
}