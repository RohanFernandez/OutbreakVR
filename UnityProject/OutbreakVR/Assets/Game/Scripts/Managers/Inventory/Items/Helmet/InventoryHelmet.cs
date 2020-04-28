using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    /// <summary>
    /// The enum that indicates the strength of the helmet, its based on the percentage of the strength
    /// </summary>
    public enum HelmetStrengthIndicator
    { 
        HELMET_STRENGTH_WEAK        = 0,
        HELMET_STRENGTH_MODERATE    = 1,
        HELMET_STRENGTH_STRONG      = 2,
    }

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
        /// The current strength of the helmet
        /// </summary>
        [SerializeField]
        private int m_iCurrentStrength = 40;
        public int CurrentStrength
        {
            get { return m_iCurrentStrength; }
            set
            {
                int l_iStrengthBeforeUpdate = m_iCurrentStrength;
                m_iCurrentStrength = (int)Mathf.Clamp(value, 0.0f, MAX_HELMET_STRENGTH);
                if (l_iStrengthBeforeUpdate != m_iCurrentStrength &&
                m_iCurrentStrength == 0 &&
                IsItemInInventory)
                {
                    onPlayerHelmetCracked();
                }
            }
    }

        /// <summary>
        /// THe max strength any helmet can have
        /// </summary>
        public const float MAX_HELMET_STRENGTH = 40;

        /// <summary>
        /// gets if the helmet is cracked
        /// </summary>
        public bool IsHelmetCracked
        {
            get { return CurrentStrength == 0; }
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
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_DAMAGE_INFLICTED_ON_PLAYER, onDamageInflictedToPlayer);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_CONTROLLER_CHANGED, onControllerChanged);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_ITEM_PICKED_UP_CONSUMED, onPickedUpItemConsumed);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_INVENTORY_ITEM_CONSUMED, onInvntoryItemConsumed);
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
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_DAMAGE_INFLICTED_ON_PLAYER, onDamageInflictedToPlayer);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_CONTROLLER_CHANGED, onControllerChanged);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_ITEM_PICKED_UP_CONSUMED, onPickedUpItemConsumed);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_INVENTORY_ITEM_CONSUMED, onInvntoryItemConsumed);
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

                Transform l_BulletsArmMonitorParent = l_WeaponBase.ArmMonitorParent_BulletCount;
                m_UIBulletsArmMonitor.transform.SetParent(l_BulletsArmMonitorParent.transform);
                m_UIBulletsArmMonitor.transform.localPosition = Vector3.zero;
                m_UIBulletsArmMonitor.transform.localRotation = Quaternion.identity;

                setArmUIScaleOnControllerType();
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
        /// on picked up drop item is consumed and the item is a helmet then update data
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onPickedUpItemConsumed(EventHash a_EventHash)
        {
            ITEM_TYPE l_ItemType = (ITEM_TYPE)a_EventHash[GameEventTypeConst.ID_ITEM_DROP_TYPE];
            if (l_ItemType == ITEM_TYPE.ITEM_HELMET ||
                l_ItemType == ITEM_TYPE.ITEM_C4 ||
                l_ItemType == ITEM_TYPE.ITEM_BLOOD_BAGS ||
                l_ItemType == ITEM_TYPE.ITEM_POWER_NODE)
            { 
                updateHelmet();
            }
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
        /// Callback called on inventory item consumed
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onInvntoryItemConsumed(EventHash a_EventHash)
        {
            updateHelmet();
        }

        /// <summary>
        /// Callback on damage inflicted on the player by an enemy
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onDamageInflictedToPlayer(EventHash a_EventHash)
        {
            int l_iDamageInflicted = (int)a_EventHash[GameEventTypeConst.ID_DAMAGE_INFLICTED];
            CurrentStrength -= l_iDamageInflicted;
            updateHelmet();
        }

        /// <summary>
        /// Callback on controller hand changed
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onControllerChanged(EventHash a_EventHash)
        {
            setArmUIScaleOnControllerType();
        }


        /// <summary>
        /// Sets the arm monitor UI's scale 
        /// If left hand then scale the x of the UI's on the arm to -1, else keep it as 1
        /// This is because on the arm is scaled.x to -1 to imitate the gun on the left hand, but this inverts the UI's on the arm as well, to prevent that we do this
        /// </summary>
        private void setArmUIScaleOnControllerType()
        {
            CONTROLLER_TYPE l_ControllerType = ControllerManager.GetActiveControllerType();
            Vector3 l_v3NewUIScale = l_ControllerType == CONTROLLER_TYPE.CONTROLLER_LEFT_REMOTE ? new Vector3(-1.0f, 1.0f, 1.0f) : Vector3.one;
            m_UIHealthArmMonitor.transform.localScale = l_v3NewUIScale;
            m_UIBulletsArmMonitor.transform.localScale = l_v3NewUIScale;
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
                    m_UIPlayerHelmet.toggleHelmetScreen(IsHelmetCracked, HelmetStrengthIndicator.HELMET_STRENGTH_WEAK);
                }
                else
                {
                    m_UIBulletsArmMonitor.show();
                    m_UIHealthArmMonitor.show();
                    m_UIPlayerHelmet.toggleReloadProgressBar(WeaponManager.IsReloadInProgress);
                    m_UIHealthArmMonitor.updateInterface(PlayerManager.HealthMeter);
                    m_UIBulletsArmMonitor.updateInterface();

                    int l_iBloodBagCount = InventoryManager.GetInventoryItem(INVENTORY_ITEM_ID.INVENTORY_BLOOD_BAGS).ItemsInInventory;
                    int l_iC4Count = InventoryManager.GetInventoryItem(INVENTORY_ITEM_ID.INVENTORY_C4).ItemsInInventory;
                    int l_iPowerNodeCount = InventoryManager.GetInventoryItem(INVENTORY_ITEM_ID.INVENTORY_POWER_NODE).ItemsInInventory;

                    m_UIPlayerHelmet.toggleHelmetScreen(IsHelmetCracked, GetHelmetStrengthIndicatorFromPercentage(GetPercentageFromHelmetCondition(CurrentStrength)), l_iPowerNodeCount, l_iBloodBagCount, l_iC4Count);
                }
            }
        }

        /// <summary>
        /// Gets the condition of the helmet between 0 - MAX_HELMET_STRENGTH from a percentage between 0 - 100 
        /// </summary>
        /// <param name="a_iHelmetConditionPercentage"></param>
        /// <returns></returns>
        public static int GetHelmetStrengthFromPercentage(int a_iHelmetConditionPercentage)
        {
            return (int)(Mathf.Clamp(a_iHelmetConditionPercentage, 0, 100) * 0.01 * MAX_HELMET_STRENGTH);
        }

        /// <summary>
        /// Gets the percentage of the helmet between 0 - 100 from a percentage between 0 - MAX_HELMET_STRENGTH 
        /// </summary>
        /// <param name="a_iHelmetCondition"></param>
        /// <returns></returns>
        public static int GetPercentageFromHelmetCondition(int a_iHelmetCondition)
        {
            return (int)((Mathf.Clamp(a_iHelmetCondition, 0.0f, MAX_HELMET_STRENGTH) / MAX_HELMET_STRENGTH) * 100.0f);
        }

        /// <summary>
        /// Returns the Helmet strength indicator based on percentage min/max values
        /// </summary>
        /// <param name="a_iPercentage"></param>
        /// <returns></returns>
        public static HelmetStrengthIndicator GetHelmetStrengthIndicatorFromPercentage(int a_iPercentage)
        {
            if (a_iPercentage < 33) 
            { 
                return HelmetStrengthIndicator.HELMET_STRENGTH_WEAK; 
            }
            else if (a_iPercentage > 66) 
            {
                return HelmetStrengthIndicator.HELMET_STRENGTH_STRONG; 
            }
            else {
                return HelmetStrengthIndicator.HELMET_STRENGTH_MODERATE; 
            }
        }
    }
}