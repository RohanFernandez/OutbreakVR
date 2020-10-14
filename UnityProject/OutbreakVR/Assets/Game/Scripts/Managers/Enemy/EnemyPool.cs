using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyPool : MonoObjectPool<EnemyBase>
    {
        public EnemyPool(EnemyBase a_EnemyPrefab, GameObject a_Parent, int a_iStartSize = 0)
            : base(a_EnemyPrefab, a_Parent, a_iStartSize)
        {

        }
    }
}