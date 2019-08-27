using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyDamageUIPool : MonoObjectPool<UI_EnemyDamageIndicator>
    {
        public EnemyDamageUIPool(UI_EnemyDamageIndicator a_UIEnemyDamageIndicator, GameObject a_Parent)
            : base(a_UIEnemyDamageIndicator, a_Parent)
        {

        }
    }
}