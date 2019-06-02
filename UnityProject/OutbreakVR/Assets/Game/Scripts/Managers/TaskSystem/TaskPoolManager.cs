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
            l_Task.onStartInitialization(a_Task.m_hashAttributes);

            return l_Task;
        }

        /// <summary>
        /// Returns the task to its respective pool
        /// </summary>
        /// <param name="a_Task"></param>
        public void returnTaskToPool(ITask a_Task)
        {
            ITaskPool l_TaskPool = null;
            if (m_dictTasksPools.TryGetValue(a_Task.getTaskType(), out l_TaskPool))
            {
                l_TaskPool.returnToPool(a_Task);
            }
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

            l_Sequence.onInitialize(a_SequenceBase.m_strSequenceID, a_SequenceBase.m_strSequenceType);

            return l_Sequence;
        }

        /// <summary>
        /// Returns the sequence back to its respective pool
        /// </summary>
        /// <param name="a_Sequence"></param>
        public void returnSequenceToPool(ISequence a_Sequence)
        {
            ISequencePool l_SequencePool = null;
            if (m_dictSequencePools.TryGetValue(a_Sequence.getSequenceType(), out l_SequencePool))
            {
                SequenceBase l_Sequencebase = (SequenceBase)a_Sequence;
                int l_iTaskCount = l_Sequencebase.m_lstTasks.Count;
                for (int l_iTaskIndex = 0; l_iTaskIndex < l_iTaskCount; l_iTaskIndex++)
                {
                    returnTaskToPool(l_Sequencebase.m_lstTasks[l_iTaskIndex]);
                }
                a_Sequence.onReturnedToPool();
                l_SequencePool.returnToPool(a_Sequence);
            }
        }
    }
}