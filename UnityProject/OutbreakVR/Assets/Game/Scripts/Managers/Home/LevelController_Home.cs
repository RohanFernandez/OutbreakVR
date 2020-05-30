﻿using ns_Mashmo;
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
        }

        #region Monitor

        [SerializeField]
        private HomeMonitorScreenFSM m_MonitorFSM = null;

        #endregion Monitor


        #region SFX

        [SerializeField]
        private TMPro.TMP_Text m_txtCurrentSFX = null;

        public void onSFXInteract()
        {
            SoundManager.IsSFXOn = !SoundManager.IsSFXOn;
            setCurrentSFXValue();
        }

        public void setCurrentSFXValue()
        {
            m_txtCurrentSFX.text = SoundManager.IsSFXOn ? "ON" : "OFF";
        }

        #endregion SFX

        #region MUSIC

        [SerializeField]
        private TMPro.TMP_Text m_txtCurrentMusic = null;

        public void onMusicInteract()
        {
            SoundManager.IsMusicOn = !SoundManager.IsMusicOn;
            setCurrentMusicValue();
        }

        public void setCurrentMusicValue()
        {
            m_txtCurrentMusic.text = SoundManager.IsMusicOn ? "ON" : "OFF";
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


    }
}