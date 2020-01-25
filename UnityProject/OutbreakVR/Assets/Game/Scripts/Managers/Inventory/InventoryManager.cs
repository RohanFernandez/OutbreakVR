using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum INVENTORY_ITEM_ID
    {
        INVENTORY_HELMET = 0,

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

            switch (l_ItemType)
            {
                case ITEM_TYPE.ITEM_HELMET:
                    {
                        /// Set helmet data
                        InventoryHelmet l_HelmetInventoryItem = null;
                        InventoryItem l_InventoryItem = null;
                        if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_HELMET, out l_InventoryItem))
                        {
                            l_HelmetInventoryItem = (InventoryHelmet)l_InventoryItem;
                            HelmetDrop l_HelmetDrop = (HelmetDrop)a_InventoryDropItem;
                            l_HelmetInventoryItem.IsHelmetCracked = l_HelmetDrop.IsHelmetCracked;
                            l_HelmetInventoryItem.ItemsInInventory = 1;
                            l_bIsItemPickedUp = true;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return l_bIsItemPickedUp;
        }

        /// <summary>
        /// Sets the item inventory to current
        /// </summary>
        /// <param name="a_SavedItemInventory"></param>
        public static void SetCurrentItemInventory(ItemInventoryStructure a_SavedItemInventory)
        {
            /// Set helmet data
            InventoryHelmet l_HelmetInventoryItem = null;
            InventoryItem l_InventoryItem = null;
            if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_HELMET, out l_InventoryItem))
            {
                l_HelmetInventoryItem = (InventoryHelmet)l_InventoryItem;
                l_HelmetInventoryItem.IsHelmetCracked = a_SavedItemInventory.m_HelmetStructure.m_bIsHelmetCracked;
                l_HelmetInventoryItem.ItemsInInventory = a_SavedItemInventory.m_HelmetStructure.m_bIsHelmetCarried ? 1 : 0;
            }
        }

        /// <summary>
        /// Sets the current inventory info to the reference arguement
        /// </summary>
        /// <param name="a_IteminventoryStructure"></param>
        public static void RetrieveInventoryInfo(ref ItemInventoryStructure a_ItemInventoryStructure)
        {
            /// Set helmet data
            
            InventoryItem l_InventoryItem = null;
            if (s_Instance.m_dictInventory.TryGetValue(INVENTORY_ITEM_ID.INVENTORY_HELMET, out l_InventoryItem))
            {
                InventoryHelmet l_HelmetInventoryItem = (InventoryHelmet)l_InventoryItem;
                a_ItemInventoryStructure.m_HelmetStructure.m_bIsHelmetCarried = l_HelmetInventoryItem.ItemsInInventory > 0;
                a_ItemInventoryStructure.m_HelmetStructure.m_bIsHelmetCracked = l_HelmetInventoryItem.IsHelmetCracked;
            }
        }
    }
}