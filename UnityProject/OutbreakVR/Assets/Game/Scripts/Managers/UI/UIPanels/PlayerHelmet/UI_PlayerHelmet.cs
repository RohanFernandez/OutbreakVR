using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_PlayerHelmet : AbsUISingleton
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_PlayerHelmet s_Instance = null;

        #region RELOAD

        /// <summary>
        /// The panel that holds all the components of the reload progress panel
        /// </summary>
        [SerializeField]
        private GameObject m_goReloadProgressPanel = null;

        /// <summary>
        /// The UI slider of the reload progress bar
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Slider m_UIReloadSlider = null;

        #endregion RELOAD

        /// <summary>
        /// The helmet screen as an image in screen space that covers the screen
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image m_imgHelmetScreen = null;

        /// <summary>
        /// The sprite on the screen of the helmet showing that it is not cracked
        /// </summary>
        [SerializeField]
        private Sprite m_sprHelmetScreenUncracked = null;

        /// <summary>
        /// The sprite on the screen of the helmet showing that it is cracked
        /// </summary>
        [SerializeField]
        private Sprite m_sprHelmetScreenCracked = null;

        /// <summary>
        /// The img that displays the helmet strength, will be on only when the helmet screen is uncracked
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image m_imgHelmetStrength = null;

        /// <summary>
        /// The sprite that displays a weak helmet strength
        /// </summary>
        [SerializeField]
        private Sprite m_sprWeakHelmetStrength = null;

        /// <summary>
        /// The sprite that displays a moderate helmet strength
        /// </summary>
        [SerializeField]
        private Sprite m_sprModerateHelmetStrength = null;

        /// <summary>
        /// The sprite that displays a strong helmet strength
        /// </summary>
        [SerializeField]
        private Sprite m_sprStrongHelmetStrength = null;

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

        /// <summary>
        /// Shows Hide the reload progress bar
        /// </summary>
        /// <param name="a_bIsReloadInProgress"></param>
        public void toggleReloadProgressBar(bool a_bIsReloadInProgress)
        {
            m_goReloadProgressPanel.SetActive(a_bIsReloadInProgress);
        }

        /// <summary>
        /// Updates the reload progress bar
        /// </summary>
        /// <param name="a_fReloadTimeElapsed"></param>
        /// <param name="a_fCurrentWeaponReloadTime"></param>
        public static void UpdateReloadProgressBar(float a_fReloadTimeElapsed, float a_fCurrentWeaponReloadTime)
        {
            s_Instance.m_UIReloadSlider.value = a_fReloadTimeElapsed / a_fCurrentWeaponReloadTime;
        }

        /// <summary>
        /// Sets the cracked/uncracked sprite on the helmet screen image
        /// </summary>
        /// <param name="a_bIsCracked"></param>
        public void toggleHelmetScreen(bool a_bIsCracked, HelmetStrengthIndicator a_HelmetStrength)
        {
            if (a_bIsCracked)
            {
                m_imgHelmetStrength.gameObject.SetActive(false);
                m_imgHelmetScreen.sprite = m_sprHelmetScreenCracked;
            }
            else
            {
                m_imgHelmetStrength.gameObject.SetActive(true);
                m_imgHelmetScreen.sprite = m_sprHelmetScreenUncracked;

                if (a_HelmetStrength == HelmetStrengthIndicator.HELMET_STRENGTH_WEAK)
                {
                    m_imgHelmetStrength.sprite = m_sprWeakHelmetStrength;
                }
                else if (a_HelmetStrength == HelmetStrengthIndicator.HELMET_STRENGTH_MODERATE)
                {
                    m_imgHelmetStrength.sprite = m_sprModerateHelmetStrength;
                }
                else 
                {
                    m_imgHelmetStrength.sprite = m_sprStrongHelmetStrength;
                }
            }

        }
    }
}