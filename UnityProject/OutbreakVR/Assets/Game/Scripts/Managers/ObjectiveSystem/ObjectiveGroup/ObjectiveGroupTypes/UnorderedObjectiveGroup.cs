using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UnorderedObjectiveGroup : ObjectiveGroupBase
    {
        /// <summary>
        /// checks if any objective in the group is complete
        /// </summary>
        /// <param name="a_Hashtable"></param>
        public override void checkForObjectiveCompletion(Hashtable a_Hashtable)
        {
            base.checkForObjectiveCompletion(a_Hashtable);
            for (int l_iObjIndex = 0; l_iObjIndex < m_iTotalObjectiveCount; l_iObjIndex++)
            {
                if (!m_lstObjectives[l_iObjIndex].isComplete())
                {
                    m_lstObjectives[l_iObjIndex].checkObjectiveCompletion(a_Hashtable);
                }
            }

            if (isObjectiveGroupComplete())
            {
                onComplete();
            }
        }
    }
}