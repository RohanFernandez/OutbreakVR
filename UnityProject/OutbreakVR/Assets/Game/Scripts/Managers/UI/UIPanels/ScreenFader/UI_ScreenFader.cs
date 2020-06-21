using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_ScreenFader : AbsUISingleton
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_ScreenFader s_Instance = null;

        /// <summary>
        /// The image component that fades
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image m_imgFader = null;

        /// <summary>
        /// Action to execute on fade complete
        /// </summary>
        private System.Action m_actOnFadeComplete = null;

        /// <summary>
        /// The time the fader takes to complete
        /// </summary>
        [SerializeField]
        private float m_fTotalFadeTime = 2.0f;

        /// <summary>
        /// Current amount of time taken in fade
        /// </summary>
        private float m_fTimeTakenInFade = 0.0f;

        private bool m_bIsFadeToBlack = true;
        private bool IsFadeToBlack
        {
            set {
                m_bIsFadeToBlack = value;
                if (m_bIsFadeToBlack)
                {
                    m_fStartAlpha = 0.0f;
                    m_fEndAlpha = 1.0f;
                }
                else
                {
                    m_fStartAlpha = 1.0f;
                    m_fEndAlpha = 0.0f;
                }
            }
        }

        private float m_fStartAlpha = 0.0f;
        private float m_fEndAlpha = 1.0f;

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
            ShowWithActionOnComplete(null, true);
        }

        public static void ShowFadeToBlack(bool a_bIsFadeToBlack)
        {
            ShowWithActionOnComplete(null, a_bIsFadeToBlack);
        }

        /// <summary>
        /// Displays the Fader and starts the fader
        /// </summary>
        public static void ShowWithActionOnComplete(System.Action a_actOnFaderComplete, bool a_bIsFadeToBlack = true)
        {
            s_Instance.IsFadeToBlack = a_bIsFadeToBlack;
            s_Instance.m_actOnFadeComplete = a_actOnFaderComplete;
            s_Instance.m_fTimeTakenInFade = 0.0f;

            Color l_FadeColor = s_Instance.m_imgFader.color;
            s_Instance.m_imgFader.color = new Color(l_FadeColor.r, l_FadeColor.g, l_FadeColor.b, s_Instance.m_fStartAlpha);
            s_Instance.show();
        }

        /// <summary>
        /// Hides the Fader
        /// </summary>
        public static void Hide()
        {
            s_Instance.m_fTimeTakenInFade = 0.0f;
            s_Instance.hide();
        }

        private void Update()
        {
            m_fTimeTakenInFade += Time.deltaTime;

            Color l_FadeColor = m_imgFader.color;
            m_imgFader.color = new Color(l_FadeColor.r, l_FadeColor.g, l_FadeColor.b, Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, m_fTimeTakenInFade / m_fTotalFadeTime));

            if (m_fTimeTakenInFade >= m_fTotalFadeTime)
            {
                Hide();
                if (m_actOnFadeComplete != null)
                {
                    m_actOnFadeComplete();
                }
            }
        }
    }
}