using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ManagedAudioSrcPool : MonoObjectPool<PooledAudioSource>
    {
        public ManagedAudioSrcPool(PooledAudioSource a_ManagedAudSrcPrefab, GameObject a_Parent, int a_iStartSize = 0)
            : base(a_ManagedAudSrcPrefab, a_Parent, a_iStartSize)
        {

        }
    }
}