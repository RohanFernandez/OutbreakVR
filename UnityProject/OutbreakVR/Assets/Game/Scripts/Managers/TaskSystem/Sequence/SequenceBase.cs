using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class SequenceBase : ISequence
    {
        /// <summary>
        /// The unique sequence identity
        /// </summary>
        [SerializeField]
        public string m_strSequenceID = string.Empty;
        public string SequenceID
        {
            get { return m_strSequenceID; }
        }

        /// <summary>
        /// List of all tasks to be executed in a single sequence
        /// </summary>
        [SerializeField]
        public List<JobBase> m_lstJobs = null;

        /// <summary>
        /// The current job at which the sequence is executing
        /// </summary>
        private int m_iCurrentJobIndex = 0;

        /// <summary>
        /// The total number of jobs in this sequence
        /// </summary>
        private int m_iJobCount = 0;

        public virtual void execute()
        {
            m_iCurrentJobIndex = 0;
            m_iJobCount = m_lstJobs.Count;
            executeJob();
        }

        /// <summary>
        /// Executes the current job in the index
        /// </summary>
        public virtual void executeJob()
        {
            if (m_iCurrentJobIndex < m_iJobCount)
            {
                m_lstJobs[m_iCurrentJobIndex].onExecute();
            }
        }

        public virtual void onJobComplete()
        {
            ++m_iCurrentJobIndex;
            if (m_iCurrentJobIndex >= m_iJobCount)
            {
                onComplete();
            }
            else
            {
                executeJob();
            }
        }

        public virtual void onComplete()
        {
            
        }

#if UNITY_EDITOR
        /// <summary>
        /// Creates a sequence from the Sequence node
        /// Adds all the jobs inside the sequence
        /// </summary>
        /// <param name="a_SequenceNode"></param>
        /// <returns></returns>
        public static SequenceBase GetSequence(System.Xml.XmlNode a_SequenceNode)
        {
            SequenceBase l_Sequence = new SequenceBase();
            System.Xml.XmlAttributeCollection l_arrAttributes = a_SequenceNode.Attributes;
            int l_iAttribCount = l_arrAttributes.Count;

            for (int l_iAttribIndex = 0; l_iAttribIndex < l_iAttribCount; l_iAttribIndex++)
            {
                System.Xml.XmlAttribute l_CurrentAttrib = l_arrAttributes[l_iAttribIndex];

                if (TaskListConsts.XML_KEYWORD_ID.Equals(l_CurrentAttrib.Name, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_Sequence.m_strSequenceID = l_CurrentAttrib.Value;
                }
            }

            System.Xml.XmlNodeList l_lstJobsNodes = a_SequenceNode.ChildNodes;
            int l_iJobCount = l_lstJobsNodes.Count;
            l_Sequence.m_lstJobs = new List<JobBase>(l_iJobCount);

            for (int l_iJobIndex = 0; l_iJobIndex < l_iJobCount; l_iJobIndex++)
            {
                System.Xml.XmlNode l_CurrentJobNode = l_lstJobsNodes[l_iJobIndex];
                JobBase l_CurrentJob = JobBase.GetJob(l_CurrentJobNode, l_Sequence);
                l_Sequence.m_lstJobs.Add(l_CurrentJob);
            }

            return l_Sequence;
        }
#endif
    }
}