using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
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
        public static bool IsGamePaused
        {
            get { return s_Instance.m_bIsGamePaused; }
        }

        /// <summary>
        /// Color of highlighted item
        /// </summary>
        [SerializeField]
        private Color m_colOutlineHighlighterNormal;
        public static Color ColOutlineHighlighterNormal
        {
            get { return s_Instance.m_colOutlineHighlighterNormal; }
        }

        /// <summary>
        /// Color of selected item
        /// </summary>
        [SerializeField]
        private Color m_colOutlineHighlighterSelected;
        public static Color ColOutlineHighlighterSelected
        {
            get { return s_Instance.m_colOutlineHighlighterSelected; }
        }

        /// <summary>
        /// Color of deactivated item
        /// </summary>
        [SerializeField]
        private Color m_colOutlineHighlighterDeactivated;
        public static Color ColOutlineHighlighterDeactivated
        {
            get { return s_Instance.m_colOutlineHighlighterDeactivated; }
        }

        /// <summary>
        /// Color of restricted item
        /// </summary>
        [SerializeField]
        private Color m_colOutlineHighlighterRestricted;
        public static Color ColOutlineHighlighterRestricted
        {
            get { return s_Instance.m_colOutlineHighlighterRestricted; }
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
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, onGameplayEnded);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_PLAYER_KILLED, onPlayerKilled);

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
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, onGameplayEnded);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_KILLED, onPlayerKilled);

            base.destroy();
            s_Instance = null;
        }

        /// <summary>
        /// Displays the load panel
        /// Loads scene name
        /// Hides the load panel
        /// Calls action sent on complete
        /// </summary>
        public static void LoadScene(string a_strSceneName, System.Action a_actionOnLoadComplete = null)
        {
            UI_LoadingPanel.Show();
            SystemManager.LoadScene(a_strSceneName,
                a_actionOnLoadComplete += () => {
                    UI_LoadingPanel.Hide();
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

            if (s_Instance.m_bIsGamePaused)
            {
                UI_PausePanel.Show(ObjectiveManager.CurrentObjectiveGroup);
            }
            else
            {
                UI_PausePanel.Hide();
            }
        }

        /// <summary>
        /// Event callback on player killed to restart the level
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onPlayerKilled(EventHash a_EventHash)
        {
            UI_ScreenFader.Show(RestartLevel);
        }

        /// <summary>
        /// Returns all reusable
        /// </summary>
        public static void ReturnAllReusables()
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

            LevelManager.GoToLevel(LevelManager.LastCheckpointLevel);
        }

        /// <summary>
        /// Goes to the home scene and home state
        /// a_bIsGamePlayEnded specifies if the gameplay was ongoing and the player decided to quit and go to the home scene
        /// </summary>
        public static void GoToHome()
        {
            PauseGame(false);
            LevelManager.GoToLevel(GameConsts.STATE_NAME_HOME);
        }

        /// <summary>
        /// Callback called on gameplay ended
        /// Hide pause panel
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onGameplayEnded(EventHash a_EventHash)
        {
            PauseGame(false);
            PlayerManager.SetPlayerState(PLAYER_STATE.NO_INTERACTION);
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        public static void OnNewGameSelected()
        {
            LevelManager.LastCheckpointLevel = string.Empty;
            LevelManager.GoToLevel(LevelManager.LastCheckpointLevel);
        }

        /// <summary>
        /// Start a training level
        /// </summary>
        public static void OnTrainingLevelSelected()
        {
            LevelManager.GoToLevel(GameConsts.STATE_NAME_TRAINING);
        }

        /// <summary>
        /// Continues game from the last checkpoint saved
        /// </summary>
        public static void OnContinueFromLastSavedSelected()
        {
            LevelManager.GoToLevel(LevelManager.LastCheckpointLevel);
        }

        private void Update()
        {
            /// TODO:: Testing INput
            #if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.H))
            {
                GoToHome();
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                RestartLevel();
            }
            #endif
        }

        /// <summary>
        /// Displays a notification panel for a certain amount of time
        /// On time complete, exits the application
        /// </summary>
        /// <param name="a_strTitle"></param>
        /// <param name="a_strMsg"></param>
        public static void ExitGameOnDisplayNotification(string a_strTitle, string a_strMsg)
        {
            s_Instance.StartCoroutine(s_Instance.displayNotificationForGivenTime(SystemConsts.DEFAULT_NOTIFICATION_TIME, a_strTitle, a_strMsg, SystemManager.ExitApplication));
        }

        /// <summary>
        /// Displays notification for given amount of time
        /// </summary>
        private IEnumerator displayNotificationForGivenTime(float a_fTime, string a_strTitle, string a_strMsg, System.Action a_ActionOnComplete)
        {
            UI_NotificationPanel.Show(a_strTitle, a_strMsg);
            yield return new WaitForSeconds(a_fTime);

            if (a_ActionOnComplete != null)
            {
                a_ActionOnComplete();
            }

            UI_NotificationPanel.Hide();
        }
    }
}