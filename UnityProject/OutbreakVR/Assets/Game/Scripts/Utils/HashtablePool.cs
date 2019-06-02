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

        /// <summary>
        /// Creates object of type T and pushes into the pool
        /// </summary>
        //public override void createObj()
        //{
        //    Hashtable l_Createdhashtable = new Hashtable(4);
        //    m_Pool.Push(l_Createdhashtable);
        //}
    }
}