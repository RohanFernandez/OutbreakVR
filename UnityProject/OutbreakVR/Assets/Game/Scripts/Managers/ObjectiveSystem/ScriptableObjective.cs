using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class ScriptableObjective
    {
        #region Attribute Keys
        public const string KEY_TASK_TYPE = "Type";
        public const string KEY_TASK_ID = "ID";
        public const string KEY_SEQUENCE_ON_COMPLETE_ID = "SequenceOnComplete";
        public const string KEY_OBJECTIVE_DESCRIPTION_ID = "ObjDesc";
        public const string KEY_OBJECTIVE_IS_COMPULSORY_ID = "IsCompulsory";
        public const string KEY_OBJECTIVE_IS_INCLUDED_IN_LISTING = "IsIncludedInListing";
        #endregion Attribute Keys

        /// <summary>
        /// Unique ID of the objective in the group
        /// </summary>
        [SerializeField]
        public string m_strID = string.Empty;

        /// <summary>
        /// The type of the objective
        /// </summary>
        [SerializeField]
        public string m_strType = string.Empty;

        /// <summary>
        /// The sequence to execute on completed
        /// </summary>
        [SerializeField]
        public string m_strSeqOnComplete = string.Empty;

        /// <summary>
        /// The description of the objective
        /// </summary>
        [SerializeField]
        public string m_strObjectiveDesc = string.Empty;

        /// <summary>
        /// The total number of attributes in this objective
        /// </summary>
        [SerializeField]
        public int m_iAttributeCount = 0;

        /// <summary>
        /// The list of all attributes
        /// </summary>
        [SerializeField]
        private List<KeyValueAttribute> m_lstAttributes = null;

        /// <summary>
        /// Hashtable of attributes
        /// </summary>
        public Hashtable m_hashAttributes = null;

        /// <summary>
        /// is the objective compulsory to complete the objective group
        /// </summary>
        public string m_strIsCompulsory = string.Empty;

        /// <summary>
        /// is the objective included in listing
        /// </summary>
        public string m_strIsIncludedInListing = string.Empty;

        public void initialize()
        {
            m_hashAttributes = new Hashtable(m_iAttributeCount);
            for (int l_iAttributesIndex = 0; l_iAttributesIndex < m_iAttributeCount; l_iAttributesIndex++)
            {
                KeyValueAttribute l_CurrentAttribute = m_lstAttributes[l_iAttributesIndex];
                m_hashAttributes.Add(l_CurrentAttribute.m_strKey, l_CurrentAttribute.m_strValue);
            }
        }

#if UNITY_EDITOR
        public static ScriptableObjective GetObjectiveFromXML(System.Xml.XmlNode a_ObjectiveNode)
        {
            System.Xml.XmlAttributeCollection l_ObjectiveAttributes = a_ObjectiveNode.Attributes;
            int l_iAttributeCount = l_ObjectiveAttributes.Count;

            string l_strType = string.Empty;
            string l_strID = string.Empty;
            string l_strSeqOnComplete = string.Empty;
            string l_strObjectiveDesc = string.Empty;
            string l_strObjectiveIsCompulsory = "true";
            string l_strIsIncludedInListing = "true";

            List<KeyValueAttribute> l_lstAttributes = new List<KeyValueAttribute>(l_iAttributeCount);
            for (int l_iAttributeIndex = 0; l_iAttributeIndex < l_iAttributeCount; l_iAttributeIndex++)
            {
                System.Xml.XmlAttribute l_CurrentTaskAttribute = l_ObjectiveAttributes[l_iAttributeIndex];

                KeyValueAttribute l_Attribute = new KeyValueAttribute();
                l_Attribute.m_strKey = l_CurrentTaskAttribute.Name;
                l_Attribute.m_strValue = l_CurrentTaskAttribute.Value;

                if (l_Attribute.m_strKey.Equals(KEY_TASK_TYPE, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strType = l_Attribute.m_strValue;
                }
                else if (l_Attribute.m_strKey.Equals(KEY_TASK_ID, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strID = l_Attribute.m_strValue;
                }
                else if (l_Attribute.m_strKey.Equals(KEY_SEQUENCE_ON_COMPLETE_ID, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strSeqOnComplete = l_Attribute.m_strValue;
                }
                else if (l_Attribute.m_strKey.Equals(KEY_OBJECTIVE_DESCRIPTION_ID, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strObjectiveDesc = l_Attribute.m_strValue;
                }
                else if (l_Attribute.m_strKey.Equals(KEY_OBJECTIVE_IS_COMPULSORY_ID, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strObjectiveIsCompulsory = l_Attribute.m_strValue;
                }
                else if (l_Attribute.m_strKey.Equals(KEY_OBJECTIVE_IS_INCLUDED_IN_LISTING, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_strIsIncludedInListing = l_Attribute.m_strValue;
                }
                l_lstAttributes.Add(l_Attribute);
            }

            ScriptableObjective l_ScriptableObjective = new ScriptableObjective();
            l_ScriptableObjective.m_iAttributeCount = l_iAttributeCount;
            l_ScriptableObjective.m_lstAttributes = l_lstAttributes;
            l_ScriptableObjective.m_strSeqOnComplete = l_strSeqOnComplete;
            l_ScriptableObjective.m_strType = l_strType;
            l_ScriptableObjective.m_strID = l_strID;
            l_ScriptableObjective.m_strObjectiveDesc = l_strObjectiveDesc;
            l_ScriptableObjective.m_strIsCompulsory = l_strObjectiveIsCompulsory;
            l_ScriptableObjective.m_strIsIncludedInListing = l_strIsIncludedInListing;

            return l_ScriptableObjective;
        }
#endif
    }
}