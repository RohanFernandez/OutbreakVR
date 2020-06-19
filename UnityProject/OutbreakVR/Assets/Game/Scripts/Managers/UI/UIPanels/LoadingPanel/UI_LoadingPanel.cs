using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_LoadingPanel : AbsUISingleton
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_LoadingPanel s_Instance = null;

        [SerializeField]
        private Sprite m_sprGreenBullet = null;

        [SerializeField]
        private Sprite m_spRedBullet = null;

        [SerializeField]
        private UnityEngine.UI.Image m_imgBulletBackground = null;

        [SerializeField]
        private UnityEngine.UI.Image m_imgBulletForeground = null;

        [SerializeField]
        private UnityEngine.UI.Slider m_ReloadSlider = null;

        private float m_fScrollTime = 0.0f;

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
        }

        public static void Hide()
        {
            s_Instance.hide();
        }

        public override void show(string a_strCode = "")
        {
            base.show();

            m_imgBulletBackground.sprite = m_spRedBullet;
            m_imgBulletForeground.sprite = m_sprGreenBullet;
            m_fScrollTime = 0.0f;
        }

        void Update()
        {
            m_fScrollTime += Time.deltaTime;

            if (m_fScrollTime > 1.0f)
            {
                m_fScrollTime = 0.0f;

                Sprite l_BgSprite = m_imgBulletBackground.sprite;
                Sprite l_FgSprite = m_imgBulletForeground.sprite;

                m_imgBulletBackground.sprite = l_FgSprite;
                m_imgBulletForeground.sprite = l_BgSprite;
            }

            m_ReloadSlider.value = m_fScrollTime;
        }
    }
}