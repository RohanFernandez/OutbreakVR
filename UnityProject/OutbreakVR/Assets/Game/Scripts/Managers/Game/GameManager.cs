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
        /// the name of the current in game level background ambient audio
        /// </summary>
        private string m_strCurrentLvlAmbientAudio = string.Empty;
        public static string CurrentLvlAmbientAudio
        {
            get { return s_Instance.m_strCurrentLvlAmbientAudio; }
            set {s_Instance.m_strCurrentLvlAmbientAudio = value;}
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
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_COMPLETED, onGameCompleted);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_ENEMY_ALERT_COUNT_CHANGED, onEnemyAlertCountChanged);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_STATE_STARTED, onGameStateStarted);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_STATE_ENDED, onGameStateEnded);

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
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAME_COMPLETED, onGameCompleted);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, onGameplayEnded);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_KILLED, onPlayerKilled);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_ENEMY_ALERT_COUNT_CHANGED, onEnemyAlertCountChanged);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAME_STATE_STARTED, onGameStateStarted);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAME_STATE_ENDED, onGameStateEnded);

            base.destroy();
            s_Instance = null;
        }


        public static void LoadScene(string a_strSceneName, System.Action a_actionOnLoadComplete = null)
        {
            UI_LoadingPanel.Show();
            SystemManager.LoadScene(a_strSceneName,
                a_actionOnLoadComplete += () => {
                    UI_LoadingPanel.Hide();
                });
        }

        /// <summary>
        /// Pause /unpause game game
        /// Is it forced to pause/unpause and its not in the gameplay
        /// </summary>
        /// <param name="a_bIsPaused"></param>
        public static void PauseGame(bool a_bIsPaused, bool a_bIsForce = true)
        {
            if (s_Instance.m_bIsGamePaused == a_bIsPaused)
            {
                return;
            }
            s_Instance.m_bIsGamePaused = a_bIsPaused;

            if (s_Instance.m_bIsGamePaused)
            {
                s_Instance.StartCoroutine(s_Instance.pauseAfterTime());
            }
            else
            {
                Time.timeScale = 1.0f;
                UI_PausePanel.Hide();
            }

            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_GAME_PAUSED, a_bIsPaused);
            l_EventHash.Add(GameEventTypeConst.ID_PAUSE_FORCED, a_bIsForce);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAME_PAUSED_TOGGLED, l_EventHash);
        }

        /// <summary>
        /// Pauses after the given time
        /// Created to give enough time to play the arm animation
        /// </summary>
        /// <returns></returns>
        private IEnumerator pauseAfterTime()
        {
            yield return new WaitForSeconds(0.5f);
            UI_PausePanel.Show();
            Time.timeScale = 0.0f;
        }

        /// <summary>
        /// Event callback on player killed to restart the level
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onPlayerKilled(EventHash a_EventHash)
        {
            UI_ScreenFader.ShowWithActionOnComplete(RestartLevel);
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

            LevelManager.GoToLevel(LevelManager.LastCheckpointLevel);

            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_LEVEL_TYPE, LevelManager.LastCheckpointLevel);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_LEVEL_RESTARTED, l_EventHash);
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

        /// <summary>
        /// On the final level of the game is completed
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onGameCompleted(EventHash a_EventHash)
        {
            // a_EventHash[GameEventTypeConst.ID_GAME_STATE_ID];

            int l_iOldGamesCompleted = PlayerDataManager.GetInt(PLAYER_KEYS._OUTBREAK_GAMES_COMPLETED);

            //Increments the number of times the player has completed the game
            PlayerDataManager.SetInt(PLAYER_KEYS._OUTBREAK_GAMES_COMPLETED, ++l_iOldGamesCompleted);
        }

        /// <summary>
        /// Event callback on the count of enemies that were alerted has changed
        /// Play the ambient audio of the level if count is 0, else play the alert sound
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onEnemyAlertCountChanged(EventHash a_EventHash)
        {
            int l_iOldEnemyAlertCount = (int)a_EventHash[GameEventTypeConst.ID_OLD_ENEMY_ALERT_COUNT];
            int l_iNewEnemyAlertCount = (int)a_EventHash[GameEventTypeConst.ID_NEW_ENEMY_ALERT_COUNT];

            bool l_bIsAmbientAudioChanged = false;
            string l_strAmbientAudioToPlay = string.Empty;
            ///Play alert ambient audio
            if (l_iOldEnemyAlertCount == 0 && l_iNewEnemyAlertCount > 0)
            {
                l_bIsAmbientAudioChanged = true;
                l_strAmbientAudioToPlay = SoundConst.AUD_CLIP_ALERT_AMBIENT;
            }
            ///Play the background ambient audio
            else if (l_iOldEnemyAlertCount > 0 && l_iNewEnemyAlertCount == 0)
            {
                l_bIsAmbientAudioChanged = true;
                l_strAmbientAudioToPlay = CurrentLvlAmbientAudio;
            }

            if (l_bIsAmbientAudioChanged)
            {
                SoundManager.StopAudioSrcWithID(SoundConst.AUD_SRC_AMBIENT);
                SoundManager.PlayAudio(SoundConst.AUD_SRC_AMBIENT, l_strAmbientAudioToPlay, true, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_MUSIC);
            }
        }

        /// <summary>
        /// fire event On game play begin if next state is not home
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onGameStateStarted(EventHash a_EventHash)
        {
            string l_strNextGameState = a_EventHash[GameEventTypeConst.ID_NEW_GAME_STATE].ToString();
            //string l_strOldGameState = a_EventHash[GameEventTypeConst.ID_OLD_GAME_STATE].ToString();

            if (!l_strNextGameState.Equals(GameConsts.STATE_NAME_HOME, System.StringComparison.OrdinalIgnoreCase))
            {
                EventHash l_EventGameplayBegin = EventManager.GetEventHashtable();
                l_EventGameplayBegin.Add(GameEventTypeConst.ID_NEW_GAME_STATE, l_strNextGameState);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAMEPLAY_BEGIN, l_EventGameplayBegin);
            }
        }

        /// <summary>
        /// Game state ended, fire gameplay ended event on home started, returns all reusables back into their pool
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onGameStateEnded(EventHash a_EventHash)
        {
            string l_strNextGameState = a_EventHash[GameEventTypeConst.ID_NEW_GAME_STATE].ToString();
            //string l_strOldGameState = a_EventHash[GameEventTypeConst.ID_OLD_GAME_STATE].ToString();

            if (l_strNextGameState.Equals(GameConsts.STATE_NAME_HOME, System.StringComparison.OrdinalIgnoreCase))
            {
                EventHash l_EventHash = EventManager.GetEventHashtable();
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, l_EventHash);
            }
        }
    }
}