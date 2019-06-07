using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ManagedAudioSrcPool : MonoObjectPool<ManagedAudioSource>
    {
        public ManagedAudioSrcPool(ManagedAudioSource a_ManagedAudSrcPrefab, GameObject a_Parent)
            : base(a_ManagedAudSrcPrefab, a_Parent)
        {

        }
    }
}