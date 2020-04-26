using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class InteractiveSelectiveLocationBase : OnInteractEnvironmentTriggerEvent
    {
        [SerializeField]
        protected Collider m_Collider = null;

        [SerializeField]
        protected INVENTORY_ITEM_ID m_InventoryItemIDToPlace;

        public override void onPointerInteract()
        {
            if (InventoryManager.IsInventoryItemUsed(m_InventoryItemIDToPlace))
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