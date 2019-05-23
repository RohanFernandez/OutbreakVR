using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class TaskList : ScriptableObject
    {
        /// <summary>
        /// The name of the tasklist
        /// </summary>
        [SerializeField]
        public string m_strName = string.Empty;

        /// <summary>
        /// XML node name that spcifies the biggest parent
        /// </summary>
        [SerializeField]
        public List<SequenceBase> m_lstSequences = null;

        private TaskList() { }

#if UNITY_EDITOR

        public static TaskList GetParsedXML(string a_strPath)
        {
            return GetTaskList(a_strPath);
        }

        /// <summary>
        /// Sets up list of all sequences
        /// </summary>
        /// <param name="a_strPath"></param>
        private static TaskList GetTaskList(string a_strPath)
        {
            TaskList l_TaskList = new TaskList();

            System.Xml.XmlDocument l_TaskListXML = new System.Xml.XmlDocument();
            l_TaskListXML.Load(a_strPath);

            System.Xml.XmlNodeList l_lstXMLNode = l_TaskListXML.GetElementsByTagName(TaskListConsts.XML_NODE_TASK_LIST);
            if (l_lstXMLNode == null)
            {
                TaskListConsts.LogXMLSearchError(a_strPath, TaskListConsts.XML_NODE_TASK_LIST, true);
                return null;
            }

            System.Xml.XmlNode l_TaskListNode = l_lstXMLNode[0];
            l_TaskList.m_strName = l_TaskListNode.Attributes[0].Value;

            System.Xml.XmlNodeList l_lstSequenceNodes = l_TaskListNode.ChildNodes;
            int l_iSequenceCount = l_lstSequenceNodes.Count;
            l_TaskList.m_lstSequences = new List<SequenceBase>(l_iSequenceCount);

            ///Parse and set sequences into the list
            for (int l_iSequenceIndex = 0; l_iSequenceIndex < l_iSequenceCount; l_iSequenceIndex++)
            {
                System.Xml.XmlNode l_CurrentSequenceNode = l_lstSequenceNodes[l_iSequenceIndex];
                SequenceBase l_CreatedSequence = SequenceBase.GetSequence(l_CurrentSequenceNode);
                l_TaskList.m_lstSequences.Add(l_CreatedSequence);
            }
            return l_TaskList;
        }
#endif
    }
}