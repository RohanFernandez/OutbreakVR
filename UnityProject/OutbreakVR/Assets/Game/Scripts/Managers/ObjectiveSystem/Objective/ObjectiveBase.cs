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
        /// Attributes in this objective
        /// </summary>
        private Hashtable m_hashAttributes = null;

        /// <summary>
        /// called on start of initialization
        /// Sets the attributes to the member variable
        /// </summary>
        /// <param name="a_hashAttributes"></param>
        public void onStartInitialization(Hashtable a_hashAttributes)
        {
            m_hashAttributes = a_hashAttributes;
            m_strType = getString(ScriptableObjective.KEY_TASK_TYPE);
            m_strID = getString(ScriptableObjective.KEY_TASK_ID);
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
            
        }

        public virtual void onUpdate()
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