using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnvironmentSmashableObjectGroup : AbsEnvironmentInteractableObject, ISmashable
    {
        /// <summary>
        /// The list of interactable objects in this group
        /// </summary>
        [SerializeField]
        private List<SmashableBase> m_lstSmashableObjects = null;

        /// <summary>
        /// Can reset the interactable value to default state
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
            int l_iInteractableCount = m_lstSmashableObjects.Count;
            for (int l_iInteractableIndex = 0; l_iInteractableIndex < l_iInteractableCount; l_iInteractableIndex++)
            {
                m_lstSmashableObjects[l_iInteractableIndex].resetValues();
            }
        }

        public void smash()
        {
            base.resetValues();
            int l_iInteractableCount = m_lstSmashableObjects.Count;
            for (int l_iInteractableIndex = 0; l_iInteractableIndex < l_iInteractableCount; l_iInteractableIndex++)
            {
                m_lstSmashableObjects[l_iInteractableIndex].smash();
            }
        }
    }
}
