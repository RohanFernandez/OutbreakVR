using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskAudio : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_AUDIO_SRC_ID     = "AudSrc";
        private const string ATTRIBUTE_AUDIO_CLIP_ID    = "AudClip";
        private const string ATTRIBUTE_IS_START         = "IsStart";
        private const string ATTRIBUTE_IS_LOOP          = "IsLoop";
        private const string ATTRIBUTE_VOLUME           = "Volume";
        private const string ATTRIBUTE_AUD_SRC_TYPE     = "SrcType";
        #endregion ATTRIBUTE_KEY

        private string m_strAudSrcID    = string.Empty;
        private string m_strAudClipID   = string.Empty;
        private bool m_bIsStart         = false;
        private bool m_bIsLoop          = false;
        private float m_fVolume         = 1.0f;
        private int m_iSrcType          = 0;

        public override void onInitialize()
        {
            base.onInitialize();
            m_strAudSrcID   = getString(ATTRIBUTE_AUDIO_SRC_ID);
            m_strAudClipID  = getString(ATTRIBUTE_AUDIO_CLIP_ID);
            m_bIsStart      = getBool(ATTRIBUTE_IS_START);
            m_bIsLoop       = getBool(ATTRIBUTE_IS_LOOP);
            m_fVolume       = getFloat(ATTRIBUTE_VOLUME);
            m_iSrcType      = getInt(ATTRIBUTE_AUD_SRC_TYPE);
        }

        public override void onExecute()
        {
            base.onExecute();
            if (m_bIsStart)
            {
                SoundManager.PlayAudio(m_strAudSrcID, m_strAudClipID, m_bIsLoop, m_fVolume, (AUDIO_SRC_TYPES)m_iSrcType);
            }
            else
            {
                SoundManager.StopAudioSrcWithID(m_strAudSrcID);
            }
            onComplete();
        }
    }
}