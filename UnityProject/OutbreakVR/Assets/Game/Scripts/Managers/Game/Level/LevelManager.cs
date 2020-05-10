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
        /// The name of the last checkpoint in the game
        /// </summary>
        [SerializeField]
        private string m_strLastCheckpointLevel = string.Empty;
        public static string LastCheckpointLevel
        {
            get {return s_Instance.m_strLastCheckpointLevel; }
            set {
                s_Instance.m_strLastCheckpointLevel = value;
                PlayerDataManager.SetString(PLAYER_KEYS._OUTBREAK_CHECKPOINT_LEVEL, s_Instance.m_strLastCheckpointLevel);
            }
        }

        /// <summary>
        /// The list of all levels in the game
        /// </summary>
        [SerializeField]
        private List<LevelData> m_lstLevelData = null;

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
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_STATE_STARTED, onGameStateStarted);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_STATE_ENDED, onStateExited);

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

            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAME_STATE_STARTED, onGameStateStarted);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_OBJECTIVE_GROUP_COMPLETED, onLevelObjectiveGroupCompleted);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAME_STATE_ENDED, onStateExited);

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
        public static void GoToLevel(string a_strGameLevelName)
        {
            string l_strSceneToLoad = string.Empty;
            bool l_bIsNewLevelToBeLoaded = false;
            ///If an empty or null level is set to load then start new game
            if (string.IsNullOrEmpty(a_strGameLevelName))
            {
                a_strGameLevelName = GameConsts.STATE_NAME_NEW_GAME;
            }

            LevelData l_CurrentLevelData = null;
            SubLevelData l_CurrentSubLevelData = null;

            ///Check if the a_strLevelName is in correct format and can be found in the list of level data and sub level data
            if (!s_Instance.getLevelAndSubLevelDataFromName(a_strGameLevelName, ref l_CurrentLevelData, ref l_CurrentSubLevelData))
            {
                return;
            }
            l_strSceneToLoad = l_CurrentLevelData.SceneName;

            ///Loads the assets (Tasklist, ObjectiveList) corresponding to the level name ex. 'TaskListLevel1'
            #region LOAD_ASSETS
            l_bIsNewLevelToBeLoaded = !s_Instance.m_strCurrLevelName.Equals(l_CurrentLevelData.LevelName, System.StringComparison.OrdinalIgnoreCase);

            s_Instance.m_strCurrLevelName = l_CurrentLevelData.LevelName;
            s_Instance.m_strCurrSubLevelName = l_CurrentSubLevelData.SubLevelName;

            #endregion LOAD_ASSETS

            ///Loads the current level data to the player
            #region LOAD_LEVEL_DATA
                
            PlayerManager.HealthMeter = l_CurrentSubLevelData.SavedData.m_iPlayerHealth;
            WeaponManager.SetCurrentWeaponInventory(l_CurrentSubLevelData.SavedData.m_WeaponInventory);
            InventoryManager.SetInventoryDataAsCurrent(l_CurrentSubLevelData.SavedData.m_ItemInventory);

            #endregion LOAD_LEVEL_DATA

            GameStateMachine.Transition(a_strGameLevelName, l_strSceneToLoad, l_bIsNewLevelToBeLoaded ? s_Instance.m_strCurrLevelName : string.Empty);
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

            int l_iSubLevelDataMaxIndex = l_CurrentLevelData.LstSubLevels.Count - 1 ;
            int l_iLevelDataMaxIndex = m_lstLevelData.Count - 1;
            bool l_bIsNextLevelToLoadInGameplay = false;

            ///if the sublevel completed is not the last sublevel in the level data, go to the next sub level
            ///else go to the 1st sublevel data in the next level
            if ((l_CurrentSubLevelData.SubLevelDataIndex + 1) < l_iSubLevelDataMaxIndex)
            {
                l_bIsNextLevelToLoadInGameplay = true;
                l_strNextLevelToLoad = l_CurrentLevelData.LevelName + "_" + l_CurrentLevelData.LstSubLevels[l_CurrentSubLevelData.SubLevelDataIndex + 1].SubLevelName;
            }
            else
            {
                if (((l_CurrentSubLevelData.SubLevelDataIndex + 1) == l_iSubLevelDataMaxIndex) &&
                        (l_CurrentLevelData.LstSubLevels[l_CurrentSubLevelData.SubLevelDataIndex + 1].LoadDataType == SUB_LEVEL_SAVE_LOAD_DATA_TYPE.LAST_LEVEL_EXIT))
                {
                    l_strNextLevelToLoad = GameConsts.STATE_NAME_HOME;

                    string l_strSubLevelInLevelName = l_CurrentLevelData.LevelName + "_" + l_CurrentLevelData.LstSubLevels[l_CurrentSubLevelData.SubLevelDataIndex + 1].SubLevelName;

                    ///Checks if it is the last level in the game, if true, the dispatch ON_GAME_COMPLETED_EVENT
                    if (l_strSubLevelInLevelName.Equals(GameConsts.STATE_NAME_LAST_LEVEL, System.StringComparison.OrdinalIgnoreCase))
                    {
                        LastCheckpointLevel = string.Empty;

                        EventHash l_EventHash = EventManager.GetEventHashtable();
                        l_EventHash.Add(GameEventTypeConst.ID_GAME_STATE_ID, l_strSubLevelInLevelName);
                        EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAME_COMPLETED, l_EventHash);
                    }
                }
                else if ((l_CurrentSubLevelData.SubLevelDataIndex == l_iSubLevelDataMaxIndex) &&
                        ((l_CurrentLevelData.LevelDataIndex + 1) <= (l_iLevelDataMaxIndex)))
                {
                    l_bIsNextLevelToLoadInGameplay = true;
                    LevelData l_NextLevelData = m_lstLevelData[(l_CurrentLevelData.LevelDataIndex + 1)];
                    l_strNextLevelToLoad = l_NextLevelData.LevelName + "_" + l_NextLevelData.LstSubLevels[0].SubLevelName;
                }
                else
                {
                    Debug.LogError("LevelManager::getLevelAndSubLevelDataFromName:: UNDEFINED ");
                }
            }

            ///Save current level progress
            if (l_bIsNextLevelToLoadInGameplay)
            {
                LevelData l_LevelDataToSave = null;
                SubLevelData l_SubLevelDataToSave = null;
                if (getLevelAndSubLevelDataFromName(l_strNextLevelToLoad, ref l_LevelDataToSave, ref l_SubLevelDataToSave))
                {
                    //Save current player data and weapon data to next level data, so that when next level starts it uses the previous level on end data
                    //Only happens if the next level is not PRE_REGISTERED_LEVEL_DATA which means that the next level should start with set data
                    if (l_SubLevelDataToSave.LoadDataType == SUB_LEVEL_SAVE_LOAD_DATA_TYPE.LOAD_FROM_PREVIOUS_LEVEL ||
                        l_SubLevelDataToSave.LoadDataType == SUB_LEVEL_SAVE_LOAD_DATA_TYPE.LAST_LEVEL_EXIT)
                    {
                        //Save Weapon Data
                        WeaponManager.RetrieveWeaponInfo(l_SubLevelDataToSave.SavedData.m_WeaponInventory.m_MeleeWeaponInfo);
                        WeaponManager.RetrieveWeaponInfo(l_SubLevelDataToSave.SavedData.m_WeaponInventory.m_PrimaryWeaponInfo);
                        WeaponManager.RetrieveWeaponInfo(l_SubLevelDataToSave.SavedData.m_WeaponInventory.m_SecondaryWeaponInfo);

                        l_SubLevelDataToSave.SavedData.m_WeaponInventory.m_CurrentWeaponCateogoryType = WeaponManager.CurrentWeaponCategoryType;

                        //Save Player Data
                        l_SubLevelDataToSave.SavedData.m_iPlayerHealth = PlayerManager.HealthMeter;
                        //l_SubLevelDataToSave.SavedData.m_v3PlayerPosition = PlayerManager.GetPosition();

                        //Save Inventory Data
                        InventoryManager.SetInventoryInfo(ref l_SubLevelDataToSave.SavedData.m_ItemInventory);
                    }

                    if (l_SubLevelDataToSave.IsCheckpoint)
                    {
                        if (l_SubLevelDataToSave.SubLevelSaveEntryKeyType == PLAYER_KEYS._OUTBREAK_NONE
                            && l_SubLevelDataToSave.LoadDataType != SUB_LEVEL_SAVE_LOAD_DATA_TYPE.PRE_REGISTERED_LEVEL_DATA)
                        {
                            Debug.LogError("LevelManager::onLevelObjectiveGroupCompleted:: Checkpoint level '" + l_strNextLevelToLoad + "' does not have a valid Save Entry Key type.");
                        }
                        else
                        {
                            LastCheckpointLevel = l_strNextLevelToLoad;
                            if (l_SubLevelDataToSave.SubLevelSaveEntryKeyType != PLAYER_KEYS._OUTBREAK_NONE)
                            {
                                saveToPlayerPrefs(l_SubLevelDataToSave);
                            }
                        }
                    }
                }
            }

            GoToLevel(l_strNextLevelToLoad);
        }

        public static bool GetLevelAndSubLevelDataFromName(string a_strLevelName, ref LevelData a_refLevelData, ref SubLevelData a_refSubLevelData)
        {
            return s_Instance.getLevelAndSubLevelDataFromName(a_strLevelName, ref a_refLevelData, ref a_refSubLevelData);
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

            return l_bIsLevelNameFormatCorrect;
        }

        /// <summary>
        /// Loads the level and sub level data from the player prefs data
        /// </summary>
        public static void LoadLevelDataFromPlayerPrefs()
        {
            LastCheckpointLevel = PlayerDataManager.GetString(PLAYER_KEYS._OUTBREAK_CHECKPOINT_LEVEL);

            int l_iLevelCount = s_Instance.m_lstLevelData.Count;
            for (int l_iLevelIndex = 0; l_iLevelIndex < l_iLevelCount; l_iLevelIndex++)
            {
                LevelData l_CurrentLevelData = s_Instance.m_lstLevelData[l_iLevelIndex];
                int l_iSubLevelCount = l_CurrentLevelData.LstSubLevels.Count;
                for (int l_iSubLevelIndex = 0; l_iSubLevelIndex < l_iSubLevelCount; l_iSubLevelIndex++)
                {
                    SubLevelData l_SubLevelData = l_CurrentLevelData.LstSubLevels[l_iSubLevelIndex];
                    if (l_SubLevelData.IsCheckpoint && (l_SubLevelData.LoadDataType == SUB_LEVEL_SAVE_LOAD_DATA_TYPE.LOAD_FROM_PREVIOUS_LEVEL ||
                        l_SubLevelData.LoadDataType == SUB_LEVEL_SAVE_LOAD_DATA_TYPE.LAST_LEVEL_EXIT))
                    {
                        string l_strSavedData = PlayerDataManager.GetString(l_SubLevelData.SubLevelSaveEntryKeyType);
                        if (!string.IsNullOrEmpty(l_strSavedData))
                        {
                            SubLevelSavedData l_SubLevelSavedData = JsonUtility.FromJson<SubLevelSavedData>(l_strSavedData);

                            if (l_SubLevelSavedData != null)
                            {
                                l_CurrentLevelData.LstSubLevels[l_iSubLevelIndex].SavedData = l_SubLevelSavedData;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves sub level saved data to player prefs
        /// </summary>
        /// <param name="a_SubLevelData"></param>
        private void saveToPlayerPrefs(SubLevelData a_SubLevelData)
        {
            string l_strSubLevelJson = JsonUtility.ToJson(a_SubLevelData.SavedData);
            PlayerDataManager.SetString(a_SubLevelData.SubLevelSaveEntryKeyType, l_strSubLevelJson);
        }

        /// <summary>
        /// Callback called on game state started
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onGameStateStarted(EventHash a_EventHash)
        {
            LevelData l_CurrentLevelData = null;
            SubLevelData l_CurrentSubLevelData = null;

            string l_strCurrenstState = a_EventHash[GameEventTypeConst.ID_NEW_GAME_STATE].ToString();
            if (getLevelAndSubLevelDataFromName(l_strCurrenstState, ref l_CurrentLevelData, ref l_CurrentSubLevelData))
            {
                string l_strOldGameState = a_EventHash[GameEventTypeConst.ID_OLD_GAME_STATE].ToString();
                LevelData l_OldLevelData = null;
                SubLevelData l_OldSubLevelData = null;
                bool l_bIsLastLevelExist = getLevelAndSubLevelDataFromName(l_strOldGameState, ref l_OldLevelData, ref l_OldSubLevelData);

                //set player position if the current level and prev level are not the same, and the current sub level index is not the old sub level index + 1
                if (l_CurrentSubLevelData.UsePlayerPosition || 
                    !(l_bIsLastLevelExist && (l_OldLevelData.LevelDataIndex == l_CurrentLevelData.LevelDataIndex) && l_OldSubLevelData.SubLevelDataIndex == (l_CurrentSubLevelData.SubLevelDataIndex - 1)))
                {
                    PlayerManager.SetPosition(l_CurrentSubLevelData.m_v3PlayerPosition);
                }

                PlayerManager.SetPlayerState(l_CurrentSubLevelData.PlayerStateOnStart);

                #region MANAGING AMBIENT AUDIO

                    ///Continue to play the last audio that was already set before, so dont do anything
                    if (l_CurrentSubLevelData.AmbientAudPlayCriteria == SubLevelData.AUDIO_PLAY_CRITERIA.CONTINUE_LAST_AUDIO)
                    {
                        //let the audio continue and dont change anything
                    }
                    ///if the next level audio id is the same as the current playing id, then dont do anything and let that audio continue to play
                    ///else stop the audio source and play the next level audio id
                    else if (l_CurrentSubLevelData.AmbientAudPlayCriteria == SubLevelData.AUDIO_PLAY_CRITERIA.CONTINUE_LAST_AUDIO_IF_SAME)
                    {
                        if (!GameManager.CurrentLvlAmbientAudio.Equals(l_CurrentSubLevelData.LvlSpecificAmbientAudioClipID, System.StringComparison.OrdinalIgnoreCase))
                        {
                            GameManager.CurrentLvlAmbientAudio = l_CurrentSubLevelData.LvlSpecificAmbientAudioClipID;
                            SoundManager.StopAudioSrcWithID(GameConsts.AUD_SRC_AMBIENT);
                            SoundManager.PlayAudio(GameConsts.AUD_SRC_AMBIENT, l_CurrentSubLevelData.LvlSpecificAmbientAudioClipID, true, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_MUSIC);
                        }
                    }
                    else if (l_CurrentSubLevelData.AmbientAudPlayCriteria == SubLevelData.AUDIO_PLAY_CRITERIA.DONT_PLAY_AUDIO)
                    {
                        GameManager.CurrentLvlAmbientAudio = string.Empty;
                        SoundManager.StopAudioSrcWithID(GameConsts.AUD_SRC_AMBIENT);
                    }
                    else if (l_CurrentSubLevelData.AmbientAudPlayCriteria == SubLevelData.AUDIO_PLAY_CRITERIA.PLAY_NEW_AUDIO)
                    {
                        GameManager.CurrentLvlAmbientAudio = l_CurrentSubLevelData.LvlSpecificAmbientAudioClipID;
                        SoundManager.StopAudioSrcWithID(GameConsts.AUD_SRC_AMBIENT);
                        SoundManager.PlayAudio(GameConsts.AUD_SRC_AMBIENT, l_CurrentSubLevelData.LvlSpecificAmbientAudioClipID, true, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_MUSIC);
                    }

                #endregion MANAGING AMBIENT AUDIO
            }
        }

        /// <summary>
        /// On game state exited
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onStateExited(EventHash a_EventHash)
        {
            LevelData l_CurrentLevelData = null;
            SubLevelData l_CurrentSubLevelData = null;

            string l_strGameState = (string)a_EventHash[GameEventTypeConst.ID_NEW_GAME_STATE];

            if (s_Instance.getLevelAndSubLevelDataFromName(l_strGameState, ref l_CurrentLevelData, ref l_CurrentSubLevelData))
            {
                if (l_CurrentSubLevelData.IsCheckpoint)
                {
                    GameManager.ReturnAllReusables();
                }
            }
        }
    }
}