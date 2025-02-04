﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class LevelObjectivePauseState : PauseManagedStateBase
    {
        /// <summary>
        /// List of all objective panels
        /// </summary>
        [SerializeField]
        private List<PanelObjective> m_lstPanelObjectives = null;

        public override void onStateEnter(string a_strState)
        {
            base.onStateEnter(a_strState);
            refreshObjectives();
        }

        public override void onStateExit(string a_strState)
        {
            base.onStateExit(a_strState);
        }

        void OnEnable()
        {
            refreshObjectives();
        }

        /// <summary>
        /// Resets the objectives in the panel as per the current level
        /// </summary>
        public void refreshObjectives()
        {
            ObjectiveGroupBase l_CurrentLevelObjectiveGroup = ObjectiveManager.CurrentObjectiveGroup;

            int l_iPanelObjectiveCount = m_lstPanelObjectives.Count;
            int l_ObjectiveGroupCount = l_CurrentLevelObjectiveGroup
                == null ? 0 : l_CurrentLevelObjectiveGroup.m_lstObjectives.Count;
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
                    ObjectiveBase l_ObjectiveBase = l_CurrentLevelObjectiveGroup.m_lstObjectives[l_iObjectiveIndex];

                    bool l_bIsIncludedInListing =  l_ObjectiveBase.isIncludedInListing();
                    l_PanelObjective.gameObject.SetActive(l_bIsIncludedInListing);
                    if (l_bIsIncludedInListing)
                    {
                        l_PanelObjective.updateText(l_ObjectiveBase.ObjDescription, l_ObjectiveBase.isComplete(), l_ObjectiveBase.isCompulsory());
                    }
                }
            }
        }

        /// <summary>
        /// On trigger/ select button pressed
        /// </summary>
        public override void onSelectPressed(System.Action a_onReturnControlToMainPanel)
        {
            base.onSelectPressed(a_onReturnControlToMainPanel);
            GameManager.PauseGame(false, false);
        }
    }
}