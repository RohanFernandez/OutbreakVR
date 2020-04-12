using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class OrderedObjectiveGroup : ObjectiveGroupBase
    {
        /// <summary>
        /// The current objective index in the ordererd list
        /// </summary>
        [SerializeField]
        private int m_iCurrentObjectiveIndex = 0;

        /// <summary>
        /// Initializes the current objective index to 0.
        /// </summary>
        /// <param name="a_strID"></param>
        /// <param name="a_strType"></param>
        public override void onInitialize(string a_strID, string a_strType)
        {
            base.onInitialize(a_strID, a_strType);
            m_iCurrentObjectiveIndex = 0;
        }

        /// <summary>
        /// checks if the objective in the current index is complete
        /// Will only check the objective in hte current index is complete
        /// </summary>
        /// <param name="a_Hashtable"></param>
        public override void checkForObjectiveCompletion(Hashtable a_Hashtable)
        {
            base.checkForObjectiveCompletion(a_Hashtable);
            if (m_iCurrentObjectiveIndex >= m_iTotalObjectiveCount)
            {
                Debug.LogError("OrdererdObjectiveGroup::checkForObjectiveCompletion:: current objective index is greater than total objectives in the group.");
                return;
            }

            m_lstObjectives[m_iCurrentObjectiveIndex].checkObjectiveCompletion(a_Hashtable);
            if (m_lstObjectives[m_iCurrentObjectiveIndex].isComplete() ||
                !m_lstObjectives[m_iCurrentObjectiveIndex].isCompulsory())
            {
                m_iCurrentObjectiveIndex++;
                if (isObjectiveGroupComplete())
                {
                    onComplete();
                }
            }
        }
    }
}