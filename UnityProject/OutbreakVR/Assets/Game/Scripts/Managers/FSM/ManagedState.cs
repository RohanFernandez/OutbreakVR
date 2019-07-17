using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class ManagedState : MonoBehaviour
    {
        [SerializeField]
        private string m_strStateID = string.Empty;

        /// <summary>
        /// Starts loading the scene if true on state enter
        /// </summary>
        [SerializeField]
        protected bool m_bIsSceneState = false;

        /// <summary>
        /// Starts saving the game if true on state enter
        /// </summary>
        [SerializeField]
        protected bool m_bIsSaveState = false;

        public string StateID
        {
            get { return m_strStateID; }
        }

        /// <summary>
        /// Returns the scene name to load
        /// </summary>
        public virtual string getSceneName
        {
            get { return string.Empty; }
        }

        public virtual void onStateEnter(string a_strOldState)
        {
            Debug.Log("<color=BLUE> ManagedState::onStateEnter :: </color> Entered: " + m_strStateID + "   , Exited : " + a_strOldState);
            if (m_bIsSceneState)
            {
                loadScene();
            }
        }

        protected virtual void loadScene()
        {
            GameManager.LoadScene(getSceneName, onSceneLoadComplete);
        }

        protected virtual void onSceneLoadComplete()
        {

        }

        public virtual void onStateExit(string a_strNewState)
        {
            Debug.Log("<color=BLUE> ManagedState::onStateExit :: </color> Entered: " + a_strNewState + "   , Old State : " + m_strStateID );
        }
    }
}