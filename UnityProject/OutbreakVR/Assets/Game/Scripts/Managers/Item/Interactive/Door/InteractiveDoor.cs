using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveDoor : MonoBehaviour
    {
        /// <summary>
        /// Anim trigger to open the door on side 1
        /// </summary>
        private const string ANIM_TRIGGER_DOOR_OPEN_SIDE_1 = "DoorOpeningSide1";

        /// <summary>
        /// Anim trigger to open the door on side 2
        /// </summary>
        private const string ANIM_TRIGGER_DOOR_OPEN_SIDE_2 = "DoorOpeningSide2";

        /// <summary>
        /// Anim trigger to close the door on any side
        /// </summary>
        private const string ANIM_TRIGGER_DOOR_OPEN_CLOSE = "DoorClose";

        /// <summary>
        /// The handle of the door
        /// </summary>
        [SerializeField]
        private InteractiveDoorHandle m_InteractiveDoorHandle = null;

        /// <summary>
        /// The animator controller to open, close the door side
        /// </summary>
        [SerializeField]
        private Animator m_animatorDoorControl = null;

        /// <summary>
        /// The go on Side 1 of the door
        /// </summary>
        [SerializeField]
        private GameObject m_goDoorSide1 = null;

        /// <summary>
        /// The go on Side 2 of the door
        /// </summary>
        [SerializeField]
        private GameObject m_goDoorSide2 = null;

        /// <summary>
        /// The collider that maintains the proxiity of the door with the player
        /// </summary>
        [SerializeField]
        private Collider m_colDoorProximity = null;

        /// <summary>
        /// The audio src that plays the door open and close
        /// </summary>
        [SerializeField]
        private UnpooledAudioSource m_UnpooledAudSrc = null;

        /// <summary>
        /// Is the door currently open
        /// </summary>
        [SerializeField]
        private bool m_bIsDoorOpen = false;
        public bool IsDoorOpen
        {
            get { return m_bIsDoorOpen; }
            set { m_bIsDoorOpen = value; }
        }

        /// <summary>
        /// The outline hightlighter
        /// </summary>
        [SerializeField]
        private OutlineHighlighterBase m_OutlineGroupHighlighterBase = null;

        /// <summary>
        /// The normal outline color
        /// </summary>
        [SerializeField]
        private Color m_colorOutlineNormal;

        /// <summary>
        /// The highlighted outline color
        /// </summary>
        [SerializeField]
        private Color m_colorOutlineHighlighted;

        /// <summary>
        /// On interactive pointer entering the door handle
        /// </summary>
        public void onDoorHandlePointerOver()
        {
            if (m_OutlineGroupHighlighterBase != null)
            {
                m_OutlineGroupHighlighterBase.toggleHighlighter(true, m_colorOutlineHighlighted);
            }
        }

        /// <summary>
        /// On interactive pointer exiting the door handle
        /// </summary>
        public void onDoorHandlePointerExit()
        {
            if (m_OutlineGroupHighlighterBase != null)
            {
                m_OutlineGroupHighlighterBase.toggleHighlighter(true, m_colorOutlineNormal);
            }
        }

        /// <summary>
        /// On interactive with the door handle of this door
        /// </summary>
        public void onDoorHandlePointerInteract()
        {
            if (IsDoorOpen)
            {
                return;
            }
            IsDoorOpen = true;
            openDoor();
        }

        /// <summary>
        /// Opens the door depending upon which side of the door the player is at
        /// </summary>
        private void openDoor()
        {
            Vector3 l_v3PlayerPos = PlayerManager.GetPosition();
            Vector3 l_v3PlayerToDoorDirection = (m_InteractiveDoorHandle.transform.position - l_v3PlayerPos).normalized;

            ///Is player facing the door so the door has to be opened in side 1, else its in side 2
            bool l_bIsPlayerInSide1 = Vector3.Dot(l_v3PlayerToDoorDirection, (m_InteractiveDoorHandle.transform.position - m_goDoorSide1.transform.position).normalized) > 0.3f;

            if (l_bIsPlayerInSide1)
            {
                m_animatorDoorControl.SetTrigger(ANIM_TRIGGER_DOOR_OPEN_SIDE_1);
            }
            else
            {
                m_animatorDoorControl.SetTrigger(ANIM_TRIGGER_DOOR_OPEN_SIDE_2);
            }

            m_UnpooledAudSrc.play(GameConsts.AUD_CLIP_DOOR_OPEN, false, 1.0f);
        }

        /// <summary>
        /// Closes door, calls anim trigger
        /// </summary>
        public void closeDoor()
        {
            if (IsDoorOpen)
            {
                IsDoorOpen = false;
                m_animatorDoorControl.SetTrigger(ANIM_TRIGGER_DOOR_OPEN_CLOSE);
                m_UnpooledAudSrc.play(GameConsts.AUD_CLIP_DOOR_CLOSE, false, 1.0f);
            }
        }
    }
}
