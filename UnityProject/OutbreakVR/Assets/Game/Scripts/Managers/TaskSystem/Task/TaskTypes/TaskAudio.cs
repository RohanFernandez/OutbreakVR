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
        private const string ATTRIBUTE_IS_POOLED        = "IsPooled";
        private const string ATTRIBUTE_GAME_OBJ_ID      = "GameObjID";

        #endregion ATTRIBUTE_KEY

        private string m_strAudSrcID                = string.Empty;
        private string m_strAudClipID               = string.Empty;
        private bool m_bIsStart                     = false;
        private bool m_bIsLoop                      = false;
        private float m_fVolume                     = 1.0f;
        private AUDIO_SRC_TYPES m_AudSrcType        = 0;

        //If not pooled get gameobject ID
        private bool m_bIsPooled                    = true;
        private UnpooledAudioSource m_UnpooledAudSrc = null;

        public override void onInitialize()
        {
            base.onInitialize();
            m_strAudSrcID               = getString(ATTRIBUTE_AUDIO_SRC_ID);
            m_strAudClipID              = getString(ATTRIBUTE_AUDIO_CLIP_ID);
            m_bIsStart                  = getBool(ATTRIBUTE_IS_START);
            m_bIsLoop                   = getBool(ATTRIBUTE_IS_LOOP);
            m_fVolume                   = getFloat(ATTRIBUTE_VOLUME);
            m_bIsPooled                 = getBool(ATTRIBUTE_IS_POOLED, true);
            GameObject l_FoundGameObj   = GameObjectManager.GetGameObjectById(getString(ATTRIBUTE_GAME_OBJ_ID));
            if (l_FoundGameObj != null)
            {
                m_UnpooledAudSrc = l_FoundGameObj.GetComponent<UnpooledAudioSource>();
            }

            string l_strAudSrcType = getString(ATTRIBUTE_AUD_SRC_TYPE);
            if (string.IsNullOrEmpty(l_strAudSrcType))
            {
                m_AudSrcType = AUDIO_SRC_TYPES.AUD_SRC_SFX;
            }
            else
            {
                m_AudSrcType = (AUDIO_SRC_TYPES)System.Enum.Parse(typeof(AUDIO_SRC_TYPES), l_strAudSrcType);
            }
        }

        public override void onExecute()
        {
            base.onExecute();

            if (m_bIsStart)
            {
                if (m_bIsPooled)   //2d pooled audio source
                {
                    SoundManager.PlayAudio(m_strAudSrcID, m_strAudClipID, m_bIsLoop, m_fVolume, m_AudSrcType);
                }
                else//unpooled audio source
                {
                    if (m_UnpooledAudSrc != null)
                    {
                        m_UnpooledAudSrc.play(m_strAudClipID, m_bIsLoop, m_fVolume);
                    }
                }
            }
            else
            {
                if (m_bIsPooled)   //2d pooled audio source
                {
                    SoundManager.StopAudioSrcWithID(m_strAudSrcID);
                }
                else
                {
                    if (m_UnpooledAudSrc != null)
                    {
                        m_UnpooledAudSrc.stop();
                    }
                }
            }

            onComplete();
        }
    }
}