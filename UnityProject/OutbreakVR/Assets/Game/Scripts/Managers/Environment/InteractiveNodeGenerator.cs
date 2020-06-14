using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveNodeGenerator : InteractiveSelectiveLocationBase
    {
        /// <summary>
        /// The power cell that will be visable on/off
        /// </summary>
        [SerializeField]
        private GameObject m_PowerCell = null;

        /// <summary>
        /// The material on the light when the light is on
        /// </summary>
        [SerializeField]
        private Material m_matLightOn = null;

        /// <summary>
        /// The material on the light when the light is off
        /// </summary>
        [SerializeField]
        private Material m_matLightOff = null;

        [SerializeField]
        private MeshRenderer m_GreenLightIndicator = null;

        [SerializeField]
        private MeshRenderer m_RedLightIndicator = null;

        /// <summary>
        /// Can reset the interactable value to default state
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
            m_PowerCell.SetActive(false);

            m_GreenLightIndicator.material = m_matLightOff;
            m_RedLightIndicator.material = m_matLightOn;
        }

        public override void onPointerInteract()
        {
            if (isInventoryItemUsed())
            {
                base.onPointerInteract();
                m_PowerCell.SetActive(true);

                m_GreenLightIndicator.material = m_matLightOn;
                m_RedLightIndicator.material = m_matLightOff;
            }
        }
    }
}