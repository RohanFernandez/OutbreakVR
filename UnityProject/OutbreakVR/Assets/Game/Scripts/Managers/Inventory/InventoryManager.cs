using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum INVENTORY_ITEM_ID
    {
        INVENTORY_HELMET        = 0,
        INVENTORY_HEALTH        = 1,
        INVENTORY_POWER_NODE    = 2,
        INVENTORY_C4            = 3,
        INVENTORY_BLOOD_BAGS    = 4,
    }

    public class InventoryManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static InventoryManager s_Instance = null;

        /// <summary>
        /// List of all pre assigned inventory items
        /// </summary>
        [SerializeField]
        private List<InventoryItem> m_lstPreDefinedInventoryItems = null;

        /// <summary>
        /// The dictionary of all inventory items
        /// </summary>
        private Dictionary<INVENTORY_ITEM_ID, InventoryItem> m_dictInventory = null;

        /// <summary>
        /// Initialize on game begin, sets singleton
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            ///Initialize inventory items in the list
            int l_iPredefinedInventoryItemCount = m_lstPreDefinedInventoryItems.Count;
            m_dictInventory = new Dictionary<INVENTORY_ITEM_ID, InventoryItem>(l_iPredefinedInventoryItemCount);

            for (int l_iInventoryIndex = 0; l_iInventoryIndex < l_iPredefinedInventoryItemCount; l_iInventoryIndex++)
            {
                InventoryItem l_CurrentInventoryItem = m_lstPreDefinedInventoryItems[l_iInventoryIndex];
                m_dictInventory.Add(l_CurrentInventoryItem.InventoryID, l_CurrentInventoryItem);
                l_CurrentInventoryItem.initialize();
            }
        }

        /// <summary>
        /// Destroys singleton instance
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }

            ///destroy inventory items in the list
            int l_iPredefinedInventoryItemCount = m_lstPreDefinedInventoryItems.Count;
            
            for (int l_iInventoryIndex = 0; l_iInventoryIndex < l_iPredefinedInventoryItemCount; l_iInventoryIndex++)
            {
                m_lstPreDefinedInventoryItems[l_iInventoryIndex].destroy();
            }

            s_Instance = null;
        }

        /// <summary>
        /// The invnetory drop item
        /// </summary>
        public static bool PickupItem(InventoryDrop a_InventoryDropItem)
        {
            bool l_bIsItemPickedUp = false;

            ITEM_TYPE l_ItemType = a_InventoryDropItem.getItemType();
            INVENTORY_ITEM_ID l_InventoryItemID = a_InventoryDropItem.InventoryID;
            InventoryItem l_InventoryItem = null;

            if (s_Instance.m_dictInventory.TryGetValue(l_InventoryItemID, out l_InventoryItem))
            {
                switch (l_InventoryItemID)
                {
                    case INVENTORY_ITEM_ID.INVENTORY_HELMET:
                        {
                            /// Set helmet data
                            InventoryHelmet l_HelmetInventoryItem = (InventoryHelmet)l_InventoryItem;

                            if (l_HelmetInventoryItem.IsItemInInventory)
                            {
                                ///the new item that will be set as a dropped cracked/uncracked helmet
                                ItemDropBase l_ItemDropBase = null;

                                ///If the player is wearing a helmet that is cracked, then replace the cracked helmet in the inventory with the uncracked pickedup helmet
                                ///and the picked up uncracked helmet with the cracked helmet as an cracked helmet item drop
                                if (l_HelmetInventoryItem.IsHelmetCracked)
                                {
                                    l_ItemDropBase = ItemDropManager.GetItemDrop(ITEM_TYPE.ITEM_CRACKED_HELMET);
                                }
                                /// If the player is wearing a helmet that is not cracked, then replace the helmet with the drop
                                else
                                {
                                    l_ItemDropBase = ItemDropManager.GetItemDrop(ITEM_TYPE.ITEM_HELMET);
                                    HelmetDrop l_HelmetDrop = (HelmetDrop)l_ItemDropBase;
                                    l_HelmetDrop.StrengthPercentage = InventoryHelmet.GetPercentageFromHelmetCondition(l_HelmetInventoryItem.CurrentStrength);
                                }

                                ///Set transform of dropped item as the previous picked up item
                                l_ItemDropBase.transform.SetParent(a_InventoryDropItem.transform.parent);
                                l_ItemDropBase.transform.SetPositionAndRotation(a_InventoryDropItem.transform.position, a_InventoryDropItem.transform.rotation);
                                l_ItemDropBase.transform.localPosition = a_InventoryDropItem.transform.localPosition;
                            }
                            ///Set the picked up helmet as current
                            HelmetDrop l_PickedHelmet = (HelmetDrop)a_InventoryDropItem;
                            l_HelmetInventoryItem.CurrentStrength = InventoryHelmet.GetHelmetStrengthFromPercentage(l_PickedHelmet.StrengthPercentage);
                            l_HelmetInventoryItem.ItemsInInventory = 1;
                            l_bIsItemPickedUp = true;
                            break;
                        }
                    case INVENTORY_ITEM_ID.INVENTORY_HEALTH:
                        {
                            /// Set health data
                            InventoryHealth l_HelmetInventoryItem = (InventoryHealth)l_InventoryItem;
                            PlayerManager.HealthMeter = (PlayerManager.HealthMeter + l_HelmetInventoryItem.getHealthValueWithPickupType(l_ItemType));

                            EventHash l_EventHash = EventManager.GetEventHashtable();
                            l_EventHash.Add(GameEventTypeConst.ID_INVENTORY_TYPE, INVENTORY_ITEM_ID.INVENTORY_HEALTH);
                            l_EventHash.Add(GameEventTypeConst.ID_INVENTORY_ITEM, l_HelmetInventoryItem);
                            EventManager.Dispatch(GAME_EVENT_TYPE.ON_INVENTORY_ITEM_CONSUMED, l_EventHash);

                            l_bIsItemPickedUp = true;
                            break;
                        }
                    case INVENTORY_ITEM_ID.INVENTORY_POWER_NODE:
                        {
                            l_InventoryItem.ItemsInInventory++;
                            l_bIsItemPickedUp = true;
                            break;
                        }
                    case INVENTORY_ITEM_ID.INVENTORY_C4:
                        {
                            l_InventoryItem.ItemsInInventory++;
                            l_bIsItemPickedUp = true;
                            break;
                        }
                    case INVENTORY_ITEM_ID.INVENTORY_BLOOD_BAGS:
                        {
                            l_InventoryItem.ItemsInInventory++;
                            l_bIsItemPickedUp = true;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            else
            {
                Debug.LogError("InventoryManager::PickupItem:: Failed to find item in inventory dict with ID '" + l_InventoryItemID.ToString() + "'");
            }

            return l_bIsItemPickedUp;
        }

        /// <summary>
        /// Sets the item inventory to current
        /// </summary>
        /// <param name="a_SavedItemInventory"></param>
        public static void SetInventoryDataAsCurrent(ItemInventoryStructure a_SavedItemInventory)
        {
            /// Set helmet data
            InventoryItem l_InventoryItem = null;
            if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_HELMET, out l_InventoryItem))
            {;
                InventoryHelmet l_HelmetInventoryItem = (InventoryHelmet)l_InventoryItem;
                l_HelmetInventoryItem.CurrentStrength = a_SavedItemInventory.m_HelmetStructure.m_iHelmetStrength;
                l_HelmetInventoryItem.ItemsInInventory = a_SavedItemInventory.m_HelmetStructure.m_bIsHelmetCarried ? 1 : 0;
            }

            l_InventoryItem = null;
            if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_C4, out l_InventoryItem))
            {
                InventoryC4 l_C4InventoryItem = (InventoryC4)l_InventoryItem;
                l_C4InventoryItem.ItemsInInventory = a_SavedItemInventory.m_iC4Count;
            }

            l_InventoryItem = null;
            if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_POWER_NODE, out l_InventoryItem))
            {
                InventoryPowerNode l_PowerNodeInventoryItem = (InventoryPowerNode)l_InventoryItem;
                l_PowerNodeInventoryItem.ItemsInInventory = a_SavedItemInventory.m_iPowerNodeCount;
            }

            l_InventoryItem = null;
            if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_BLOOD_BAGS, out l_InventoryItem))
            {
                InventoryBloodBags l_BloodBagsInventoryItem = (InventoryBloodBags)l_InventoryItem;
                l_BloodBagsInventoryItem.ItemsInInventory = a_SavedItemInventory.m_iBloodBagCount;
            }
        }

        /// <summary>
        /// Sets the current inventory info to the reference arguement
        /// </summary>
        /// <param name="a_IteminventoryStructure"></param>
        public static void SetInventoryInfo(ref ItemInventoryStructure a_ItemInventoryStructure)
        {
            /// Set helmet data
            InventoryItem l_InventoryItem = null;
            if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_HELMET, out l_InventoryItem))
            {
                InventoryHelmet l_HelmetInventoryItem = (InventoryHelmet)l_InventoryItem;
                a_ItemInventoryStructure.m_HelmetStructure.m_bIsHelmetCarried = l_HelmetInventoryItem.ItemsInInventory > 0;
                a_ItemInventoryStructure.m_HelmetStructure.m_iHelmetStrength = l_HelmetInventoryItem.CurrentStrength;
            }

            /// Set C4 data
            l_InventoryItem = null;
            if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_C4, out l_InventoryItem))
            {
                InventoryC4 l_C4InventoryItem = (InventoryC4)l_InventoryItem;
                a_ItemInventoryStructure.m_iC4Count = l_C4InventoryItem.ItemsInInventory;
            }

            /// Set power node data
            l_InventoryItem = null;
            if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_POWER_NODE, out l_InventoryItem))
            {
                InventoryPowerNode l_PowerNodeInventoryItem = (InventoryPowerNode)l_InventoryItem;
                a_ItemInventoryStructure.m_iPowerNodeCount = l_PowerNodeInventoryItem.ItemsInInventory;
            }

            /// Set blood bag data
            l_InventoryItem = null;
            if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_BLOOD_BAGS, out l_InventoryItem))
            {
                InventoryBloodBags l_BloodBagInventoryItem = (InventoryBloodBags)l_InventoryItem;
                a_ItemInventoryStructure.m_iBloodBagCount = l_BloodBagInventoryItem.ItemsInInventory;
            }
        }

        /// <summary>
        /// Returns an inventory item with the given ID
        /// </summary>
        /// <param name="a_InventoryItemID"></param>
        /// <returns></returns>
        public static InventoryItem GetInventoryItem(INVENTORY_ITEM_ID a_InventoryItemID)
        {
            InventoryItem l_InventoryItem = null;
            s_Instance.m_dictInventory.TryGetValue(a_InventoryItemID, out l_InventoryItem);
            return l_InventoryItem;
        }

        /// <summary>
        /// Uses the inventory item
        /// </summary>
        /// <param name="a_InventoryItemID"></param>
        public static bool IsInventoryItemUsed(INVENTORY_ITEM_ID a_InventoryItemID)
        {
            InventoryItem l_InventoryItem = InventoryManager.GetInventoryItem(a_InventoryItemID);
            if ((l_InventoryItem != null) &&
                (l_InventoryItem.ItemsInInventory > 0))
            {
                l_InventoryItem.ItemsInInventory--;

                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_INVENTORY_TYPE, a_InventoryItemID);
                l_EventHash.Add(GameEventTypeConst.ID_INVENTORY_ITEM, l_InventoryItem);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_INVENTORY_ITEM_CONSUMED, l_EventHash);

                return true;
            }
            return false;
        }
    }
}