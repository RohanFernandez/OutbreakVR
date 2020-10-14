using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EffectsPool : MonoObjectPool<EffectsBase>
    {
        public EffectsPool(EffectsBase a_EffectPrefab, GameObject a_Parent, int a_iStartSize = 0)
            : base(a_EffectPrefab, a_Parent, a_iStartSize)
        {

        }
    }
}