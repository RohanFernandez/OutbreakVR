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

        /// <summary>
        /// The turret switch to interact with
        /// </summary>
        [SerializeField]
        private Collider m_colTurretSwitch = null;

        /// <summary>
        /// Group of all highlighters for the turret lever
        /// </summary>
        [SerializeField]
        private OutlineHighlighterGroup m_OutlineHighlighterGrp = null;

        public override void onActivate()
        {
            base.onActivate();
            m_AutomatedTurret = (AutomatedTurret)EnemyManager.GetActiveEnemyWithID(ENEMY_TYPE.AUTOMATED_TURRET, EnemyID);
            m_colTurretSwitch.enabled = true;
            m_OutlineHighlighterGrp.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
        }

        public override void onDeactivate()
        {
            base.onDeactivate();
            m_colTurretSwitch.enabled = false;
            m_OutlineHighlighterGrp.toggleHighlighter(false, GameManager.ColOutlineHighlighterDeactivated);
        }

        public override void onInteract()
        {
            base.onInteract();
            if (m_AutomatedTurret != null)
            {
                m_AutomatedTurret.onSwitchedOff();
            }
        }

    #region IPointerOver
        public void onPointerEnter()
        {
            m_OutlineHighlighterGrp.toggleHighlighter(true, GameManager.ColOutlineHighlighterSelected);
        }

        public void onPointerExit()
        {
            m_OutlineHighlighterGrp.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
        }

        public void onPointerInteract()
        {
            Debug.LogError("INTERACTION");
            onInteract();
            onDeactivate();
        }
    #endregion IPointerOver
    }
}