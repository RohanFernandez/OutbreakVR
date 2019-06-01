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
            
        }

        public virtual void onComplete()
        {
            m_SequenceCallbackTaskComplete();
        }
    }
}