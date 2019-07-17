﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    //public enum LEVEL_TYPE
    //{
    //    LEVEL1
    //}

    public class GameManager : AbsGroupComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static GameManager s_Instance = null;

        /// <summary>
        /// The current Game level
        /// </summary>
        [SerializeField]
        private string m_strCurrentLevel = string.Empty;
        public static string CurrentLevel
        {
            get { return s_Instance.m_strCurrentLevel; }
        }

        /// <summary>
        /// Current In game state
        /// </summary>
        [SerializeField]
        private string m_strInGameState = string.Empty;
        public static string InGameState
        {
            get { return s_Instance.m_strInGameState; }
        }

        /// <summary>
        /// Is the game pause currently
        /// </summary>
        [SerializeField]
        private bool m_bIsGamePaused = false;

        /// <summary>
        /// Sets the current level as arguement as fires an event if the old event is not the new
        /// </summary>
        /// <param name="a_LevelType"></param>
        private void setGameLevel(string a_strLevelType)
        {
            Debug.Log("<color=BLUE>GameManager::setGameLevel::</color> Setting level type '" + a_strLevelType + "'");
            s_Instance.m_strCurrentLevel = a_strLevelType;
            EventHash l_Hashtable = EventManager.GetEventHashtable();
            l_Hashtable.Add(GameEventTypeConst.ID_LEVEL_TYPE, a_strLevelType);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_LEVEL_SELECTED, l_Hashtable);
        }

        /// <summary>
        /// Sets game state to play
        /// </summary>
        public static void SetGameState(string a_strGameState)
        {
            s_Instance.m_strInGameState = a_strGameState;
            string[] l_strarr = a_strGameState.Split('_');

            s_Instance.setGameLevel(l_strarr[0]);
            GameStateMachine.Transition(s_Instance.m_strCurrentLevel);
        }

        /// <summary>
        /// Sets singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            base.initialize();
        }

        /// <summary>
        /// Destroys singleton instance
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            base.destroy();

            s_Instance = null;
        }

        //public static void StartCoroutineExecution(IEnumerator a_Enumerator)
        //{
        //    s_Instance.StartCoroutine(a_Enumerator);
        //}

        /// <summary>
        /// Displays the load panel
        /// Loads scene name
        /// Hides the load panel
        /// Calls action sent on complete
        /// </summary>
        public static void LoadScene(string a_strSceneName, System.Action a_actionOnLoadComplete = null)
        {
            //Show load panel
            SystemManager.LoadScene(a_strSceneName,
                a_actionOnLoadComplete += () => {
                    /// Add action of hiding loading panel
                } );
        }

        /// <summary>
        /// Pause /unpause game game
        /// </summary>
        /// <param name="a_bIsPaused"></param>
        public static void PauseGame(bool a_bIsPaused)
        {
            if (s_Instance.m_bIsGamePaused == a_bIsPaused)
            {
                return;
            }
            s_Instance.m_bIsGamePaused = a_bIsPaused;

            Time.timeScale = a_bIsPaused ? 0.0f : 1.0f;
            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_GAME_PAUSED, a_bIsPaused);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAME_PAUSED_TOGGLED, l_EventHash);
        }

        /// <summary>
        /// Returns all reusable
        /// </summary>
        private static void ReturnAllReusables()
        {
            EnemyManager.ReturnAllToPool();
            ItemDropManager.ReturnAllToPool();
        }

        /// <summary>
        /// Callback called on level restarted
        /// </summary>
        /// <param name="a_EventHash"></param>
        public static void RestartLevel()
        {
            PauseGame(false);
            EventHash l_EventHash = EventManager.GetEventHashtable();
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, l_EventHash);

            GameStateMachine.Transition(GameStateMachine.GetCurrentState());
        }

        /// <summary>
        /// Goes to the home scene
        /// </summary>
        public static void GoToHome()
        {
            PauseGame(false);
            EventHash l_EventHash = EventManager.GetEventHashtable();
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, l_EventHash);

            GameStateMachine.Transition(GameConsts.STATE_NAME_HOME);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.H))
            {
                GoToHome();
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                RestartLevel();
            }
        }
    }
}