using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyPool : MonoObjectPool<EnemyBase>
    {
        public EnemyPool(EnemyBase a_EnemyPrefab, GameObject a_Parent)
            : base(a_EnemyPrefab, a_Parent)
        {

        }
    }
}