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
        private bool m_bIsTargetHit = false;

        /// <summary>
        /// The headset cursor
        /// </summary>
        [SerializeField]
        private GameObject m_HeadsetCursor = null;

        /// <summary>
        /// The mesh renderer UI
        /// </summary>
        [SerializeField]
        private MeshRenderer m_meshrendUICursor = null;

        /// <summary>
        /// The crosshair sprite renderer
        /// </summary>
        [SerializeField]
        private SpriteRenderer m_spriterendCrosshair = null;

        /// <summary>
        /// The laser pointer
        /// </summary>
        [SerializeField]
        private LineRenderer m_LineRenderer = null;

        private Vector3 m_v3LaserStartPosition = Vector3.zero;
        public Vector3 v3LaserStartPosition
        {
            get { return m_v3LaserStartPosition; }
        }

        private Vector3 m_v3LaserEndPosition = Vector3.zero;
        public Vector3 v3LaserEndPosition
        {
            get { return m_v3LaserEndPosition; }
        }

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
            m_v3LaserEndPosition = Vector3.Distance(m_v3LaserStartPosition, m_v3LaserEndPosition) > ControllerManager.MaxCursorDistance || !m_bIsTargetHit ? m_v3LaserStartPosition + m_v3PointerForward * ControllerManager.MaxCursorDistance : m_v3LaserEndPosition;

            //if (!m_bIsTargetHit)
            //{
            //    
            //}

            m_LineRenderer.enabled = ControllerManager.IsLineRendererOn;
            if (m_LineRenderer.enabled)
            {
                m_LineRenderer.SetPosition(0, m_v3LaserStartPosition);
                m_LineRenderer.SetPosition(1, m_v3LaserEndPosition);
            }

            m_HeadsetCursor.transform.position = m_v3LaserEndPosition;
            m_HeadsetCursor.SetActive(true);
        }

        /// <summary>
        /// If true enables the crosshair sprite renderer and disables the pointer mesh renderer
        /// </summary>
        /// <param name="a_bIsCrossHair"></param>
        public void setPointerAsCrosshair(bool a_bIsCrossHair)
        {
            m_meshrendUICursor.enabled = !a_bIsCrossHair;
            m_spriterendCrosshair.enabled = a_bIsCrossHair;
        }
    }
}