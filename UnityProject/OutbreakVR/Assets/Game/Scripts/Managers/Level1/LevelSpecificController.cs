using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [Serializable]
    class LevelSpecificGameObjects
    {
        [SerializeField]
        private string m_strLevelID = string.Empty;
        public string LevelID
        {
            get { return m_strLevelID; }
        }

        [SerializeField]
        private List<GameObject> m_lstActiveGameObjects = null;
        public List<GameObject> LstActiveGameObj
        {
            get { return m_lstActiveGameObjects; }
        }
    }

    public class LevelSpecificController : MonoBehaviour
    {
        [SerializeField]
        private List<LevelSpecificGameObjects> m_lstLevelSpefic = null;

        [SerializeField]
        private List<GameObject> m_lstLevelSpecificParents = null;

        void Awake()
        {
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_STATE_STARTED, onGameStateChanged);
        }

        void OnDestroy()
        {
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAME_STATE_STARTED, onGameStateChanged);
        }

        private void onGameStateChanged(EventHash a_EventHash)
        {
            string l_strNewLevelName = a_EventHash[GameEventTypeConst.ID_NEW_GAME_STATE].ToString();
            LevelSpecificGameObjects l_CurrentLevelSpecificGameObj = getLevelSpecificGameObject(l_strNewLevelName);

            if (l_CurrentLevelSpecificGameObj == null) { return; }

            List<GameObject> l_lstCurrentActiveGameObjects = l_CurrentLevelSpecificGameObj.LstActiveGameObj;

            int l_iLevelSpcificParentCount = m_lstLevelSpecificParents.Count;
            for (int l_iLevelSpcificParentIndex = 0; l_iLevelSpcificParentIndex < l_iLevelSpcificParentCount; l_iLevelSpcificParentIndex++)
            {
                GameObject l_CurrentParent = m_lstLevelSpecificParents[l_iLevelSpcificParentIndex];
                l_CurrentParent.SetActive(l_lstCurrentActiveGameObjects.Contains(l_CurrentParent));
            }
        }

        private LevelSpecificGameObjects getLevelSpecificGameObject(string a_strLevelId)
        {
            int l_iLevelSpecificCount = m_lstLevelSpefic.Count;
            for(int l_iLevelSpficIndex = 0; l_iLevelSpficIndex < l_iLevelSpecificCount; l_iLevelSpficIndex++)
            {
                if (m_lstLevelSpefic[l_iLevelSpficIndex].LevelID.Equals(a_strLevelId, System.StringComparison.OrdinalIgnoreCase))
                {
                    return m_lstLevelSpefic[l_iLevelSpficIndex];
                }
            }
            return null;
        }
    }
}