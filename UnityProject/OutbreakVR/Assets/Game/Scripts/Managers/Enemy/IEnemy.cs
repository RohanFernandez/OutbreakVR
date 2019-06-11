using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IEnemy : IReusable
    {
        ENEMY_TYPE getEnemyType();

        ENEMY_ATTACK_TYPE getEnemyAttackType();

        void activateEnemy();
        void deactivateEnemy();
    }
}