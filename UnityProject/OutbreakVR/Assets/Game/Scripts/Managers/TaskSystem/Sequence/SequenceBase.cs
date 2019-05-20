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

        public virtual void execute()
        {
            
        }

        public virtual void onComplete()
        {
            
        }

        public virtual void onDestroy()
        {
            
        }

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

            return l_Sequence;
        }
    }
}