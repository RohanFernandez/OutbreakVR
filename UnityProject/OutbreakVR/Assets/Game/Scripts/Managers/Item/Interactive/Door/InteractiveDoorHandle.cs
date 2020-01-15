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
        /// The collider of the door handle
        /// </summary>
        [SerializeField]
        private Collider m_colInteractive = null;

        /// <summary>
        /// Activates/ Deactivates the door handle for interaction
        /// </summary>
        /// <param name="a_bIsActivated"></param>
        public void toggleDoorHandleInteraction(bool a_bIsActivated)
        {
            m_colInteractive.enabled = a_bIsActivated;
        }

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