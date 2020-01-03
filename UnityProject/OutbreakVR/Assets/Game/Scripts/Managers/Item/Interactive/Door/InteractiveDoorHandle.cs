using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveDoorHandle : MonoBehaviour, IPointerOver
    {
        /// <summary>
        /// The door that controls the handle
        /// </summary>
        [SerializeField]
        private InteractiveDoor m_InteractiveDoor = null;

        /// <summary>
        /// On pointer enter over the door handle
        /// </summary>
        public void onPointerEnter()
        {
            m_InteractiveDoor.onDoorHandlePointerOver();
        }

        /// <summary>
        /// On pointer exiting the door handle
        /// </summary>
        public void onPointerExit()
        {
            m_InteractiveDoor.onDoorHandlePointerExit();
        }

        /// <summary>
        /// On pointer interacting with the door handle
        /// </summary>
        public void onPointerInteract()
        {
            m_InteractiveDoor.onDoorHandlePointerInteract();
        }
    }
}