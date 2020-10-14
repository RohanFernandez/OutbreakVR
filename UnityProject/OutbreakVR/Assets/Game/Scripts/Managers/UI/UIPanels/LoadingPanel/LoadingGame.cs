using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class LoadingGame : LoadingBase
    {
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

        //public override void show()
        //{
        //    base.show();

        //    m_imgBulletBackground.sprite = m_spRedBullet;
        //    m_imgBulletForeground.sprite = m_sprGreenBullet;
        //    m_fScrollTime = 0.0f;
        //}

        //void Update()
        //{
        //    m_fScrollTime += Time.deltaTime;

        //    if (m_fScrollTime > 1.0f)
        //    {
        //        m_fScrollTime = 0.0f;

        //        Sprite l_BgSprite = m_imgBulletBackground.sprite;
        //        Sprite l_FgSprite = m_imgBulletForeground.sprite;

        //        m_imgBulletBackground.sprite = l_FgSprite;
        //        m_imgBulletForeground.sprite = l_BgSprite;
        //    }

        //    m_ReloadSlider.value = m_fScrollTime;
        //}
    }
}