using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class MainPauseControlPanel : MonoBehaviour
    {
        /// <summary>
        /// The list of options in the pause panel
        /// </summary>
        [SerializeField]
        private List<UnityEngine.UI.Button> m_lstBtnOptions = null;

        /// <summary>
        /// The pause panel UI
        /// </summary>
        [SerializeField]
        private PauseStateMachine m_PauseStateMachine = null;

        /// <summary>
        /// Current pause state
        /// </summary>
        [SerializeField]
        private PauseManagedStateBase m_CurrentPauseState = null;

        /// <summary>
        /// The current selected option in the button option panel
        /// </summary>
        private int m_iCurrentSelectedBtnOption = 0;

        /// <summary>
        /// Can move up/down in the main panel options
        /// </summary>
        private bool m_bIsControlInMainPanel = true;
        private bool IsControlInMainPanel
        {
            get { return m_bIsControlInMainPanel; }
            set {
                if (m_bIsControlInMainPanel != value)
                {
                    if (value)
                    {
                        showButtonOptionAsHovered(m_iCurrentSelectedBtnOption);
                    }
                    else
                    {
                        showButtonOptionAsSelected(m_iCurrentSelectedBtnOption);
                    }
                }
                m_bIsControlInMainPanel = value;}
        }

        /// <summary>
        /// Resets the panel with the first button selection
        /// </summary>
        public void resetPanel()
        {
            IsControlInMainPanel = true;
            showButtonOptionAsHovered(0);
        }

        #region IPauseState

        public void onBottomPressed()
        {
            if (IsControlInMainPanel)
            {
                showButtonOptionAsHovered((m_iCurrentSelectedBtnOption + 1) % m_lstBtnOptions.Count);
            }
        }

        public void onLeftPressed()
        {
            if (!IsControlInMainPanel)
            {
                GameManager.PlayClickSound();
                m_CurrentPauseState.onLeftPressed();
            }
        }

        public void onRightPressed()
        {
            if (!IsControlInMainPanel)
            {
                GameManager.PlayClickSound();
                m_CurrentPauseState.onRightPressed();
            }
        }

        public void onSelectPressed()
        {
            if (m_CurrentPauseState != null)
            {
                bool l_bControlBeforeSelection = IsControlInMainPanel;
                IsControlInMainPanel = m_CurrentPauseState.PanelType != PauseManagedStateBase.PANEL_CONTROL_TYPE.CONFIRMATION;

                ///The first time the state is selected
                if ((l_bControlBeforeSelection != IsControlInMainPanel) && !IsControlInMainPanel)
                {
                    GameManager.PlayClickSound();
                    m_CurrentPauseState.onStateSelected();
                }
                //the second time the control is in the state panel
                else
                { 
                    m_CurrentPauseState.onSelectPressed(onReturnControlToMainPanel);
                }

            }
        }

        public void onTopPressed()
        {
            if (IsControlInMainPanel)
            {
                int l_iNewIndex = m_iCurrentSelectedBtnOption - 1;
                if (l_iNewIndex == -1) { l_iNewIndex = m_lstBtnOptions.Count - 1; }

                showButtonOptionAsHovered(l_iNewIndex);
            }
        }

        #endregion IPauseState

        /// <summary>
        /// Returns the control from the paused state to the main panel
        /// </summary>
        private void onReturnControlToMainPanel()
        {
            IsControlInMainPanel = true;
        }

        /// <summary>
        /// Sets the option as hovered
        /// </summary>
        /// <param name="a_iOptionIndex"></param>
        private void showButtonOptionAsHovered(int a_iOptionIndex)
        {
            m_iCurrentSelectedBtnOption = a_iOptionIndex;

            ///Set all buttons as idle
            int l_iBtnCount = m_lstBtnOptions.Count;
            for (int l_iBtnIndex = 0; l_iBtnIndex < l_iBtnCount; l_iBtnIndex++)
            {
                UnityEngine.UI.Button l_btnCurrentOption = m_lstBtnOptions[l_iBtnIndex];
                l_btnCurrentOption.image.sprite = l_iBtnIndex == a_iOptionIndex ? l_btnCurrentOption.spriteState.highlightedSprite : l_btnCurrentOption.spriteState.pressedSprite;
            }

            selectBtnWithCurrentIndex();
        }

        /// <summary>
        /// Sets the option as hovered
        /// </summary>
        /// <param name="a_iOptionIndex"></param>
        private void showButtonOptionAsSelected(int a_iOptionIndex)
        {
            ///Set all buttons as idle
            int l_iBtnCount = m_lstBtnOptions.Count;
            for (int l_iBtnIndex = 0; l_iBtnIndex < l_iBtnCount; l_iBtnIndex++)
            {
                UnityEngine.UI.Button l_btnCurrentOption = m_lstBtnOptions[l_iBtnIndex];
                l_btnCurrentOption.image.sprite = l_iBtnIndex == a_iOptionIndex ? l_btnCurrentOption.spriteState.selectedSprite : l_btnCurrentOption.spriteState.pressedSprite;
            }
        }

        private void selectBtnWithCurrentIndex()
        {
            m_lstBtnOptions[m_iCurrentSelectedBtnOption].onClick.Invoke();
        }

        #region BUTTON OPTIONS

        /// <summary>
        /// On button clicked return to game
        /// </summary>
        public void onBtnClicked_ReturnToGame()
        {
            transitionPauseState(UI_PausePanel.PAUSE_STATE_CONTINUE);
        }

        /// <summary>
        /// On button clicked go to home
        /// </summary>
        public void onBtnClicked_GoToHome()
        {
            transitionPauseState(UI_PausePanel.PAUSE_STATE_GO_HOME);
        }

        /// <summary>
        /// On button clicked go to home
        /// </summary>
        public void onBtnClicked_GoToLastCheckpoint()
        {   
            transitionPauseState(UI_PausePanel.PAUSE_STATE_LAST_CHECKPOINT);
        }

        /// <summary>
        /// On button clicked open instruc
        /// </summary>
        public void onBtnClicked_OpenInstructionPanel()
        {
            transitionPauseState(UI_PausePanel.PAUSE_STATE_INSTRUCTIONS);
        }

        /// <summary>
        /// On button clicked open instruc
        /// </summary>
        public void onBtnClicked_OpenObjectivesPanel()
        {
            transitionPauseState(UI_PausePanel.PAUSE_STATE_OBJECTIVE);
        }

        /// <summary>
        /// transition state machine to the given pause state
        /// </summary>
        /// <param name="a_strPauseState"></param>
        private void transitionPauseState(string a_strPauseState)
        {
            m_PauseStateMachine.transitionToPauseState(a_strPauseState);
            ManagedState l_ManagedState = m_PauseStateMachine.getRegisteredManagedState(a_strPauseState);
            m_CurrentPauseState = (l_ManagedState != null) ? (PauseManagedStateBase)l_ManagedState : null;
        }

        #endregion BUTTON OPTIONS

    }
}