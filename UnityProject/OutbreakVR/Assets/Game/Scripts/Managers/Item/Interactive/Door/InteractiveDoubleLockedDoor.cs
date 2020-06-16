using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveDoubleLockedDoor : InteractiveLockedDoor
    {
        /// <summary>
        /// Thge left / right part of the double door which is not this interactive locked door
        /// </summary>
        [SerializeField]
        private InteractiveDoor m_OtherSidedDoor = null;

        protected override void toggleDoorLock(bool a_bIsLocked)
        {
            base.toggleDoorLock(a_bIsLocked);
            m_OtherSidedDoor.lockDoor(a_bIsLocked);
        }
    }
}