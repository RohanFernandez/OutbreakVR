using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EventHashPool : ObjectPool<EventHash>
    {
        public EventHashPool(int a_iStartSize = 0)
            : base(typeof(EventHash).ToString(), a_iStartSize)
        {
        }
    }
}