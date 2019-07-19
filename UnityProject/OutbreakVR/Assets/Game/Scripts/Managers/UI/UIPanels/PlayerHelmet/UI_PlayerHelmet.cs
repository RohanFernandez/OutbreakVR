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

        #region WEAPON

        /// <summary>
        /// The current weapon type text
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtWeaponType = null;

        /// <summary>
        /// The current weapon category text
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtWeaponCategory = null;

        /// <summary>
        /// The first weapon mag bullet count
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtFirstMagBulletCount = null;

        /// <summary>
        /// The total bullets currently available in the current weapon
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtTotalBulletsAvailable = null;

        /// <summary>
        /// The panel gameobject that holds the current weapons bullet
        /// </summary>
        [SerializeField]
        private GameObject m_goBulletPanelParent = null;

        #endregion WEAPON

        #region RELOAD

        /// <summary>
        /// The panel that holds all the components of the reload progress panel
        /// </summary>
        [SerializeField]
        private GameObject m_goReloadProgressPanel = null;

        #endregion RELOAD

        #region HEALTH

        /// <summary>
        /// The panel that displays the player health
        /// </summary>
        [SerializeField]
        private GameObject m_goHealthPanel = null;

        [SerializeField]
        private TMPro.TMP_Text m_txtPlayerHealth = null;

        #endregion HEALTH

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

            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onWeaponChanged);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_WEAPON_RELOADED, onWeaponReloaded);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_WEAPON_FIRED, onWeaponFired);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_BULLETS_ADDED, onBulletsAdded);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_PLAYER_HEALTH_UPDATED, onPlayerHealthUpdated);

            updateWeaponInterface();
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
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onWeaponChanged);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_WEAPON_RELOADED, onWeaponReloaded);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_WEAPON_FIRED, onWeaponFired);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_BULLETS_ADDED, onBulletsAdded);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_HEALTH_UPDATED, onPlayerHealthUpdated);
            
            s_Instance = null;
        }

        public static void Show()
        {
            s_Instance.show();
            s_Instance.updateWeaponInterface();
        }

        public static void Hide()
        {
            s_Instance.hide();
        }

        /// <summary>
        /// Updates the weapon related information
        /// </summary>
        private void updateWeaponInterface()
        {
            WeaponBase l_CurrentWeaponBase = WeaponManager.GetCurrentWeaponBase();

            if (l_CurrentWeaponBase != null)
            {
                m_txtWeaponCategory.text = l_CurrentWeaponBase.m_WeaponCategoryType.ToString();
                m_txtWeaponType.text = l_CurrentWeaponBase.m_WeaponType.ToString();

                bool l_bIsWeaponAGun = l_CurrentWeaponBase.m_WeaponCategoryType != WEAPON_CATEGORY_TYPE.MELEE;
                m_goBulletPanelParent.SetActive(l_bIsWeaponAGun);

                if (l_bIsWeaponAGun)
                {
                    GunWeaponBase l_GunWeaponBase = (GunWeaponBase)l_CurrentWeaponBase;   
                    m_txtFirstMagBulletCount.text = l_GunWeaponBase.BulletCountInFirstMag.ToString();
                    m_txtTotalBulletsAvailable.text = l_GunWeaponBase.TotalBullets.ToString();
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// Event called on weapon changed
        /// </summary>
        /// <param name="a_EventHash"></param>
        public void onWeaponChanged(EventHash a_EventHash)
        {
            updateWeaponInterface();
        }

        /// <summary>
        /// Event callback on weapon reloaded
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onWeaponReloaded(EventHash a_EventHash)
        {
            updateWeaponInterface();
        }

        /// <summary>
        /// Event callback on weapon fired
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onWeaponFired(EventHash a_EventHash)
        {
            updateWeaponInterface();
        }

        /// <summary>
        /// Event callback on bullets added
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onBulletsAdded(EventHash a_EventHash)
        {
            updateWeaponInterface();
        }

        /// <summary>
        /// Shows Hide the reload progress bar
        /// </summary>
        /// <param name="a_bIsReloadInProgress"></param>
        public static void ToggleReloadProgressBar(bool a_bIsReloadInProgress)
        {
            s_Instance.m_goReloadProgressPanel.SetActive(a_bIsReloadInProgress);
        }

        /// <summary>
        /// Updates the reload progress bar
        /// </summary>
        /// <param name="a_fReloadTimeElapsed"></param>
        /// <param name="a_fCurrentWeaponReloadTime"></param>
        public static void UpdateReloadProgressBar(float a_fReloadTimeElapsed, float a_fCurrentWeaponReloadTime)
        {
            
        }

        /// <summary>
        /// Callback called on player health is updated
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onPlayerHealthUpdated(EventHash a_EventHash)
        {
            m_txtPlayerHealth.text = a_EventHash[GameEventTypeConst.ID_PLAYER_HEALTH].ToString();
        }
    }
}