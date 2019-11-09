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
        public void onLevelOptionSelected(string a_strGameStateName)
        {
            GameManager.OnNewGameSelected();
        }

        /// <summary>
        /// On btn selected to continue the game from the last saved level
        /// </summary>
        public void onContinueLevelOptionSelected()
        {
            //GameManager.ContinueFromLastSavedState();
        }
    }
}
