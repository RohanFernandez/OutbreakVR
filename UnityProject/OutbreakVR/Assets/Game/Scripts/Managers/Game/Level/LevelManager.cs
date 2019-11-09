using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class LevelManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static LevelManager s_Instance = null;

        /// <summary>
        /// The list of all levels in the game
        /// </summary>
        [SerializeField]
        private List<LevelData> m_lstLevelData = null;

        /// <summary>
        /// The level data of the current level the game is in
        /// </summary>
        [SerializeField]
        private string m_strCurrLevelName = string.Empty;

        /// <summary>
        /// The sublevel data of the current sub level the game is in
        /// </summary>
        [SerializeField]
        private string m_strCurrSubLevelName = string.Empty;

        /// <summary>
        /// Sets the singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_OBJECTIVE_GROUP_COMPLETED, onLevelObjectiveGroupCompleted);

            int l_iLevelDataCount = m_lstLevelData.Capacity;
            for (int l_iLevelDataIndex = 0; l_iLevelDataIndex < l_iLevelDataCount; l_iLevelDataIndex++)
            {
                m_lstLevelData[l_iLevelDataIndex].initialize(l_iLevelDataIndex);
            }
        }

        /// <summary>
        /// Sets singleton to null
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }

            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_OBJECTIVE_GROUP_COMPLETED, onLevelObjectiveGroupCompleted);

            s_Instance = null;
        }

        /// <summary>
        /// Gets the level data with the given name from the list of all Level Datas
        /// </summary>
        /// <param name="a_strLevelData"></param>
        /// <returns></returns>
        private LevelData getLevelDataWithName(string a_strLevelData)
        {
            LevelData l_CurrLevelData = null;
            int l_iLevelDataCount = m_lstLevelData.Count;
            for (int l_iCurrLevelData = 0; l_iCurrLevelData < l_iLevelDataCount; l_iCurrLevelData++)
            {
                if (a_strLevelData.Equals(m_lstLevelData[l_iCurrLevelData].LevelName, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_CurrLevelData = m_lstLevelData[l_iCurrLevelData];
                }
            }
            return l_CurrLevelData;
        }

        /// <summary>
        /// Transferring game to level specified
        /// a_strLevelName is in the format "Levelname"+"_"+"LevelID" ex "Level1_100"
        /// </summary>
        /// <param name="a_strLevelName"></param>
        public static void GoToLevel(string a_strLevelName)
        {
            string l_strSceneToLoad = string.Empty;

            if (a_strLevelName.Equals(GameConsts.STATE_NAME_HOME, System.StringComparison.OrdinalIgnoreCase))
            {
                GameManager.GoToHome();
            }
            else
            {
                LevelData l_CurrentLevelData = null;
                SubLevelData l_CurrentSubLevelData = null;

                ///Check if the a_strLevelName is in correct format and can be found in the list of level data and sub level data
                if (!s_Instance.getLevelAndSubLevelDataFromName(a_strLevelName, ref l_CurrentLevelData, ref l_CurrentSubLevelData))
                {
                    return;
                }
                l_strSceneToLoad = l_CurrentLevelData.SceneName;

                ///Loads the assets (Tasklist, ObjectiveList) corresponding to the level name ex. 'TaskListLevel1'
                #region LOAD_ASSETS
                if (!s_Instance.m_strCurrLevelName.Equals(l_CurrentLevelData.LevelName, System.StringComparison.OrdinalIgnoreCase))
                {
                    EventHash l_Hashtable = EventManager.GetEventHashtable();
                    l_Hashtable.Add(GameEventTypeConst.ID_LEVEL_TYPE, l_CurrentLevelData.LevelName);
                    EventManager.Dispatch(GAME_EVENT_TYPE.ON_LEVEL_SELECTED, l_Hashtable);
                }

                s_Instance.m_strCurrLevelName = l_CurrentLevelData.LevelName;
                s_Instance.m_strCurrSubLevelName = l_CurrentSubLevelData.SubLevelName;

                #endregion LOAD_ASSETS

                ///Loads the current level data to the player
                #region LOAD_LEVEL_DATA

                SubLevelData l_SubLevelDataToLoadToPlayer = null;
                switch (l_CurrentSubLevelData.LoadDataType)
                {
                    case SUB_LEVEL_SAVE_LOAD_DATA_TYPE.LOAD_FROM_PREVIOUS_LEVEL:
                        {
                            l_SubLevelDataToLoadToPlayer = (l_CurrentSubLevelData.SubLevelDataIndex != 0) ? l_CurrentLevelData.lstSubLevels[--l_CurrentSubLevelData.SubLevelDataIndex] : l_CurrentSubLevelData;
                            break;
                        }
                    case SUB_LEVEL_SAVE_LOAD_DATA_TYPE.SAVED_LEVEL_DATA:
                        {
                            l_SubLevelDataToLoadToPlayer = l_CurrentSubLevelData;
                            break;
                        }
                }

                PlayerManager.HealthMeter = l_SubLevelDataToLoadToPlayer.m_iPlayerHealth;
                WeaponManager.SetCurrentWeaponInventory(l_SubLevelDataToLoadToPlayer.m_WeaponInventory);

                #endregion LOAD_LEVEL_DATA

                ///Load scene will call the callback directly if already loaded
                GameManager.LoadScene(l_strSceneToLoad, s_Instance.onLevelSceneLoadComplete);
            }

        }

        /// <summary>
        /// On scene load completed transition to the in game state
        /// </summary>
        private void onLevelSceneLoadComplete()
        {
            string l_strGameLevel = m_strCurrLevelName + "_" + m_strCurrSubLevelName;
            GameStateMachine.Transition(l_strGameLevel);

            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_GAME_STATE_ID, l_strGameLevel);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAMEPLAY_BEGIN, l_EventHash);
        }

        /// <summary>
        /// Callback called to event ON_OBJECTIVE_GROUP_COMPLETED
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onLevelObjectiveGroupCompleted(EventHash a_EventHash)
        {
            string l_strLevelNameOfObjectiveCompleted = a_EventHash[GameEventTypeConst.ID_OLD_GAME_STATE].ToString();

            LevelData l_CurrentLevelData = null;
            SubLevelData l_CurrentSubLevelData = null;
            if (!s_Instance.getLevelAndSubLevelDataFromName(l_strLevelNameOfObjectiveCompleted, ref l_CurrentLevelData, ref l_CurrentSubLevelData))
            {
                Debug.LogError("LevelManager::getLevelAndSubLevelDataFromName:: Error retrieving level name :: '" + l_strLevelNameOfObjectiveCompleted + "'");
                return;
            }

            string l_strNextLevelToLoad = string.Empty;

            int l_iSubLevelDataMaxIndex = l_CurrentLevelData.lstSubLevels.Count - 1 ;
            int l_iLevelDataMaxIndex = m_lstLevelData.Count - 1;

            ///if the sublevel completed is not the last sublevel in the level data, go to the next sub level
            ///else go to the 1st sublevel data in the next level
            if ((l_CurrentSubLevelData.SubLevelDataIndex + 1) < l_iSubLevelDataMaxIndex)
            {
                l_strNextLevelToLoad = l_CurrentLevelData.LevelName + "_" + l_CurrentLevelData.lstSubLevels[l_CurrentSubLevelData.SubLevelDataIndex + 1].SubLevelName;
            }
            else
            {
                if (((l_CurrentSubLevelData.SubLevelDataIndex + 1) == l_iSubLevelDataMaxIndex) &&
                        (l_CurrentLevelData.lstSubLevels[l_CurrentSubLevelData.SubLevelDataIndex + 1].LoadDataType == SUB_LEVEL_SAVE_LOAD_DATA_TYPE.LAST_LEVEL_EXIT))
                {
                    l_strNextLevelToLoad = GameConsts.STATE_NAME_HOME;
                }
                else if((l_CurrentSubLevelData.SubLevelDataIndex == l_iSubLevelDataMaxIndex) &&
                        ((l_CurrentLevelData.LevelDataIndex + 1) <= (l_iLevelDataMaxIndex)))
                {
                    LevelData l_NextLevelData = m_lstLevelData[(l_CurrentLevelData.LevelDataIndex + 1)];
                    l_strNextLevelToLoad = l_NextLevelData.LevelName + "_" + l_NextLevelData.lstSubLevels[0].SubLevelName;
                }


                //if ((l_CurrentSubLevelData.SubLevelDataIndex + 1) == l_iSubLevelDataCount &&
                //    m_lstLevelData.Count <= l_CurrentLevelData.LevelDataIndex + 1)
                //{
                //    LevelData l_NextLevelData = m_lstLevelData[(l_CurrentLevelData.LevelDataIndex + 1)];
                //    l_strNextLevelToLoad = l_NextLevelData.LevelName + "_" + l_NextLevelData.lstSubLevels[0].SubLevelName;
                //}
                /////If the last sub level data is a LAST_LEVEL_EXIT then go to HOME
                //else if (((l_CurrentSubLevelData.SubLevelDataIndex + 1) < (l_iSubLevelDataCount - 1)) &&
                //    (l_CurrentLevelData.lstSubLevels[l_CurrentSubLevelData.SubLevelDataIndex + 1].LoadDataType == SUB_LEVEL_SAVE_LOAD_DATA_TYPE.LAST_LEVEL_EXIT))
                //{
                //    l_strNextLevelToLoad = GameConsts.STATE_NAME_HOME;
                //}
            }

            ///TODO:: Save current level progress

            GoToLevel(l_strNextLevelToLoad);
        }

        /// <summary>
        /// Gets the level data and sub level data from the level name ex "Level1_100"
        /// </summary>
        /// <param name="a_strLevelName"></param>
        /// <param name="a_refLevelData"></param>
        /// <param name="a_refSubLevelData"></param>
        /// <returns></returns>
        private bool getLevelAndSubLevelDataFromName(string a_strLevelName, ref LevelData a_refLevelData, ref SubLevelData a_refSubLevelData)
        {
            string[] l_strarr = a_strLevelName.Split('_');

            bool l_bIsLevelNameFormatCorrect = (l_strarr.Length > 1);

            if (l_bIsLevelNameFormatCorrect)
            {
                string l_strCurrentLevelName = l_strarr[0];
                string l_strCurrentSubLevelName = l_strarr[1];

                a_refLevelData = s_Instance.getLevelDataWithName(l_strCurrentLevelName);
                if (a_refLevelData == null)
                {
                    Debug.LogError("LevelManager::GoToLevel:: The current level data with name '" + l_strCurrentLevelName + "' could not be found");
                    l_bIsLevelNameFormatCorrect = false;
                }

                if (l_bIsLevelNameFormatCorrect)
                {
                    a_refSubLevelData = a_refLevelData.getSubLevelData(l_strCurrentSubLevelName);
                    if (a_refSubLevelData == null)
                    {
                        Debug.LogError("LevelManager::GoToLevel:: The current sub level data with name '" + l_strCurrentSubLevelName + "' could not be found");
                        l_bIsLevelNameFormatCorrect = false;
                    }
                }
            }
            else
            {
                Debug.LogError("LevelManager::GoToLevel:: The level name of load is not in correct format, Level Name '" + a_strLevelName + "'");
            }

            return l_bIsLevelNameFormatCorrect;
        }
    }
}