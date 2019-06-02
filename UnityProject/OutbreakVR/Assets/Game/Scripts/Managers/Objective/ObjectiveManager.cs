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
        private ObjectiveList m_CurrentObjectiveLists = null;

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

            initializeObjectiveLists();
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
            s_Instance = null;
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
        /// On state changed manage the checklist
        /// </summary>
        public void onStateChanged(Hashtable a_hashtable)
        {

        }
    }
}