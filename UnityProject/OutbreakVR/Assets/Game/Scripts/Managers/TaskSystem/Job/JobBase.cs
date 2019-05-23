using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class JobAttribute
    {
        public JobAttribute(string a_Key, string a_Value)
        {
            m_Key = a_Key;
            m_Value = a_Value;
        }

        [SerializeField]
        public string m_Key = string.Empty;

        [SerializeField]
        public string m_Value = string.Empty;
    }

    [System.Serializable]
    public class JobBase : IJob
    {
        [SerializeField]
        public string m_strJobID = string.Empty;

        [SerializeField]
        public string m_strJobType = string.Empty;

        [SerializeField]
        public List<JobAttribute> m_lstJobData = null;

        [SerializeField]
        private int m_iAttributeCount = 0;

        [SerializeField]
        public SequenceBase m_Sequence = null; 

        protected Hashtable m_hashInitTaskData = null;

        protected JobBase(Hashtable a_Hashtable)
        {
            m_strJobID = a_Hashtable[TaskListConsts.XML_KEYWORD_ID].ToString();
            m_strJobType = a_Hashtable[TaskListConsts.XML_KEYWORD_TYPE].ToString();
            m_iAttributeCount = a_Hashtable.Count;

            m_lstJobData = new List<JobAttribute>(m_iAttributeCount);
            foreach (DictionaryEntry l_Pair in a_Hashtable)
            {
                m_lstJobData.Add(new JobAttribute(l_Pair.Key.ToString(), l_Pair.Value.ToString()));
            }
        }

        /// <summary>
        /// Returns the job attribute with given ID
        /// </summary>
        /// <param name="a_strJobAttributeKey"></param>
        /// <returns></returns>
        protected JobAttribute getJobAttribute(string a_strJobAttributeKey)
        {
            for (int l_iAttributeIndex = 0; l_iAttributeIndex < m_iAttributeCount ; l_iAttributeIndex++)
            {
                if (a_strJobAttributeKey.Equals(m_lstJobData[l_iAttributeIndex].m_Key))
                {
                    return m_lstJobData[l_iAttributeIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// Called on start of execution of this job
        /// </summary>
        public virtual void onExecute()
        {
        }

        /// <summary>
        /// Called on job completed execution
        /// </summary>
        public virtual void onComplete()
        {
            m_Sequence.onJobComplete();
        }

        /// <summary>
        /// Returns a Job of type inherited from Job base
        /// </summary>
        /// <param name="a_JobNode"></param>
        /// <returns></returns>
        public static JobBase GetJob(System.Xml.XmlNode a_JobNode, SequenceBase a_ParentSequence)
        {
            JobBase l_CurrentJob = null;
            System.Xml.XmlAttributeCollection l_lstAttributes = a_JobNode.Attributes;
            int l_iAttribCount = l_lstAttributes.Count;
            Hashtable l_HashAttrib = new Hashtable(l_iAttribCount);

            for (int l_iAttribIndex = 0; l_iAttribIndex < l_iAttribCount; l_iAttribIndex++)
            {
                System.Xml.XmlAttribute l_CurrentArrtib = l_lstAttributes[l_iAttribIndex];
                l_HashAttrib.Add(l_CurrentArrtib.Name, l_CurrentArrtib.Value);
            }
            string l_strJobtype = l_HashAttrib[TaskListConsts.XML_KEYWORD_TYPE].ToString();
            System.Reflection.Assembly l_Assembly = typeof(JobBase).Assembly;
            System.Type l_JobType = l_Assembly.GetType("ns_Mashmo." + l_strJobtype);

            if (l_JobType == null)
            {
                Debug.LogError("Cannot parse job type of type : '" + l_strJobtype + "'");
                return null;
            }
            l_CurrentJob = System.Activator.CreateInstance(l_JobType, l_HashAttrib) as JobBase;
            l_CurrentJob.m_Sequence = a_ParentSequence;

            return l_CurrentJob;
        }
    }
}