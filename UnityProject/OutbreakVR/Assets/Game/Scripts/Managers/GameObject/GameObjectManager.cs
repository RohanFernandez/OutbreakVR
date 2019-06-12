using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class GameObjectManager : AbsComponentHandler
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static GameObjectManager s_Instace = null;

        /// <summary>
        /// Dictionary of gameobject ID to the gameobject
        /// </summary>
        private Dictionary<string, RegisteredGameObject> m_dictGameObjectIDs = null;

        /// <summary>
        /// Sets the singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instace != null)
            {
                return;
            }
            s_Instace = this;

            m_dictGameObjectIDs = new Dictionary<string, RegisteredGameObject>(10);
        }

        /// <summary>
        /// Sets the singleton instance to null
        /// </summary>
        public override void destroy()
        {
            if (s_Instace != this)
            {
                return;
            }
            s_Instace = null;
        }

        /// <summary>
        /// Gets gameobject that is registered in the dictionary
        /// </summary>
        /// <param name="a_strGameObjectID"></param>
        /// <returns></returns>
        public static GameObject GetGameObjectById(string a_strGameObjectID)
        {
            RegisteredGameObject l_GameObj = null;
            if (!s_Instace.m_dictGameObjectIDs.TryGetValue(a_strGameObjectID, out l_GameObj))
            {
                Debug.Log("<color=ORANGE>GameObjectManager::GetGameObjectById:: </color> Failed to find registered gameobject with ID: " + a_strGameObjectID);
                return null;
            }

            return l_GameObj.gameObject;
        }

        /// <summary>
        /// Registers the gameobject into the dictionary
        /// </summary>
        /// <param name="a_GameObject"></param>
        public static void registerGameObj(string a_strID, RegisteredGameObject a_GameObject)
        {
            if (GetGameObjectById(a_strID) != null)
            {
                Debug.Log("<color = ORANGE> GameObjectManager::registerGameObj:: </color> Cannot register because gameobject with ID '"+ a_strID + "' already exists.");
                return;
            }
            s_Instace.m_dictGameObjectIDs.Add(a_strID, a_GameObject);
        }

        /// <summary>
        /// Unregisters the gameobject into the dictionary
        /// </summary>
        /// <param name="a_GameObject"></param>
        public static void unregisterGameObj(string a_strID)
        {
            if (GetGameObjectById(a_strID) == null)
            {
                Debug.LogError("GameObjectManager::registerGameObj:: Cannot unregister because gameobject is not registered '"+ a_strID + "'");
                return;
            }
            s_Instace.m_dictGameObjectIDs.Remove(a_strID);
        }
    }
}