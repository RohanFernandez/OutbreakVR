using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class StaticEnemy : EnemyBase
    {
        public override ENEMY_ATTACK_TYPE getEnemyAttackType()
        {
            return ENEMY_ATTACK_TYPE.STATIC;
        }

        /// <summary>
        /// Updates movements
        /// </summary>
        public override void Update()
        {
            base.Update();
        }

        public override void activateEnemy()
        {
            base.activateEnemy();
        }
    }
}