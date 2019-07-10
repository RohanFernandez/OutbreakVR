using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IComponentHandler
    {
        void initialize();
        void destroy();
    }
}