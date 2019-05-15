using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class Test1 : MonoBehaviour
    {
        public void init()
        {
            EventManager.SubscribeTo(GAME_EVENT_TYPE.TEST1, FirstCallback);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.TEST2, SecondCallback);
        }

        public void destroy()
        {
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.TEST1, FirstCallback);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.TEST2, SecondCallback);
        }

        void FirstCallback(Hashtable a_hash)
        {
            Debug.LogError("Callback1"+ a_hash[GameEventTypeConst.test1Arg]);
        }
        void SecondCallback(Hashtable a_hash)
        {
            Debug.LogError("Callback2" + a_hash[GameEventTypeConst.test2Arg]);
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                Hashtable l_hash = new Hashtable(1);
                l_hash.Add(GameEventTypeConst.test1Arg, "Test1 is working");
                EventManager.Dispatch(GAME_EVENT_TYPE.TEST1, l_hash);
            }

            if (Input.GetKeyUp(KeyCode.B))
            {
                Hashtable l_hash = new Hashtable(1);
                l_hash.Add(GameEventTypeConst.test2Arg, "Test2 is working");
                EventManager.Dispatch(GAME_EVENT_TYPE.TEST2, l_hash);
            }

            if (Input.GetKeyUp(KeyCode.L))
            {
                EventManager.LogEvents();
            }
        }
    }

}