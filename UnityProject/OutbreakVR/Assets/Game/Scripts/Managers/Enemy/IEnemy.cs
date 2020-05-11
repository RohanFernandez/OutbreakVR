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

        void inflictDamage(int a_iDamage, Vector3 a_v3HitPoint, ENEMY_HIT_COLLISION a_EnemyHitCollision);
    }
}