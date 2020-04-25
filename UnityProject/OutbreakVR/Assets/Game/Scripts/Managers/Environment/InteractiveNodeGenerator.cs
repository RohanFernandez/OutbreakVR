using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveNodeGenerator : OnInteractEnvironmentTriggerEvent
    {
        public override void onPointerInteract()
        {
            if (InventoryManager.IsInventoryItemUsed(INVENTORY_ITEM_ID.INVENTORY_POWER_NODE))
            {
                base.onPointerInteract();
            }
        }
    }
}