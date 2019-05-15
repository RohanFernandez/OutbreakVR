using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class Test2 : MonoBehaviour
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
            Debug.LogError("Test2Callback1"+ a_hash[GameEventTypeConst.test1Arg]);
        }
        void SecondCallback(Hashtable a_hash)
        {
            Debug.LogError("Test2Callback2" + a_hash[GameEventTypeConst.test2Arg]);
        }

    }

}