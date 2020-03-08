using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EffectsPool : MonoObjectPool<EffectsBase>
    {
        public EffectsPool(EffectsBase a_EffectPrefab, GameObject a_Parent)
            : base(a_EffectPrefab, a_Parent)
        {

        }
    }
}