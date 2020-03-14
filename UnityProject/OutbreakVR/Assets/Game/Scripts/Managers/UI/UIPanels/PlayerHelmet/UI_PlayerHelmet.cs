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
    }
}