﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class TaskBase : ITask
    {
        /// <summary>
        /// Callback for the sequence on task is complete
        /// </summary>
        System.Action m_SequenceCallbackTaskComplete = null;

        /// <summary>
        /// Hashtable of key to value of the task attributes
        /// </summary>
        Hashtable m_hashAttributes = null;

        /// <summary>
        /// The name of the task type class
        /// </summary>
        private string m_strTaskType = string.Empty;

        public virtual void onStartInitialization(Hashtable a_hashAttributes)
        {
            m_hashAttributes = a_hashAttributes;
            m_strTaskType = getString(ScriptableTask.KEY_TASK_TYPE);
            onInitialize();
        }

        public virtual void onStartExecution(System.Action a_SequenceCallbackTaskComplete)
        {
            m_SequenceCallbackTaskComplete = a_SequenceCallbackTaskComplete;
            onExecute();
        }

        public virtual void onInitialize()
        {   
        }

        public virtual void onExecute()
        {
        }

        public virtual void onComplete()
        {
            m_SequenceCallbackTaskComplete();
        }

        public virtual void onUpdate()
        {

        }

        //Returns the type of task
        public string getTaskType()
        {
            return m_strTaskType;
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
