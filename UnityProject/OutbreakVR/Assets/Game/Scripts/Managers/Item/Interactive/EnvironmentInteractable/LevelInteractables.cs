using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class LevelInteractables
    {
        /// <summary>
        /// The level name these interactables are located at
        /// </summary>
        [SerializeField]
        private string m_strLevelName = string.Empty;
        public string LevelName
        {
            get { return m_strLevelName; }
        }

        [SerializeField]
        private List<AbsEnvironmentInteractableObject> m_lstInteractables = null;

        /// <summary>
        /// Resets all the values of the interactable that is in the list
        /// </summary>
        public void resetValues()
        {
            int l_iInteractableCount = m_lstInteractables.Count;
            for (int l_iInteractableIndex = 0; l_iInteractableIndex < l_iInteractableCount; l_iInteractableIndex++)
            {
                AbsEnvironmentInteractableObject l_AbsInteractable = m_lstInteractables[l_iInteractableIndex];
                if (l_AbsInteractable != null)
                {
                    l_AbsInteractable.resetValues();
                }
            }
        }
    }
}
