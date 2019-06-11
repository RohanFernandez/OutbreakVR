using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class ObjectiveBase : IObjective
    {
        [SerializeField]
        private string m_strID = string.Empty;

        /// <summary>
        /// The type of the objective
        /// </summary>
        [SerializeField]
        private string m_strType = string.Empty;

        /// <summary>
        /// Exectues sequence on complete
        /// </summary>
        [SerializeField]
        private string m_strSequenceOnComplete = string.Empty;

        /// <summary>
        /// Is objective complete
        /// </summary>
        [SerializeField]
        protected bool m_bIsComplete = false;

        /// <summary>
        /// Attributes in this objective
        /// </summary>
        protected Hashtable m_hashAttributes = null;

        /// <summary>
        /// called on start of initialization
        /// Sets the attributes to the member variable
        /// </summary>
        /// <param name="a_hashAttributes"></param>
        public virtual void onInitialize(Hashtable a_hashAttributes)
        {
            m_bIsComplete = false;
            m_hashAttributes = a_hashAttributes;
            m_strType = getString(ScriptableObjective.KEY_TASK_TYPE);
            m_strID = getString(ScriptableObjective.KEY_TASK_ID);
            m_strSequenceOnComplete = getString(ScriptableObjective.KEY_SEQUENCE_ON_COMPLETE_ID);
        }

        /// <summary>
        /// Returns the objective type name
        /// </summary>
        /// <returns></returns>
        public string getObjectiveType()
        {
            return null;
        }

        public virtual void onComplete()
        {
            m_bIsComplete = true;
            TaskManager.ExecuteSequence(m_strSequenceOnComplete);
        }

        /// <summary>
        /// Is complete
        /// </summary>
        public bool isComplete()
        {
            return m_bIsComplete;
        }

        /// <summary>
        /// Checks if the objective is complete with the args
        /// </summary>
        /// <returns></returns>
        public virtual void checkObjectiveCompletion(Hashtable a_Hashtable)
        {
            
        }

        public virtual void onReturnedToPool()
        {
            
        }

        public virtual void onRetrievedFromPool()
        {
            
        }

        #region GET VARIABLE FROM OBJECT

        /// <summary>
        /// Returns string from hashtable object
        /// Returns string.Empty if object is null
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public string getString(string a_strAttributeKey)
        {
            return GeneralUtils.GetString(m_hashAttributes, a_strAttributeKey);
        }

        /// <summary>
        /// Returns string from hashtable object
        /// Returns 0 if object is null
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public int getInt(string a_strAttributeKey)
        {
            return GeneralUtils.GetInt(m_hashAttributes, a_strAttributeKey);
        }

        /// <summary>
        /// Returns string from hashtable object
        /// Returns 0.0f if object is null
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public float getFloat(string a_strAttributeKey)
        {
            return GeneralUtils.GetFloat(m_hashAttributes, a_strAttributeKey);
        }

        /// <summary>
        /// Returns string from hashtable object
        /// Return false if key is null or empty
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public bool getBool(string a_strAttributeKey)
        {
            return GeneralUtils.GetBool(m_hashAttributes, a_strAttributeKey);
        }
        #endregion GET VARIABLE FROM OBJECT
    }
}