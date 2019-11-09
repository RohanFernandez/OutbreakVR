using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class ScriptableObjectiveGroup 
    {
        /// <summary>
        /// Unique group ID
        /// </summary>
        [SerializeField]
        public string m_strID = string.Empty;

        /// <summary>
        /// Objective group type
        /// </summary>
        [SerializeField]
        public string m_strType = string.Empty;

        /// <summary>
        /// Total count of objectives in the list
        /// </summary>
        [SerializeField]
        public int m_iObjectiveCount = 0;

        /// <summary>
        /// List of all objectives
        /// </summary>
        [SerializeField]
        public List<ScriptableObjective> m_lstScriptibeObjective = null;

        public void initialize()
        {
            for (int l_iObjectiveIndex = 0; l_iObjectiveIndex < m_iObjectiveCount; l_iObjectiveIndex++)
            {
                m_lstScriptibeObjective[l_iObjectiveIndex].initialize();
            }
        }

        #region Attribute Keys
        public const string KEY_OBJECTIVE_GROUP_TYPE    = "Type";
        public const string KEY_GROUP_ID                = "ID";
        public const string KEY_START_STATE_ON_COMPLETE = "StartStateOnComplete";
        #endregion Attribute Keys

#if UNITY_EDITOR

        /// <summary>
        /// Returns a scriptable objective group from an XML node
        /// </summary>
        /// <param name="a_Node"></param>
        /// <returns></returns>
        public static ScriptableObjectiveGroup GetObjectiveGroupFromXML(System.Xml.XmlNode a_ObjectiveGroupNode)
        {
            string l_strObjectiveGroupId = string.Empty;
            string l_strObjectiveGroupType = string.Empty;
            string l_strChangeStateOnComplete = string.Empty;

            int l_iAttributesCount = a_ObjectiveGroupNode.Attributes.Count;
            for (int l_iAttributeIndex = 0; l_iAttributeIndex < l_iAttributesCount; l_iAttributeIndex++)
            {
                System.Xml.XmlAttribute l_Attribute = a_ObjectiveGroupNode.Attributes[l_iAttributeIndex];
                if (l_Attribute.Name.Equals(KEY_GROUP_ID, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strObjectiveGroupId = l_Attribute.Value;
                }
                else if (l_Attribute.Name.Equals(KEY_OBJECTIVE_GROUP_TYPE, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strObjectiveGroupType = l_Attribute.Value;
                }
                else if (l_Attribute.Name.Equals(KEY_START_STATE_ON_COMPLETE, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strChangeStateOnComplete = l_Attribute.Value;
                }
            }

            System.Xml.XmlNodeList l_lstObjectiveNodes = a_ObjectiveGroupNode.ChildNodes;
            int l_iObjectiveCount = l_lstObjectiveNodes.Count;

            // Parse tasks
            List<ScriptableObjective> l_lstScriptableObjective = new List<ScriptableObjective>(l_iObjectiveCount);
            for (int l_iTaskIndex = 0; l_iTaskIndex < l_iObjectiveCount; l_iTaskIndex++)
            {
                System.Xml.XmlNode l_ObjectiveNode = l_lstObjectiveNodes[l_iTaskIndex];
                l_lstScriptableObjective.Add(ScriptableObjective.GetObjectiveFromXML(l_ObjectiveNode));
            }

            ScriptableObjectiveGroup l_ScriptableObjectiveGroup = new ScriptableObjectiveGroup();
            l_ScriptableObjectiveGroup.m_strID                      = l_strObjectiveGroupId;
            l_ScriptableObjectiveGroup.m_iObjectiveCount            = l_iObjectiveCount;
            l_ScriptableObjectiveGroup.m_strType                    = l_strObjectiveGroupType;
            l_ScriptableObjectiveGroup.m_lstScriptibeObjective      = l_lstScriptableObjective;

            return l_ScriptableObjectiveGroup;
        }
#endif
    }
}