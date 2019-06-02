﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ObjectiveList : ScriptableObject
    {
        /// <summary>
        /// The unique name of the checklist
        /// </summary>
        [SerializeField]
        public string m_strName = string.Empty;

        /// <summary>
        /// Total number of Objective Groups in this list
        /// </summary>
        [SerializeField]
        public int m_iObjectiveGroupCount = 0;

        /// <summary>
        /// List of all objectives groups
        /// </summary>
        [SerializeField]
        public List<ScriptableObjectiveGroup> m_lstObjectiveGroup = null;

        /// <summary>
        /// Sets the 
        /// </summary>
        public void initialize()
        {

        }

#if UNITY_EDITOR

        /// <summary>
        /// Name of the tag in the objective list
        /// </summary>
        private const string TAG_OBJECTIVE_LIST = "ObjectiveList";

        /// <summary>
        /// Returns a ObjectiveList, parsed from the XML
        /// </summary>
        /// <param name="a_strURL"></param>
        /// <returns></returns>
        public static ObjectiveList GetObjectiveList(string a_strURL)
        {
            System.Xml.XmlDocument l_XMLDoc = new System.Xml.XmlDocument();
            l_XMLDoc.Load(a_strURL);

            System.Xml.XmlNodeList l_NodeList = l_XMLDoc.GetElementsByTagName(TAG_OBJECTIVE_LIST);
            if (l_NodeList == null || l_NodeList.Count == 0)
            {
                Debug.LogError("ObjectiveList::GetObjectiveListFromXML::Could not find node with Tag : " + TAG_OBJECTIVE_LIST);
                return null;
            }

            System.Xml.XmlNode l_ObjectiveListNode = l_NodeList[0];
            System.Xml.XmlAttribute l_ObjectiveListAttribute = l_ObjectiveListNode.Attributes[0];

            // Name of the objective list
            string l_strTaskListName = l_ObjectiveListAttribute.Value;

            // Parse Objective group
            System.Xml.XmlNodeList l_lstObjectiveGroupNodes = l_ObjectiveListNode.ChildNodes;
            int l_iObjectiveGroupCount = l_lstObjectiveGroupNodes.Count;
            List<ScriptableObjectiveGroup> l_lstObjectiveGroup = new List<ScriptableObjectiveGroup>(l_iObjectiveGroupCount);

            for (int l_iObjGroupIndex = 0; l_iObjGroupIndex < l_iObjectiveGroupCount; l_iObjGroupIndex++)
            {
                System.Xml.XmlNode l_ObjGroupNode = l_lstObjectiveGroupNodes[l_iObjGroupIndex];
                l_lstObjectiveGroup.Add(ScriptableObjectiveGroup.GetObjectiveGroupFromXML(l_ObjGroupNode));
            }

            ObjectiveList l_ObjectiveList = new ObjectiveList();
            l_ObjectiveList.m_strName = l_strTaskListName;
            l_ObjectiveList.m_lstObjectiveGroup = l_lstObjectiveGroup;
            l_ObjectiveList.m_iObjectiveGroupCount = l_iObjectiveGroupCount;

            return l_ObjectiveList;
        }

#endif
    }
}