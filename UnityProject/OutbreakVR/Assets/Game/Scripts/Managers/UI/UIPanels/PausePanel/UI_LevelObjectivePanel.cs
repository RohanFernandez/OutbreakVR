using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_LevelObjectivePanel : MonoBehaviour
    {
        /// <summary>
        /// List of all objective panels
        /// </summary>
        [SerializeField]
        private List<PanelObjective> m_lstPanelObjectives = null;

        /// <summary>
        /// Resets the objectives in the panel as per the current level
        /// </summary>
        public void refreshObjectives(ObjectiveGroupBase a_CurrentLevelObjectiveGroup)
        {
            int l_iPanelObjectiveCount = m_lstPanelObjectives.Count;
            int l_ObjectiveGroupCount = a_CurrentLevelObjectiveGroup.m_lstObjectives.Count;
            for (int l_iObjectiveIndex = 0; l_iObjectiveIndex < l_iPanelObjectiveCount; l_iObjectiveIndex++)
            {
                PanelObjective l_PanelObjective = m_lstPanelObjectives[l_iObjectiveIndex];

                // objectives are less than panels then disable the empty panels
                if (l_iObjectiveIndex >= l_ObjectiveGroupCount)
                {
                    l_PanelObjective.gameObject.SetActive(false);
                }
                else
                {
                    ObjectiveBase l_ObjectiveBase = a_CurrentLevelObjectiveGroup.m_lstObjectives[l_iObjectiveIndex];
                    l_PanelObjective.gameObject.SetActive(true);
                    l_PanelObjective.updateText(l_ObjectiveBase.ObjDescription, l_ObjectiveBase.isComplete());
                }
            }
        }
    }
}