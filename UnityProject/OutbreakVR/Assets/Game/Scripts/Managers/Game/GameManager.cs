using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    //public enum LEVEL_TYPE
    //{
    //    LEVEL1
    //}

    public class GameManager : AbsGroupComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static GameManager s_Instance = null;

        /// <summary>
        /// The current Game level
        /// </summary>
        [SerializeField]
        private string m_strCurrentLevel = string.Empty;

        /// <summary>
        /// Sets the current level as arguement as fires an event if the old event is not the new
        /// </summary>
        /// <param name="a_LevelType"></param>
        public static void SetGameLevel(string a_strLevelType)
        {
            s_Instance.m_strCurrentLevel = a_strLevelType;
            Hashtable l_Hashtable = EventManager.GetHashtable();
            l_Hashtable.Add(GameEventTypeConst.ID_LEVEL_TYPE, a_strLevelType);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_LEVEL_SELECTED, l_Hashtable);
            EventManager.ReturnHashtableToPool(l_Hashtable);
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
            base.destroy();
            s_Instance = null;
        }

        public static void StartCoroutineExecution(IEnumerator a_Enumerator)
        {
            s_Instance.StartCoroutine(a_Enumerator);
        }

        /// <summary>
        /// Displays the load panel
        /// Loads scene name
        /// Hides the load panel
        /// Calls action sent on complete
        /// </summary>
        public static void LoadScene(string a_strSceneName, System.Action a_actionOnLoadComplete = null)
        {
            //Show load panel
            SystemManager.LoadScene(a_strSceneName,
                a_actionOnLoadComplete += () => {
                    /// Add action of hiding loading panel
                } );
        }
    }
}