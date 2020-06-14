using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveC4SelectLocation : InteractiveSelectiveLocationBase
    {
        public override void onPointerInteract()
        {
            if (isInventoryItemUsed())
            {
                base.onPointerInteract();
            }
        } 
    }
}