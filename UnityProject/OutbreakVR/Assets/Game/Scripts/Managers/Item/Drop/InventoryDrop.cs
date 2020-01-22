using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InventoryDrop : ItemDropBase
    {
        public override ITEM_CATEGORY getItemCategoryType()
        {
            return ITEM_CATEGORY.INVENTORY;
        }
    }
}