using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InventoryHelmet : InventoryItem
    {
        /// <summary>
        /// UI player helmet that displays the reloading sign and various screen effects
        /// </summary>
        [SerializeField]
        private UI_PlayerHelmet m_UIPlayerHelmet = null;

        /// <summary>
        /// UI that displays the health on the arm watch
        /// </summary>
        [SerializeField]
        private UI_HealthArmMonitor m_UIHealthArmMonitor = null;

        /// <summary>
        /// UI that displays the bullet count on the arm watch
        /// </summary>
        [SerializeField]
        private UI_BulletsArmMonitor m_UIBulletsArmMonitor = null;

        /// <summary>
        /// sets if the helmet is cracked
        /// </summary>
        [SerializeField]
        private bool m_bIsHelmetCracked = false;
        public bool IsHelmetCracked
        {
            get { return m_bIsHelmetCracked; }
            set {
                bool l_bIsHelmetCrackedBeforeChange = m_bIsHelmetCracked;
                m_bIsHelmetCracked = value;
                if (l_bIsHelmetCrackedBeforeChange != m_bIsHelmetCracked)
                {
                    onPlayerHelmetCracked();
                }
            }
        }

        public override void initialize()
        {
            base.initialize();
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onCurrentWeaponChanged);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, onPlayerStateChanged);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_RELOAD_TOGGLED, onReloadToggled);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_PLAYER_HEALTH_UPDATED, onPlayerHealthUpdated);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_WEAPON_RELOADED, onWeaponReloaded);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_WEAPON_FIRED, onWeaponFired);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_BULLETS_ADDED, onBulletsAdded);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onWeaponChanged);
        }

        public override void destroy()
        {
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onCurrentWeaponChanged);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, onPlayerStateChanged);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_RELOAD_TOGGLED, onReloadToggled);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_HEALTH_UPDATED, onPlayerHealthUpdated);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_WEAPON_RELOADED, onWeaponReloaded);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_WEAPON_FIRED, onWeaponFired);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_BULLETS_ADDED, onBulletsAdded);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onWeaponChanged);
            base.destroy();
        }

        /// <summary>
        /// Callback on the current weapon changed, 
        /// </summary>
        private void onCurrentWeaponChanged(EventHash a_EventHash)
        {
            WEAPON_TYPE l_WeaponType = (WEAPON_TYPE)a_EventHash[GameEventTypeConst.ID_NEW_WEAPON_TYPE];
            WeaponBase l_WeaponBase =  (WeaponBase)a_EventHash[GameEventTypeConst.ID_NEW_WEAPON_BASE];

            if (l_WeaponType != WEAPON_TYPE.NONE && l_WeaponBase != null)
            {
                Transform l_HealthArmMonitorParent = l_WeaponBase.ArmMonitorParent_Health;
                m_UIHealthArmMonitor.transform.SetParent(l_HealthArmMonitorParent.transform);
                m_UIHealthArmMonitor.transform.localPosition = Vector3.zero;
                m_UIHealthArmMonitor.transform.localRotation = Quaternion.identity;
                m_UIHealthArmMonitor.transform.localScale = Vector3.one;

                Transform l_BulletsArmMonitorParent = l_WeaponBase.ArmMonitorParent_BulletCount;
                m_UIBulletsArmMonitor.transform.SetParent(l_BulletsArmMonitorParent.transform);
                m_UIBulletsArmMonitor.transform.localPosition = Vector3.zero;
                m_UIBulletsArmMonitor.transform.localRotation = Quaternion.identity;
                m_UIBulletsArmMonitor.transform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// On player helmet cracked
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onPlayerHelmetCracked()
        {
            updateHelmet();
        }

        /// <summary>
        /// On player state changed, enable helmet and arm monitor if required
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onPlayerStateChanged(EventHash a_EventHash)
        {
            updateHelmet();
        }

        /// <summary>
        /// Called when the count of this item has changed
        /// </summary>
        protected override void onItemAmountInInventoryChanged()
        {
            base.onItemAmountInInventoryChanged();
            updateHelmet();
        }

        /// <summary>
        /// Callback called when the reload is started/stopped
        /// </summary>
        private void onReloadToggled(EventHash a_EventHash)
        {
            updateHelmet();
        }

        /// <summary>
        /// Callback called on player health is updated
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onPlayerHealthUpdated(EventHash a_EventHash)
        {
            updateHelmet();
        }

        /// <summary>
        /// Callback called on weapon reloaded
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onWeaponReloaded(EventHash a_EventHash)
        {
            updateHelmet();
        }

        /// <summary>
        /// Callback called on weapon fired
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onWeaponFired(EventHash a_EventHash)
        {
            updateHelmet();
        }

        /// <summary>
        /// Callback called on bullets added
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onBulletsAdded(EventHash a_EventHash)
        {
            updateHelmet();
        }

        /// <summary>
        /// Callback called on current weapon changed
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onWeaponChanged(EventHash a_EventHash)
        {
            updateHelmet();
        }
        
        /// <summary>
        /// Sets if the helmet should be enabled/ disabled
        /// Sets the current weapons bullet count, weapon type
        /// Displays reloading if helmet not cracked
        /// </summary>
        private void updateHelmet()
        {
            PLAYER_STATE l_CurrentPlayerState = PlayerManager.PlayerState;

            // check if the player is in the game and the player is wearing a helmet
            if (l_CurrentPlayerState == PLAYER_STATE.MENU_SELECTION ||
                l_CurrentPlayerState == PLAYER_STATE.NO_INTERACTION ||
                !IsItemInInventory)
            {
                m_UIPlayerHelmet.hide();
                m_UIHealthArmMonitor.hide();
                m_UIBulletsArmMonitor.hide();
            }
            else
            {
                m_UIPlayerHelmet.show();

                //is the helmet cracked
                if (IsHelmetCracked)
                {
                    m_UIBulletsArmMonitor.hide();
                    m_UIHealthArmMonitor.hide();
                    m_UIPlayerHelmet.toggleReloadProgressBar(false);
                }
                else
                {
                    m_UIBulletsArmMonitor.show();
                    m_UIHealthArmMonitor.show();
                    m_UIPlayerHelmet.toggleReloadProgressBar(WeaponManager.IsReloadInProgress);
                    m_UIHealthArmMonitor.updateInterface(PlayerManager.HealthMeter);
                    m_UIBulletsArmMonitor.updateInterface(PlayerManager.HealthMeter);
                }
            }
        }
    }
}