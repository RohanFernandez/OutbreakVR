﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class SequenceBase : ISequence
    {
        /// <summary>
        /// List of all tasks to be executed in this sequence
        /// </summary>
        protected List<ITask> m_lstTasks = new List<ITask>(5);

        /// <summary>
        /// The index of the currently running task
        /// </summary>
        protected int m_iRunningTask = 0;

        /// <summary>
        /// Total tasks in this sequence
        /// </summary>
        protected int m_iTotalTasks = 0;

        /// <summary>
        /// The unique sequence ID
        /// </summary>
        protected string m_strSequenceID = string.Empty;

        public void addTask(ITask a_Task)
        {
            m_lstTasks.Add(a_Task);
        }

        public virtual void onInitialize(string a_strSequenceID)
        {
            m_iRunningTask = 0;
            m_strSequenceID = a_strSequenceID;
            m_iTotalTasks = m_lstTasks.Count;
        }

        /// <summary>
        /// On execution begin of the sequence
        /// </summary>
        public virtual void onExecute()
        {
            executeTask();
        }

        /// <summary>
        /// Callback on sequence complete
        /// </summary>
        public virtual void onComplete()
        {
            m_lstTasks.Clear();

            Hashtable l_hash = new Hashtable(1);
            l_hash.Add(GameEventTypeConst.ID_SEQUENCE_REF, this);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_SEQUENCE_COMPLETE, l_hash);
        }

        /// <summary>
        /// Executes the next task in line
        /// </summary>
        public virtual void executeTask()
        {
            if (m_iRunningTask < m_iTotalTasks)
            {
                m_lstTasks[m_iRunningTask].onStartExecution(onTaskComplete);
            }
            else
            {
                onComplete();
            }
        }

        /// <summary>
        /// on any task complete
        /// </summary>
        public virtual void onTaskComplete()
        {
            //execute next task
            m_iRunningTask++;
            executeTask();
        }
    }
}