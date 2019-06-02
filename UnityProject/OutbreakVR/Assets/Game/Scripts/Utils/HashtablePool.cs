using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class HashtablePool : ObjectPool<Hashtable>
    {
        public HashtablePool(int a_iStartSize = 0)
            : base("System.Collections.Hashtable", a_iStartSize)
        {
        }
    }
}