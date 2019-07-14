using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ManagedAudioSrcPool : MonoObjectPool<PooledAudioSource>
    {
        public ManagedAudioSrcPool(PooledAudioSource a_ManagedAudSrcPrefab, GameObject a_Parent)
            : base(a_ManagedAudSrcPrefab, a_Parent)
        {

        }
    }
}