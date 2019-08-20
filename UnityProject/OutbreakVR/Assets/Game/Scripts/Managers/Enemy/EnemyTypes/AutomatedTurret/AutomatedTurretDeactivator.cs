using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class AutomatedTurretDeactivator : EnemyDependantBase, IPointerOver
    {
        /// <summary>
        /// The automated turret that this manages
        /// </summary>
        [SerializeField]
        private AutomatedTurret m_AutomatedTurret = null;

        public override void onActivate()
        {
            base.onActivate();
            m_AutomatedTurret = (AutomatedTurret)EnemyManager.GetActiveEnemyWithID(ENEMY_TYPE.AUTOMATED_TURRET, EnemyID);
        }

        public override void onDeactivate()
        {
            base.onDeactivate();
        }

        public override void onInteract()
        {
            base.onInteract();
            m_AutomatedTurret.onSwitchedOff();
        }

    #region IPointerOver
        public void onPointerEnter()
        {
            
        }

        public void onPointerExit()
        {
            
        }

        public void onPointerInteract()
        {
            onInteract();
            onDeactivate();
        }
    #endregion IPointerOver
    }
}