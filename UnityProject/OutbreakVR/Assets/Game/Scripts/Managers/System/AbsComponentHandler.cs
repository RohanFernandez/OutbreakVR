using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class AbsComponentHandler : MonoBehaviour
    {
        public abstract void initialize();
        public abstract void destroy();
    }
}