using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IReusable
    {
        void onReturnedToPool();
        void onRetrievedFromPool();
    }
}
