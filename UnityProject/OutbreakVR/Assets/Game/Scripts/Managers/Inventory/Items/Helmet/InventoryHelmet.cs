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
        /// UI player helmet that displays the reloading sign and various screen effects
        /// </summary>
        [SerializeField]
        private UI_ArmMonitor m_UIArmMonitor = null;

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
        }

        public override void destroy()
        {
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onCurrentWeaponChanged);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, onPlayerStateChanged);
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
                Transform l_ArmMonitorParent = l_WeaponBase.ArmMonitorParent;
                m_UIArmMonitor.transform.SetParent(l_ArmMonitorParent.transform);
                m_UIArmMonitor.transform.localPosition = Vector3.zero;
                m_UIArmMonitor.transform.localRotation = Quaternion.identity;
                m_UIArmMonitor.transform.localScale = Vector3.one;
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
        /// Sets if the helmet should be enabled/ disabled
        /// Sets the current weapons bullet count, weapon type
        /// Displays reloading if helmet not cracked
        /// </summary>
        private void updateHelmet()
        {
            PLAYER_STATE l_CurrentPlayerState = PlayerManager.PlayerState;

            if (l_CurrentPlayerState == PLAYER_STATE.MENU_SELECTION ||
                l_CurrentPlayerState == PLAYER_STATE.NO_INTERACTION ||
                !IsItemInInventory)
            {
                m_UIPlayerHelmet.hide();
                m_UIArmMonitor.hide();
            }
            else
            {
                m_UIPlayerHelmet.show();
                if (IsHelmetCracked)
                {
                    //display cracked
                    m_UIArmMonitor.hide();
                }
                else
                {
                    m_UIArmMonitor.show();
                }
            }
        }
    }
}