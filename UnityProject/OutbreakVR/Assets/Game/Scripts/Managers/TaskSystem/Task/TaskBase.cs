using System.Collections;
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
        /// The name of the task type class
        /// </summary>
        private string m_strTaskType = string.Empty;

        public virtual void onStartExecution(System.Action a_SequenceCallbackTaskComplete)
        {
            m_SequenceCallbackTaskComplete = a_SequenceCallbackTaskComplete;
            onExecute();
        }

        public virtual void onExecute()
        {
        }

        public virtual void onInitialize(Hashtable a_hashAttributes)
        {
            m_strTaskType = a_hashAttributes[ScriptableTask.KEY_TASK_TYPE].ToString();
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
    }
}