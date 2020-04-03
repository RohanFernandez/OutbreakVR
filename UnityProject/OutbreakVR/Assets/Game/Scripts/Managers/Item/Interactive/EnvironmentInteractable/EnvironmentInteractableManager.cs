using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnvironmentInteractableManager : MonoBehaviour
    {
        [SerializeField]
        private List<LevelInteractables> m_lstLevelInteractables = null;

        void Awake()
        {
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_STATE_STARTED, onGameStateStarted);
        }

        void OnDestroy()
        {
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAME_STATE_STARTED, onGameStateStarted);
        }

        /// <summary>
        /// Event called on game state started
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onGameStateStarted(EventHash a_EventHash)
        {
            string a_strNewLevelName = a_EventHash[GameEventTypeConst.ID_NEW_GAME_STATE].ToString();
            resetLevelInteractables(a_strNewLevelName);
        }

        /// <summary>
        /// Resets the interactables of level in list
        /// </summary>
        /// <param name="a_strLevelName"></param>
        private void resetLevelInteractables(string a_strLevelName)
        {
            int l_iLevelListCount = m_lstLevelInteractables.Count;
            for (int l_iLevelIndex = 0; l_iLevelIndex < l_iLevelListCount; l_iLevelIndex++)
            {
                if (m_lstLevelInteractables[l_iLevelIndex].LevelName.Equals(a_strLevelName, System.StringComparison.OrdinalIgnoreCase))
                {
                    m_lstLevelInteractables[l_iLevelIndex].resetValues();
                    break;
                }
            }
        }
    }
}