using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ObjectiveGroupBase : IObjectiveGroup
    {
        /// <summary>
        /// List of all objectives in this group
        /// </summary>
        public List<IObjective> m_lstObjectives = new List<IObjective>(5);

        /// <summary>
        /// The unique ID of this objective group base
        /// </summary>
        private string m_strID = string.Empty;

        /// <summary>
        /// The type of the objective group class type
        /// </summary>
        private string m_strObjGroupType = string.Empty;

        /// <summary>
        /// Total objectives in this group
        /// </summary>
        private int m_iTotalObjectiveCount = 0;

        public void onInitialize(string a_strID, string a_strType)
        {
            m_strID = a_strID;
            m_strObjGroupType = a_strType;
            m_iTotalObjectiveCount = m_lstObjectives.Count;
        }

        public void addObjective(ObjectiveBase a_Objective)
        {
            m_lstObjectives.Add(a_Objective);
        }

        public string getObjGroupType()
        {
            return m_strObjGroupType;
        }

        public string getObjGroupID()
        {
            return m_strID;
        }

        public void onReturnedToPool()
        {
            m_lstObjectives.Clear();
        }

        /// <summary>
        /// When all the objectives in this group is completed
        /// </summary>
        public virtual void onComplete()
        {

        }
    }
}