using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_CinematicTextPanel : AbsUISingleton
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_CinematicTextPanel s_Instance = null;

        [SerializeField]
        private TMPro.TMP_Text m_txtMessage = null;

        [SerializeField]
        private string m_strMessage = string.Empty;

        [SerializeField]
        private float m_fTimerBetweenCharReveal = 0.2f;

        private float m_fTimeSinceLastChar = 0.0f;

        private int m_iMessageLength = 0;

        [SerializeField]
        UnpooledAudioSource m_UnpooledAudSrc = null;

        /// <summary>
        /// initializes, sets singleton to this
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
        }

        /// <summary>
        /// sets singleton to null
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }

            s_Instance = null;
        }

        public static void Show()
        {
            s_Instance.show();
            s_Instance.m_fTimeSinceLastChar = 0.0f;
            s_Instance.m_txtMessage.maxVisibleCharacters = 0;
            s_Instance.m_iMessageLength = s_Instance.m_strMessage.Length;
            s_Instance.m_txtMessage.text = s_Instance.m_strMessage;
        }

        public static void Hide()
        {
            s_Instance.hide();
        }

        void Update()
        {
            m_fTimeSinceLastChar += Time.deltaTime;
            if (m_fTimeSinceLastChar > m_fTimerBetweenCharReveal)
            {
                m_fTimeSinceLastChar = 0.0f;

                if (m_txtMessage.maxVisibleCharacters < s_Instance.m_iMessageLength)
                {
                    m_txtMessage.maxVisibleCharacters++;
                    m_UnpooledAudSrc.play();
                }
            }
        }
    }
}