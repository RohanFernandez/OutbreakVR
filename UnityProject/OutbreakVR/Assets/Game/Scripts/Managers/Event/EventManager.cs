using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EventManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static EventManager s_Instance = null;

        /// <summary>
        /// Dictionary of events to delegate
        /// </summary>
        private Dictionary<GAME_EVENT_TYPE, System.Action<Hashtable>> m_dictGameEvents = null;

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
            m_dictGameEvents = new Dictionary<GAME_EVENT_TYPE, System.Action<Hashtable>>(10);
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
            s_Instance = null;
        }

        /// <summary>
        /// Subscribes to the game event
        /// </summary>
        public static void SubscribeTo(GAME_EVENT_TYPE a_GameEventType, System.Action<Hashtable> a_EventCallback)
        {
            if (s_Instance.m_dictGameEvents.ContainsKey(a_GameEventType))
            {
                s_Instance.m_dictGameEvents[a_GameEventType] += a_EventCallback;
            }
            else
            {
                s_Instance.m_dictGameEvents.Add(a_GameEventType, a_EventCallback);
            }
        }

        /// <summary>
        /// Unsubscribes from the game event
        /// </summary>
        public static void UnsubscribeFrom(GAME_EVENT_TYPE a_GameEventType, System.Action<Hashtable> a_EventCallback)
        {
            if (s_Instance.m_dictGameEvents.ContainsKey(a_GameEventType))
            {
                s_Instance.m_dictGameEvents[a_GameEventType] -= a_EventCallback;
            }
        }

        /// <summary>
        /// Fires event subscribed
        /// </summary>
        /// <param name="a_GameEventType"></param>
        /// <param name="a_HashtableArgs"></param>
        public static void Dispatch(GAME_EVENT_TYPE a_GameEventType, Hashtable a_HashtableArgs)
        {
            System.Action<Hashtable> l_Event = null;
            s_Instance.m_dictGameEvents.TryGetValue(a_GameEventType, out l_Event);
            if (l_Event != null)
            {
                l_Event.Invoke(a_HashtableArgs);
            }
        }

        /// <summary>
        /// Returns the number of events subscribed
        /// </summary>
        /// <param name="a_GameEventType"></param>
        /// <returns></returns>
        public static int GetInvocationListCount(GAME_EVENT_TYPE a_GameEventType)
        {
            System.Action<Hashtable> l_Event = null;
            s_Instance.m_dictGameEvents.TryGetValue(a_GameEventType, out l_Event);
            if (l_Event != null)
            {
                return l_Event.GetInvocationList().Length;
            }
            return 0;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Logs events and types of objects and its method name
        /// </summary>
        public static void LogEvents()
        {
            foreach (KeyValuePair < GAME_EVENT_TYPE, System.Action<Hashtable>> l_keyValEvent in s_Instance.m_dictGameEvents)
            {
                System.Action<Hashtable> l_EventList = l_keyValEvent.Value;
                string l_strEventType = l_keyValEvent.Key.ToString();

                if (l_EventList != null)
                {
                    System.Delegate[] l_DelegateArray = l_EventList.GetInvocationList();
                    int l_iDelegateCount = l_DelegateArray.Length;
                    for (int l_iDelegateIndex = 0; l_iDelegateIndex < l_iDelegateCount; l_iDelegateIndex++)
                    {
                        System.Delegate l_CurrentDelegate = l_DelegateArray[l_iDelegateIndex];
                        l_strEventType += "\nObject: "+ l_CurrentDelegate.Target.GetType().Name + ", MethodName: "+ l_CurrentDelegate.Method.Name;
                    }
                }
                Debug.Log(l_strEventType); ;
            }
        }
#endif
    }
}