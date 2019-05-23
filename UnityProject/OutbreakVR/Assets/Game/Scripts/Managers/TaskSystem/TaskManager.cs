using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static TaskManager s_Instance = null;

        private const string TASK_LIST_DIRECTORY = "TaskListAssets\\";

        /// <summary>
        /// The dictionary that will hold all the sequences so that it will be easier to retriece the sequence by its name
        /// </summary>
        private Dictionary<string, SequenceBase> m_dictSequences = null;

        /// <summary>
        /// List of all sequences that are running at the moment
        /// </summary>
        [SerializeField]
        private List<SequenceBase> m_lstRunningSequences = null;

        /// <summary>
        /// sets singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            m_lstRunningSequences = new List<SequenceBase>(15);
            loadTaskList("TaskListLevel1");
        }

        /// <summary>
        /// destroys singleton instance
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            s_Instance = null;
        }

        /// <summary>
        /// Loads task list asset from resource folder
        /// instantiates it and sets it as the current TaskList
        /// </summary>
        /// <param name="a_strTaskListName"></param>
        private void loadTaskList(string a_strTaskListName)
        {
            //if (a_strTaskListName.Equals(m_TaskList.m_strName, System.StringComparison.OrdinalIgnoreCase))
            //{
            //    Debug.LogError("TaskManager::loadTaskList:: Attempting to initialize the already loaded tasklist.");
            //    return;
            //}

            m_dictSequences = null;
            UnityEngine.Object l_objTaskList = (Resources.Load(TASK_LIST_DIRECTORY + a_strTaskListName, typeof(TaskList)));
            TaskList l_TaskList = (TaskList)Instantiate(l_objTaskList);

            if (l_TaskList == null)
            {
                Debug.LogError("TaskManager::loadTaskList::Failed to load TaskList Asset with name :: '" + a_strTaskListName + "'");
                return;
            }

            setUpSequenceDictionary(l_TaskList);
            Resources.UnloadAsset(l_objTaskList);

        }

        /// <summary>
        /// Sets up the dictionary 
        /// </summary>
        private void setUpSequenceDictionary(TaskList a_TaskList)
        {
            int l_iSequenceCount = a_TaskList.m_lstSequences.Count;
            m_dictSequences = new Dictionary<string, SequenceBase>(l_iSequenceCount);

            for (int l_iSequenceIndex = 0; l_iSequenceIndex < l_iSequenceCount; l_iSequenceIndex++)
            {
                SequenceBase l_CurrentSequence = a_TaskList.m_lstSequences[l_iSequenceIndex];
                m_dictSequences.Add(l_CurrentSequence.m_strSequenceID, l_CurrentSequence);
            }
        }

        /// <summary>
        /// Returns sequence in dictionary with ID
        /// </summary>
        /// <param name="a_strSequenceID"></param>
        /// <returns></returns>
        private SequenceBase getSequenceWithID(string a_strSequenceID)
        {
            SequenceBase l_SequenceBase = null;
            m_dictSequences.TryGetValue(a_strSequenceID, out l_SequenceBase);
            return l_SequenceBase;
        }

        /// <summary>
        /// Starts executing the sequence
        /// </summary>
        /// <param name="a_Sequence"></param>
        private void executeSequence(SequenceBase a_Sequence)
        {
            m_lstRunningSequences.Add(a_Sequence);
            a_Sequence.execute();
        }

        /// <summary>
        /// Removes given sequence from list of running sequences
        /// </summary>
        /// <param name="a_Sequence"></param>
        public static void OnSequenceComplete(SequenceBase a_Sequence)
        {
            s_Instance.m_lstRunningSequences.Remove(a_Sequence);
        }

        /// <summary>
        /// Starts executing the sequence with name
        /// </summary>
        /// <param name="a_strSequenceName"></param>
        public static void ExecuteSequece(string a_strSequenceName)
        {
            SequenceBase l_Sequence =  s_Instance.getSequenceWithID(a_strSequenceName);
            if (l_Sequence == null)
            {
                Debug.LogError("TaskManager::ExecuteSequece:: Sequence with name :'"+ a_strSequenceName + "' could not be found in the dictionary");
                return;
            }

            s_Instance.executeSequence(l_Sequence);
        }
    }
}