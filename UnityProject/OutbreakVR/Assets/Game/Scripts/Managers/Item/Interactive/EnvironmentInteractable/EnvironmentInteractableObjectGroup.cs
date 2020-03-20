using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnvironmentInteractableObjectGroup : AbsEnvironmentInteractableObject
    {
        /// <summary>
        /// The list of interactable objects in this group
        /// </summary>
        [SerializeField]
        private List<AbsEnvironmentInteractableObject> m_lstInteractableObjects = null;

        /// <summary>
        /// Can reset the interactable value to default state
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
            int l_iInteractableCount = m_lstInteractableObjects.Count;
            for (int l_iInteractableIndex = 0; l_iInteractableIndex < l_iInteractableCount; l_iInteractableIndex++)
            {
                m_lstInteractableObjects[l_iInteractableIndex].resetValues();
            }
        }
    }
}