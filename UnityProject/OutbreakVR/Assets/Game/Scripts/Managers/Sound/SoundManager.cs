using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum AUDIO_SRC_TYPES
    {
        AUD_SRC_SFX,
        AUD_SRC_MUSIC,
    };


    [System.Serializable]
    public class AudioData
    {
        /// <summary>
        /// The unique audio ID.
        /// </summary>
        [SerializeField]
        private string m_strAudioID = string.Empty;
        public string AudioID
        {
            get{ return m_strAudioID; }
        }

        /// <summary>
        /// The audio file referenced with the ID.
        /// </summary>
        [SerializeField]
        private AudioClip m_AudClip = null;
        public AudioClip AudClip
        {
            get { return m_AudClip; }
        }
    }

    public class SoundManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static SoundManager s_Instance = null;

        /// <summary>
        /// Mute all SFX.
        /// Set the volume of all the sound effect audio clips to 0 if true 1 if false.
        /// </summary>
        [SerializeField]
        private bool m_bSFXOn = false;
        public static bool IsSFXOn
        {
            get { return s_Instance.m_bSFXOn; }
            set {
                bool l_bSFXBeforeToggle = s_Instance.m_bSFXOn;
                s_Instance.m_bSFXOn = value;

                if (l_bSFXBeforeToggle != value)
                {
                    EventHash l_Eventhash = EventManager.GetEventHashtable();
                    l_Eventhash.Add(GameEventTypeConst.ID_AUDIO_SRC_TYPE, AUDIO_SRC_TYPES.AUD_SRC_SFX);
                    l_Eventhash.Add(GameEventTypeConst.ID_IS_AUDIO_MODE_ACTIVATED, value);
                    EventManager.Dispatch(GAME_EVENT_TYPE.ON_AUDIO_MODE_TOGGLED, l_Eventhash);
                }

                s_Instance.toggleMuteAllActive(!s_Instance.m_bSFXOn, AUDIO_SRC_TYPES.AUD_SRC_SFX);
            }
        }

        /// <summary>
        /// Mute/Unmute all music.
        /// Set the volume of all the background/ambient audio clips to 0 if true 1 if false..
        /// </summary>
        [SerializeField]
        private bool m_bIsMusicOn = false;
        public static bool IsMusicOn
        {
            get { return s_Instance.m_bIsMusicOn; }
            set {
                bool l_bMusicBeforeToggle = s_Instance.m_bSFXOn;

                s_Instance.m_bIsMusicOn = value;

                if (l_bMusicBeforeToggle != value)
                {
                    EventHash l_Eventhash = EventManager.GetEventHashtable();
                    l_Eventhash.Add(GameEventTypeConst.ID_AUDIO_SRC_TYPE, AUDIO_SRC_TYPES.AUD_SRC_MUSIC);
                    l_Eventhash.Add(GameEventTypeConst.ID_IS_AUDIO_MODE_ACTIVATED, value);
                    EventManager.Dispatch(GAME_EVENT_TYPE.ON_AUDIO_MODE_TOGGLED, l_Eventhash);
                }

                s_Instance.toggleMuteAllActive(!s_Instance.m_bIsMusicOn, AUDIO_SRC_TYPES.AUD_SRC_MUSIC);
            }
        }

        /// <summary>
        /// List of all audio files that will be played via the soundmanger
        /// </summary>
        [SerializeField]
        private List<AudioData> m_lstAudioData = null;

        /// <summary>
        /// The prefab of the audio src that will be placed in the pool
        /// </summary>
        [SerializeField]
        private PooledAudioSource m_ManagedAudioSrcPrefab = null;

        /// <summary>
        /// The pool to use for reuse of managed audio source object
        /// </summary>
        private ManagedAudioSrcPool m_AudioSrcPool = null;

        /// <summary>
        /// Sets singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            m_AudioSrcPool = new ManagedAudioSrcPool(m_ManagedAudioSrcPrefab, this.gameObject, 10);
        }

        /// <summary>
        /// Destroys singleton instance
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }

            m_AudioSrcPool.returnAll();

            s_Instance = null;
        }

        /// <summary>
        /// Plays audio with a specific audio src id.
        /// </summary>
        /// <param name="m_strAudioSrcId"></param>
        /// <param name="a_strAudioID"></param>
        /// <param name="a_bIsLoop"></param>
        /// <param name="a_fVolume"></param>
        /// <param name="a_AudSrcType"></param>
        /// <param name="a_actionOnComplete"></param>
        public static void PlayAudio(string a_strAudioSrcId, string a_strAudioID, bool a_bIsLoop, float a_fVolume,
            AUDIO_SRC_TYPES a_AudSrcType, System.Action a_actionOnComplete = null)
        {
            AudioData l_AudData = GetAudDataWithID(a_strAudioID);
            if (l_AudData == null)
            {
                Debug.LogError("SoundManager::PlayAudio:: Cannot Play Audio Data with id : '"+ a_strAudioID + "'");
                return;
            }

            PooledAudioSource l_ManagedAudSrc = s_Instance.getCurrentlyPlayingAudSrc(a_strAudioSrcId);
            if (l_ManagedAudSrc == null)
            {
                l_ManagedAudSrc = s_Instance.m_AudioSrcPool.getObject();
            }
            
            l_ManagedAudSrc.gameObject.SetActive(true);
            l_ManagedAudSrc.play(a_strAudioSrcId, l_AudData, a_bIsLoop, a_fVolume, a_AudSrcType, a_actionOnComplete);
        }

        /// <summary>
        /// Stops Audio src with ID from the currently playing list
        /// </summary>
        /// <param name="m_strAudioSrcId"></param>
        public static void StopAudioSrcWithID(string m_strAudioSrcId)
        {
            if (s_Instance == null)
            {
                Debug.LogError("SoundManager::StopAudioSrcWithID:: SoundManager is null, cannot stop audio with ID:" + m_strAudioSrcId);
                return;
            }

            PooledAudioSource l_ManagedAudSrc = s_Instance.getCurrentlyPlayingAudSrc(m_strAudioSrcId);
            if (l_ManagedAudSrc == null)
            {
                Debug.Log("Soundmanager::StopAudioSrcWithID:: Cannot find audio source with ID: '" + m_strAudioSrcId + "'");
                return;
            }

            ReturnAudSrcToPool(l_ManagedAudSrc);
        }

        /// <summary>
        /// Returns current active audio src to pool
        /// </summary>
        public static void ReturnAudSrcToPool(PooledAudioSource a_AudSrc)
        {
            a_AudSrc.stop();
            s_Instance.m_AudioSrcPool.returnToPool(a_AudSrc);
        }

        /// <summary>
        /// Gets currently playing ManagedAudSrc ID with ID name
        /// </summary>
        /// <param name="a_strAudSrcId"></param>
        /// <returns></returns>
        private PooledAudioSource getCurrentlyPlayingAudSrc(string a_strAudSrcId)
        {
            int l_iCurrentlyPlayingCount = s_Instance.m_AudioSrcPool.getActiveList().Count;
            for (int l_iCurrentIndex = 0; l_iCurrentIndex < l_iCurrentlyPlayingCount; l_iCurrentIndex++)
            {
                PooledAudioSource l_CurrentManagedAudSrc = s_Instance.m_AudioSrcPool.getActiveList()[l_iCurrentIndex];
                if (l_CurrentManagedAudSrc.AudioSrcID.Equals(a_strAudSrcId))
                {
                    return l_CurrentManagedAudSrc;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns audio data with ID
        /// </summary>
        /// <param name="a_strAudioID"></param>
        /// <returns></returns>
        public static AudioData GetAudDataWithID(string a_strAudioID)
        {
            int l_iAudDataCount = s_Instance.m_lstAudioData.Count;
            for (int l_iAudDataIndex = 0; l_iAudDataIndex < l_iAudDataCount; l_iAudDataIndex++)
            {
                AudioData l_CurrentAudData = s_Instance.m_lstAudioData[l_iAudDataIndex];
                if (l_CurrentAudData.AudioID.Equals(a_strAudioID))
                {
                    return l_CurrentAudData;
                }
            }
            return null;
        }

        /// <summary>
        /// Mutes/ unmutes all active audio src with given type
        /// </summary>
        /// <param name="a_bIsMute"></param>
        /// <param name="a_AudSrcType"></param>
        private void toggleMuteAllActive(bool a_bIsMute, AUDIO_SRC_TYPES a_AudSrcType)
        {
            int l_iActiveSrcCount = m_AudioSrcPool.getActiveList().Count;
            for (int l_iCurrentindex = 0; l_iCurrentindex < l_iActiveSrcCount; l_iCurrentindex++)
            {
                PooledAudioSource l_CurrentManagedAudSrc = m_AudioSrcPool.getActiveList()[l_iCurrentindex];

                if (l_CurrentManagedAudSrc.AudSrcType == a_AudSrcType)
                {
                    l_CurrentManagedAudSrc.mute(a_bIsMute);
                }
            }
        }
    }
}