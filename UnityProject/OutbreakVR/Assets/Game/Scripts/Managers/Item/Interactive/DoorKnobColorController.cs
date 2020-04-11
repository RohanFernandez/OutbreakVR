using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class DoorKnobColorController : MonoBehaviour
    {
        [SerializeField]
        private List<SpriteRenderer> m_lstDoorKnob = null;

        /// <summary>
        /// Sets the color of the door knob
        /// </summary>
        /// <param name="a_Color"></param>
        public void setColor(Color a_Color)
        {
            int l_iDoorKnobCount = m_lstDoorKnob.Count;
            for (int l_iDoorKnobIndex = 0; l_iDoorKnobIndex < l_iDoorKnobCount; l_iDoorKnobIndex++)
            {
                m_lstDoorKnob[l_iDoorKnobIndex].color = a_Color;
            }
        }
    }
}