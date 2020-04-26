using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveNodeGenerator : OnInteractEnvironmentTriggerEvent
    {
        [SerializeField]
        private Collider m_Collider = null;

        public override void onPointerInteract()
        {
            if (InventoryManager.IsInventoryItemUsed(INVENTORY_ITEM_ID.INVENTORY_POWER_NODE))
            {
                base.onPointerInteract();
                m_Collider.enabled = false;
            }
        }

        /// <summary>
        /// Can reset the interactable value to default state
        /// </summary>
        public override void resetValues()
        {
            m_Collider.enabled = true;
        }
    }
}