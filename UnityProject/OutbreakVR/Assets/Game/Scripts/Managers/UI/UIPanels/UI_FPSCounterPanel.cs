using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_FPSCounterPanel : AbsUISingleton
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_FPSCounterPanel s_Instance = null;

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

        private const float ONE_SECOND = 1.0f;
        private float m_fTimer = 0.0f;
        private int m_iFPS = 0;

        [SerializeField]
        private TMPro.TMP_Text m_txtFPS = null;

        void Update()
        {
            m_fTimer += Time.unscaledDeltaTime;
            m_iFPS++;
            if (m_fTimer >= ONE_SECOND)
            {
                m_txtFPS.text = m_iFPS.ToString();
                m_iFPS = 0;

                m_fTimer -= ONE_SECOND;
                if (m_fTimer >= ONE_SECOND)
                {
                    m_fTimer = 0.0f;
                }
            }
        }
    }
}