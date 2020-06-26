using ns_Mashmo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class LevelController_Home : MonoBehaviour
    {
        void Awake()
        {
            m_ContinueCollider.enabled = !string.IsNullOrEmpty(LevelManager.LastCheckpointLevel);
            setCurrentMusicValue();
            setCurrentSFXValue();
        }

        /// <summary>
        /// Text component of the wall clock displaying the time
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtDigitalWallClock = null;

        #region Monitor

        [SerializeField]
        private HomeMonitorScreenFSM m_MonitorFSM = null;

        #endregion Monitor


        #region SFX

        [SerializeField]
        private TMPro.TMP_Text m_txtCurrentSFX = null;

        [SerializeField]
        private Animator m_SFXWalkieAnimator = null;

        private const string ANIM_SFX_BOOL = "IsSFXOn";

        public void onSFXInteract()
        {
            SoundManager.IsSFXOn = !SoundManager.IsSFXOn;
            setCurrentSFXValue();
        }

        public void setCurrentSFXValue()
        {
            m_txtCurrentSFX.text = SoundManager.IsSFXOn ? "ON" : "OFF";
            m_SFXWalkieAnimator.SetBool(ANIM_SFX_BOOL, SoundManager.IsSFXOn);
        }

        #endregion SFX

        #region MUSIC

        [SerializeField]
        private Animator m_MusicRadioAnimator = null;

        [SerializeField]
        private TMPro.TMP_Text m_txtCurrentMusic = null;

        private const string ANIM_MUSIC_BOOL = "IsMusicOn";

        public void onMusicInteract()
        {
            SoundManager.IsMusicOn = !SoundManager.IsMusicOn;
            setCurrentMusicValue();
        }

        public void setCurrentMusicValue()
        {
            m_txtCurrentMusic.text = SoundManager.IsMusicOn ? "ON" : "OFF";
            m_MusicRadioAnimator.SetBool(ANIM_MUSIC_BOOL, SoundManager.IsMusicOn);
        }

        #endregion MUSIC


        #region TRAINING

        public void startTrainingLevel()
        {
            GameManager.OnTrainingLevelSelected();
        }

        #endregion TRAINING

        #region NEW GAME

        public void onNewGameInteract()
        {
            m_MonitorFSM.transitionMonitorScreen(HomeMonitorScreenFSM.NEW_GAME_CONFIRMATION_STATE_CONTROLS);
        }

        public void startNewGameLevel()
        {
            GameManager.OnNewGameSelected();
        }

        #endregion NEW GAME

        #region CONTINUE

        [SerializeField]
        private Collider m_ContinueCollider = null;

        public void onContinueInteract()
        {
            m_MonitorFSM.transitionMonitorScreen(HomeMonitorScreenFSM.CONTINUE_CONFIRMATION_STATE_CONTROLS);
        }

        public void startContinueLastSavedState()
        {
            GameManager.OnContinueFromLastSavedSelected();
        }

        #endregion CONTINUEGAME

        #region EXIT GAME

        public void exitGame()
        {
            SystemManager.ExitApplication();
        }

        #endregion EXIT GAME

        void Update()
        {
            m_txtDigitalWallClock.text = System.DateTime.Now.ToString("HH:mm");
        }

        public void playClickSound()
        {
            GameManager.PlayClickSound();
        }
    }
}