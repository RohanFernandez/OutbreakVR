using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveDoorProximityDetector : MonoBehaviour
    {
        /// <summary>
        /// The interactive door to which its a proximity detector to
        /// </summary>
        [SerializeField]
        private InteractiveDoor m_InteractiveDoor = null;

        /// <summary>
        /// The layer mask that registers the collision on enter
        /// </summary>
        [SerializeField]
        private LayerMask m_LayerMaskCollision;

        /// <summary>
        /// The list of all gameobjects currently in trigger
        /// </summary>
        [SerializeField]
        private List<Transform> m_lstDoorTriggers = null;

        /// <summary>
        /// Close door on player trigger exit
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerEnter(Collider a_Other)
        {
            if (GeneralUtils.IsLayerInLayerMask(m_LayerMaskCollision, a_Other.gameObject.layer))
            {
                m_lstDoorTriggers.Add(a_Other.transform);
            }
        }

        /// <summary>
        /// Close door on player trigger exit
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerExit(Collider a_Other)
        {
            if (GeneralUtils.IsLayerInLayerMask(m_LayerMaskCollision, a_Other.gameObject.layer))
            {
                m_lstDoorTriggers.Remove(a_Other.transform);
                if (m_lstDoorTriggers.Count == 0) { m_InteractiveDoor.closeDoor(); }
            }
        }

        /// <summary>
        /// Resets so that the list of triggers is empty
        /// </summary>
        public void resetValues()
        {
            m_lstDoorTriggers.Clear();
        }
    }
}