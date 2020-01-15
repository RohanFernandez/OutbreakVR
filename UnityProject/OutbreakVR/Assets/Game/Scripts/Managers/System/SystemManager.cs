using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class SystemManager : AbsGroupComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static SystemManager s_Instance = null;

        /// <summary>
        /// This object is the initial manager hence initialize is called by awake
        /// </summary>
        void Awake()
        {
            initialize();
        }

        /// <summary>
        /// This object is marked as a dont destroy object, hence OnDestroy will be called only when the game ends
        /// </summary>
        void OnDestroy()
        {
            destroy();
        }

        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            DontDestroyOnLoad(this);
            base.initialize();
        }

        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            base.destroy();
            s_Instance = null;
        }

        /// <summary>
        /// Function called to load any scene.
        /// On scene load complete will call the actionOnSceneLoaded function.
        /// </summary>
        /// <param name="a_strSceneName"></param>
        /// <param name="actionOnSceneLoaded"></param>
        public static void LoadScene(string a_strSceneName, System.Action actionOnSceneLoaded)
        {
            s_Instance.StartCoroutine(s_Instance.loadSceneAsync(a_strSceneName, actionOnSceneLoaded));
        }

        /// <summary>
        /// Async loads the scene.
        /// </summary>
        /// <param name="a_strSceneName"></param>
        /// <param name="actionOnSceneLoaded"></param>
        /// <returns></returns>
        private IEnumerator loadSceneAsync(string a_strSceneName, System.Action actionOnSceneLoaded)
        {
            UnityEngine.SceneManagement.Scene l_OldScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            if (!l_OldScene.name.Equals(a_strSceneName, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("<color=BLUE>SystemManager::loadSceneAsync::</color> Loading scene with name : '"+ a_strSceneName + "'");
                AsyncOperation l_AsyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(a_strSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                while (!l_AsyncOperation.isDone)
                {
                    yield return null;
                }
                EventHash l_hash = EventManager.GetEventHashtable();
                l_hash.Add(GameEventTypeConst.ID_OLD_SCENE_NAME, l_OldScene.name);
                l_hash.Add(GameEventTypeConst.ID_NEW_SCENE_NAME, a_strSceneName);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_SCENE_CHANGED, l_hash);
            }

            if (actionOnSceneLoaded != null)
            {
                actionOnSceneLoaded();
            }
        }

        /// <summary>
        /// Exits the application
        /// </summary>
        public static void ExitApplication()
        {
            UnityEngine.Application.Quit();
        }
    }
}