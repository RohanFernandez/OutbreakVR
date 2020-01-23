using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveCabinetDrawer : AbsInteractiveDoorBase
    {
        /// <summary>
        /// Can reset the interactable value to default state
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
        }

        public override void Awake()
        {
            if (m_OutlineGroupHighlighterBase != null)
            {
                m_OutlineGroupHighlighterBase.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
            }
        }

        /// <summary>
        /// On interactive pointer entering the door handle
        /// </summary>
        public override void onDoorHandlePointerOver()
        {
            base.onDoorHandlePointerOver();
        }

        /// <summary>
        /// On interactive pointer exiting the door handle
        /// </summary>
        public override void onDoorHandlePointerExit()
        {
            base.onDoorHandlePointerExit();
        }

        /// <summary>
        /// On interactive with the door handle of this door
        /// </summary>
        public override void onDoorHandlePointerInteract()
        {
            base.onDoorHandlePointerInteract();
        }

        /// <summary>
        /// Opens the door depending upon which side of the door the player is at
        /// </summary>
        public override void openDoor()
        {
            base.openDoor();
        }

        /// <summary>
        /// Closes door, calls anim trigger
        /// </summary>
        public override void closeDoor(bool a_bIsReset = false)
        {
            base.closeDoor(a_bIsReset);
        }
    }
}