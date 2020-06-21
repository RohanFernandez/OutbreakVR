using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class LevelController_Cinematic1 : MonoBehaviour
    {
        #region CINEMATIC

        private const string OBJ_TRIGGER_END_CINEMATIC = "TriggerEndCinematic";

        public void showLoadingQuoteUI()
        {
            UI_LoadingPanel.Show(UI_LoadingPanel.LOADING_PANEL_QUOTE);
        }

        public void showFadeToWhite()
        {
            UI_LoadingPanel.Hide();
            UI_ScreenFader.ShowFadeToBlack(false);
        }

        public void showFadeToBlack()
        {
            UI_ScreenFader.ShowFadeToBlack(true);
        }

        public void endCinematic()
        {
            ObjectiveManager.TriggerObjective(OBJ_TRIGGER_END_CINEMATIC);
        }

        #endregion CINEMATIC


    }
}