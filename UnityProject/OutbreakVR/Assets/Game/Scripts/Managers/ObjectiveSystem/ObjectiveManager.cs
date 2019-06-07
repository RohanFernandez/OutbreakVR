using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ObjectiveManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static ObjectiveManager s_Instance = null;

        /// <summary>
        /// Folder name where the Objective list assets are kept
        /// </summary>
        private const string OBJECTIVE_LIST_ASSETS_PATH = "ObjectiveListAssets";

        /// <summary>
        /// List of all objective lists
        /// </summary>
        [SerializeField]
        private List<ObjectiveList> m_lstObjectiveLists = null;

        /// <summary>
        /// Current objective list for which objectives are managed
        /// </summary>
        [SerializeField]
        private ObjectiveList m_CurrentObjectiveList = null;

        /// <summary>
        /// Dictionary of level type to its respective ObjectiveList
        /// </summary>
        private Dictionary<string, ObjectiveList> m_dictLevelObjectiveList = null;

        /// <summary>
        /// Manages the objective group pool and objective pool
        /// Creates a objective group pool
        /// </summary>
        private ObjectivePoolManager m_ObjectivePoolManager = null;

        /// <summary>
        /// The current objective group to accomplish
        /// </summary>
        [SerializeField]
        private ObjectiveGroupBase m_CurrentObjectiveGroup = null;

        /// <summary>
        /// Set singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_STATE_CHANGED, onStateChanged);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_LEVEL_SELECTED, onLevelSet);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_LEVEL_OBJECTIVE_TRIGGERED, onLevelObjectiveTriggered);

            m_ObjectivePoolManager = new ObjectivePoolManager();

            initializeObjectiveLists();

            ///initialize dictionary of level type to ObjectiveList
            int l_iTotalObjectiveLists = m_lstObjectiveLists.Count;
            m_dictLevelObjectiveList = new Dictionary<string, ObjectiveList>(l_iTotalObjectiveLists);
            for (int l_iObjListIndex = 0; l_iObjListIndex < l_iTotalObjectiveLists; l_iObjListIndex++)
            {
                ObjectiveList l_ObjList = m_lstObjectiveLists[l_iObjListIndex];
                m_dictLevelObjectiveList.Add(l_ObjList.m_strName, l_ObjList);
            }
        }

        /// <summary>
        /// Destroy , set singleton to null
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }

            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAME_STATE_CHANGED, onStateChanged);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_LEVEL_SELECTED, onLevelSet);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_LEVEL_OBJECTIVE_TRIGGERED, onLevelObjectiveTriggered);
            s_Instance = null;
        }

        /// <summary>
        /// Set the current objective group
        /// </summary>
        private void setCurrentObjectiveGroup(string a_strObjectiveGroupID)
        {
            if (string.IsNullOrEmpty(a_strObjectiveGroupID) || m_CurrentObjectiveList == null)
            {
                m_CurrentObjectiveGroup = null;
                return;
            }

            ScriptableObjectiveGroup l_ObjectiveGroup = m_CurrentObjectiveList.getScriptableObjectiveGroup(a_strObjectiveGroupID);
            if (l_ObjectiveGroup == null)
            {
                m_CurrentObjectiveGroup = null;
            }
            else
            {
                Debug.Log("<color=BLUE>ObjectiveManager::setCurrentObjectiveGroup::</color> Setting objective group with ID '" + a_strObjectiveGroupID + "'");
                m_CurrentObjectiveGroup = (ObjectiveGroupBase)m_ObjectivePoolManager.getObjectiveGroupFromPool(l_ObjectiveGroup);
            }
        }

        /// <summary>
        /// Creates and instance of all Objective list and stores themn into the list
        /// </summary>
        private void initializeObjectiveLists()
        {
            Object[] l_arrAssets = Resources.LoadAll(OBJECTIVE_LIST_ASSETS_PATH);
            int l_iAssetCount = l_arrAssets.Length;
            for (int l_iAssetIndex = 0; l_iAssetIndex < l_iAssetCount; l_iAssetIndex++)
            {
                Object l_AssetObject = l_arrAssets[l_iAssetIndex];
                ObjectiveList l_ObjectiveList = Instantiate(l_AssetObject) as ObjectiveList;
                m_lstObjectiveLists.Add(l_ObjectiveList);
                l_ObjectiveList.initialize();
            }
        }

        /// <summary>
        /// Sets the objective list as the current
        /// </summary>
        public static void SetObjectiveList(string a_strLevelName)
        {
            ObjectiveList l_ObjList = null;
            if (s_Instance.m_dictLevelObjectiveList.TryGetValue(a_strLevelName, out l_ObjList))
            {
                s_Instance.m_CurrentObjectiveList = l_ObjList;
            }
            else
            {
                s_Instance.m_CurrentObjectiveList = null;
                Debug.LogError("ObjectiveManager::SetObjectiveList:: Objective list for level type '" + a_strLevelName + "' does not exist");
            }
            s_Instance.setCurrentObjectiveGroup(null);
        }

        /// <summary>
        /// Checks if an objective in the current objective group is complete
        /// </summary>
        private void manageObjectiveCompletion(Hashtable a_Hashtable)
        {
            if (m_CurrentObjectiveGroup == null || (m_CurrentObjectiveGroup.m_lstObjectives.Count == 0))
            {
                return;
            }

            m_CurrentObjectiveGroup.checkForObjectiveCompletion(a_Hashtable);
            if (m_CurrentObjectiveGroup.IsComplete() &&
                !string.IsNullOrEmpty(m_CurrentObjectiveGroup.m_strChangeStateOnComplete))
            {
                GameStateMachine.Transition(m_CurrentObjectiveGroup.m_strChangeStateOnComplete);
            }
        }

        /// <summary>
        /// Level objective is complete
        /// check for objective in the current objective group is complete
        /// </summary>
        public void onLevelObjectiveTriggered(Hashtable a_Hashtable)
        {
            manageObjectiveCompletion(a_Hashtable);
        }

        /// <summary>
        /// Event called on level set
        /// Sets the Objective list to the given level type
        /// </summary>
        /// <param name="a_hashtable"></param>
        public void onLevelSet(Hashtable a_hashtable)
        {
            string l_strLevelType = a_hashtable[GameEventTypeConst.ID_LEVEL_TYPE].ToString();
            SetObjectiveList(l_strLevelType);
        }

        /// <summary>
        /// On state changed manage the checklist
        /// </summary>
        public void onStateChanged(Hashtable a_hashtable)
        {
            string l_strNewState = a_hashtable[GameEventTypeConst.ID_NEW_GAME_STATE].ToString();
            setCurrentObjectiveGroup(l_strNewState);
        }
    }
}