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
        public string ID
        {
            get { return m_strID; }
        }

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
        /// Description of the objective
        /// </summary>
        [SerializeField]
        private string m_strObjDescription = string.Empty;
        public string ObjDescription
        {
            get { return m_strObjDescription; }
        }

        /// <summary>
        /// Is objective complete
        /// </summary>
        [SerializeField]
        protected bool m_bIsComplete = false;

        /// <summary>
        /// Is objective compulsory
        /// </summary>
        [SerializeField]
        protected bool m_bIsCompulsory = true;

        /// <summary>
        /// Should be included in the listing
        /// </summary>
        [SerializeField]
        protected bool m_bIsIncludedInListing = true;

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
            m_strObjDescription = getString(ScriptableObjective.KEY_OBJECTIVE_DESCRIPTION_ID);
            m_bIsCompulsory = getBool(ScriptableObjective.KEY_OBJECTIVE_IS_COMPULSORY_ID, true);
            m_bIsIncludedInListing = getBool(ScriptableObjective.KEY_OBJECTIVE_IS_INCLUDED_IN_LISTING, true);
        }

        /// <summary>
        /// Returns the objective type name
        /// </summary>
        /// <returns></returns>
        public string getObjectiveType()
        {
            return m_strType;
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
        /// Is included in listing
        /// </summary>
        public bool isIncludedInListing()
        {
            return m_bIsIncludedInListing;
        }

        /// <summary>
        /// Is the objective compulsory
        /// </summary>
        public bool isCompulsory()
        {
            return m_bIsCompulsory;
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
        public bool getBool(string a_strAttributeKey, bool a_bDefaultValue = false)
        {
            return GeneralUtils.GetBool(m_hashAttributes, a_strAttributeKey, a_bDefaultValue);
        }
        #endregion GET VARIABLE FROM OBJECT
    }
}