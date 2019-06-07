using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class ObjectiveGroupBase : IObjectiveGroup
    {
        /// <summary>
        /// The unique ID of this objective group base
        /// </summary>
        [SerializeField]
        private string m_strID = string.Empty;

        /// <summary>
        /// The type of the objective group class type
        /// </summary>
        [SerializeField]
        private string m_strObjGroupType = string.Empty;

        /// <summary>
        /// Is objective group complete
        /// </summary>
        [SerializeField]
        protected bool m_bIsComplete = false;

        /// <summary>
        /// Change state to on all objectives are complete
        /// </summary>
        [SerializeField]
        public string m_strChangeStateOnComplete = string.Empty;

        /// <summary>
        /// List of all objectives in this group
        /// </summary>
        [SerializeField]
        public List<ObjectiveBase> m_lstObjectives = new List<ObjectiveBase>(5);

        /// <summary>
        /// Total objectives in this group
        /// </summary>
        protected int m_iTotalObjectiveCount = 0;

        public virtual void onInitialize(string a_strID, string a_strType, string a_strStateChangeOnComplete)
        {
            m_bIsComplete = false;
            m_strID = a_strID;
            m_strObjGroupType = a_strType;
            m_strChangeStateOnComplete = a_strStateChangeOnComplete;
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
            m_bIsComplete = true;
        }

        /// <summary>
        /// Is complete
        /// </summary>
        /// <returns></returns>
        public bool IsComplete()
        {
            return m_bIsComplete;
        }

        /// <summary>
        /// are all the objectives in the group complete
        /// </summary>
        protected bool isObjectiveGroupComplete()
        {
            int l_iObjectiveCount = m_lstObjectives.Count;
            for (int l_iObjIndex = 0; l_iObjIndex < l_iObjectiveCount; l_iObjIndex++)
            {
                if (!m_lstObjectives[l_iObjIndex].isComplete())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if an objective in the group is complete
        /// </summary>
        /// <param name="a_Hashtable"></param>
        public virtual void checkForObjectiveCompletion(Hashtable a_Hashtable)
        {

        }
    }
}