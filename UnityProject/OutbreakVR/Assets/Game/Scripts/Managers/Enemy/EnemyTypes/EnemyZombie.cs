using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyZombie : MeleeAttackEnemy
    {
        /// <summary>
        /// On enemy state changed
        /// </summary>
        protected override void onStateChanged(ENEMY_STATE l_OldNavState, ENEMY_STATE a_NavState)
        {
            base.onStateChanged(l_OldNavState, a_NavState);
            switch (a_NavState)
            {
                case ENEMY_STATE.IDLE:
                    {
                        // Randomly set the idle animation with the ration of 8 : 2, ANIM_TRIGGER_IDLE  : ANIM_TRIGGER_IDLE_AGONY
                        m_Animator.SetTrigger((UnityEngine.Random.Range(1, 11) < 9) ? ANIM_TRIGGER_IDLE : ANIM_TRIGGER_IDLE_AGONY);
                        break;
                    }
            }
        }
    }
}