using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveBloodBagsSelectLocation : InteractiveSelectiveLocationBase
    {
        [SerializeField]
        private string m_strObjectiveTrigger = string.Empty;

        public override void onPointerInteract()
        {
            if (isInventoryItemUsed())
            {
                base.onPointerInteract();
                if (!string.IsNullOrEmpty(m_strObjectiveTrigger))
                {
                    ObjectiveManager.TriggerObjective(m_strObjectiveTrigger);
                }
            }
        }
    }
}