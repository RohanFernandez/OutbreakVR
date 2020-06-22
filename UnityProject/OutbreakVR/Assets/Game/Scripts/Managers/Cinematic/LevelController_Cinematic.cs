using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class LevelController_Cinematic : MonoBehaviour
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

        public void inflictDamagebreakHelmet()
        {
            PlayerManager.InflictDamage(80, DAMAGE_INFLICTION_TYPE.DEFAULT);
        }

        public void endCinematic()
        {
            ObjectiveManager.TriggerObjective(OBJ_TRIGGER_END_CINEMATIC);
        }

        public void showOutbreakLogo()
        {
            UI_LoadingPanel.Show(UI_LoadingPanel.LOADING_PANEL_OUTBREAK_LOGO);
        }

        public void showCinematicText()
        {
            UI_CinematicTextPanel.Show();
        }

        public void hideCinematicText()
        {
            UI_CinematicTextPanel.Hide();
        }

        #endregion CINEMATIC


    }
}