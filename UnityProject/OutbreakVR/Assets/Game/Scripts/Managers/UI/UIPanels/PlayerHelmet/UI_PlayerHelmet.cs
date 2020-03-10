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

            //EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onWeaponChanged);
            //EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_WEAPON_RELOADED, onWeaponReloaded);
            //EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_WEAPON_FIRED, onWeaponFired);
            //EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_BULLETS_ADDED, onBulletsAdded);
            //EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_PLAYER_HEALTH_UPDATED, onPlayerHealthUpdated);

            //updateWeaponInterface();
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
            //EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onWeaponChanged);
            //EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_WEAPON_RELOADED, onWeaponReloaded);
            //EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_WEAPON_FIRED, onWeaponFired);
            //EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_BULLETS_ADDED, onBulletsAdded);
            //EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_HEALTH_UPDATED, onPlayerHealthUpdated);
            
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

        ///// <summary>
        ///// Event called on weapon changed
        ///// </summary>
        ///// <param name="a_EventHash"></param>
        //public void onWeaponChanged(EventHash a_EventHash)
        //{
        //    updateWeaponInterface();
        //}

        ///// <summary>
        ///// Event callback on weapon reloaded
        ///// </summary>
        ///// <param name="a_EventHash"></param>
        //private void onWeaponReloaded(EventHash a_EventHash)
        //{
        //    updateWeaponInterface();
        //}

        ///// <summary>
        ///// Event callback on weapon fired
        ///// </summary>
        ///// <param name="a_EventHash"></param>
        //private void onWeaponFired(EventHash a_EventHash)
        //{
        //    updateWeaponInterface();
        //}

        ///// <summary>
        ///// Event callback on bullets added
        ///// </summary>
        ///// <param name="a_EventHash"></param>
        //private void onBulletsAdded(EventHash a_EventHash)
        //{
        //    updateWeaponInterface();
        //}

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