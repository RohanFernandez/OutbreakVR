using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_LevelSelection : AbsUIComponent
    {
        /// <summary>
        /// On level option selected set go to state
        /// </summary>
        public void onNewLevelOptionSelected()
        {
            UI_LoadingPanel.Show();
            GameManager.OnNewGameSelected();
        }

        /// <summary>
        /// On training option selected set go to state
        /// </summary>
        public void onTrainingOptionSelected()
        {
            UI_LoadingPanel.Show();
            GameManager.OnTrainingLevelSelected();
        }

        /// <summary>
        /// On btn selected to continue the game from the last saved level
        /// </summary>
        public void onContinueLevelOptionSelected()
        {
            UI_LoadingPanel.Show();
            GameManager.OnContinueFromLastSavedSelected();
        }

        /// <summary>
        /// On btn selected to play the first level
        /// </summary>
        public void onFirstLevelOptionSelected()
        {
            UI_LoadingPanel.Show();
            LevelManager.GoToLevel(GameConsts.STATE_NAME_FIRST_LEVEL);
        }
    }
}
