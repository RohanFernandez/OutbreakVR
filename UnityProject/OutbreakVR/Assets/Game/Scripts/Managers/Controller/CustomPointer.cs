using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class CustomPointer : OVRCursor
    {
        /// <summary>
        /// Is target hit with the cursor
        /// </summary>
        [SerializeField]
        private bool m_bIsTargetHit = false;

        /// <summary>
        /// The headset cursor
        /// </summary>
        [SerializeField]
        private GameObject m_HeadsetCursor = null;

        /// <summary>
        /// The laser pointer
        /// </summary>
        [SerializeField]
        private LineRenderer m_LineRenderer = null;

        private Vector3 m_v3LaserStartPosition = Vector3.zero;
        private Vector3 m_v3LaserEndPosition = Vector3.zero;
        private Vector3 m_v3PointerForward = Vector3.zero;

        public override void SetCursorRay(Transform a_Ray)
        {
            m_v3LaserStartPosition = a_Ray.position;
            m_v3PointerForward = a_Ray.forward;
            m_bIsTargetHit = false;
        }

        public override void SetCursorStartDest(Vector3 start, Vector3 dest, Vector3 normal)
        {
            m_v3LaserEndPosition = dest;
            m_bIsTargetHit = true;
        }

        private void LateUpdate()
        {
            if (ControllerManager.IsLaserActive)
            {
                updateCursorPosition();
            }
            else
            {
                m_LineRenderer.enabled = false;
                m_HeadsetCursor.SetActive(false);
            }
        }

        /// <summary>
        /// Updates cursor state
        /// If target is hit then set the end position to the hit position else set the max position
        /// If target is hit and laser is on set cursor at laser hit position
        /// </summary>
        private void updateCursorPosition()
        {
            if (!m_bIsTargetHit)
            {
                m_v3LaserEndPosition = m_v3LaserStartPosition + m_v3PointerForward * ControllerManager.MAX_CURSOR_DISTANCE;
            }

            if (ControllerManager.IsRemoteAttached)
            {
                m_HeadsetCursor.SetActive(m_bIsTargetHit);
                m_HeadsetCursor.transform.position = m_v3LaserEndPosition;

                if (ControllerManager.IsLineRendererOn)
                {
                    m_LineRenderer.enabled = true;
                    m_LineRenderer.SetPosition(0, m_v3LaserStartPosition);
                    m_LineRenderer.SetPosition(1, m_v3LaserEndPosition);
                }
            }
            else
            {
                m_LineRenderer.enabled = false;
                m_HeadsetCursor.SetActive(true);
                m_HeadsetCursor.transform.position = m_v3LaserEndPosition;
            }
        }
    }
}