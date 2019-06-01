using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskPoolManager
    {
        /// <summary>
        /// Stores the name of the type of task to its pool
        /// </summary>
        private Dictionary<string, ITaskPool> m_dictTasksPools = null;

        /// <summary>
        /// Stores the name of the type of sequence to its pool
        /// </summary>
        private Dictionary<string, ISequencePool> m_dictSequencePools = null;

        public TaskPoolManager()
        {
            m_dictTasksPools = new Dictionary<string, ITaskPool>(20);
            m_dictSequencePools = new Dictionary<string, ISequencePool>(20);
        }

        /// <summary>
        /// Returns a task from the pool if a pooled task exists else creates a new task and returns
        /// Sets up the attributes as the arguement
        /// </summary>
        /// <param name="a_Task"></param>
        /// <returns></returns>
        public ITask getTaskFromPool(ScriptableTask a_Task)
        {
            ITaskPool l_TaskPool = null;
            if (!m_dictTasksPools.TryGetValue(a_Task.m_strTaskType, out l_TaskPool))
            {
                l_TaskPool = new TaskPool(a_Task.m_strTaskType, 10);
                m_dictTasksPools.Add(a_Task.m_strTaskType, l_TaskPool);
            }
            ITask l_Task = l_TaskPool.getTask();
            l_Task.onInitialize(a_Task.m_hashAttributes);

            return l_Task;
        }

        /// <summary>
        /// Returns sequence from the pool and sets it as the arguement
        /// </summary>
        /// <param name="a_SequenceBase"></param>
        /// <returns></returns>
        public ISequence getSequenceFromPool(ScriptableSequence a_SequenceBase)
        {
            ISequencePool l_SequencePool = null;
            if (!m_dictSequencePools.TryGetValue(a_SequenceBase.m_strSequenceType, out l_SequencePool))
            {
                l_SequencePool = new SequencePool(a_SequenceBase.m_strSequenceType, 10);
                m_dictSequencePools.Add(a_SequenceBase.m_strSequenceType, l_SequencePool);
            }

            ISequence l_Sequence = l_SequencePool.getSequence();
            for (int l_iTaskIndex = 0; l_iTaskIndex < a_SequenceBase.m_iTaskCount; l_iTaskIndex++)
            {
                l_Sequence.addTask(getTaskFromPool(a_SequenceBase.m_lstTasks[l_iTaskIndex]));
            }

            l_Sequence.onInitialize(a_SequenceBase.m_strSequenceID);

            return l_Sequence;
        }
    }
}