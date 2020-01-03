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
        /// Close door on player trigger exit
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerExit(Collider a_Other)
        {
            if (a_Other.tag.Equals(GameConsts.TAG_PLAYER, System.StringComparison.OrdinalIgnoreCase))
            {
                m_InteractiveDoor.closeDoor();
            }
        }
    }
}