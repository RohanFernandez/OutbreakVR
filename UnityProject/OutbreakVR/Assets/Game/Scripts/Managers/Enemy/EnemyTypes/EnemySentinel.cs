using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemySentinel : MeleeAttackEnemy
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
                        m_Animator.SetTrigger(ANIM_TRIGGER_IDLE);
                        break;
                    }
            }
        }
    }
}