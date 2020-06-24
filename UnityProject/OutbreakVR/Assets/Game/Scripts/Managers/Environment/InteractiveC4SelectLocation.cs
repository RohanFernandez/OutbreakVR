using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveC4SelectLocation : InteractiveSelectiveLocationBase
    {
        [SerializeField]
        private GameObject m_goTransparentMesh = null;

        [SerializeField]
        private GameObject m_goOpaqueMesh = null;

        public override void resetValues()
        {
            base.resetValues();
            m_goTransparentMesh.SetActive(true);
            m_goOpaqueMesh.SetActive(false);
        }

        public override void onPointerInteract()
        {
            if (isInventoryItemUsed())
            {
                base.onPointerInteract();
                m_goTransparentMesh.SetActive(false);
                m_goOpaqueMesh.SetActive(true);
            }
        } 
    }
}