using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InventoryDrop : ItemDropBase
    {
        /// <summary>
        /// The inventory id of this item as managed by the inventory manager
        /// </summary>
        [SerializeField]
        private INVENTORY_ITEM_ID m_InventoryID;
        public INVENTORY_ITEM_ID InventoryID
        {
            get { return m_InventoryID; }
        }

        public override ITEM_CATEGORY getItemCategoryType()
        {
            return ITEM_CATEGORY.INVENTORY;
        }
    }
}