﻿using System.Collections;
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

        /// <summary>
        /// The automated turret trigger
        /// </summary>
        [SerializeField]
        private AutomatedTurretTrigger m_AutomatedTurretTrigger = null;

        /// <summary>
        /// The automated turret lever animator
        /// </summary>
        [SerializeField]
        private Animator m_TurretLeverAnimator = null;

        /// <summary>
        /// is the turret activated as soon as it is added
        /// </summary>
        [SerializeField]
        private bool m_bIsTurretActivatedOnStart = false;

        private const string ANIM_TRIG_LEVER_ON         = "TurretLeverOn";
        private const string ANIM_TRIG_LEVER_ON_TO_OFF  = "TurretLeverOnToOff";

        public override void onActivate()
        {
            base.onActivate();
            onDeactivate();
            m_AutomatedTurret = (AutomatedTurret)EnemyManager.GetActiveEnemyWithID(ENEMY_TYPE.AUTOMATED_TURRET, EnemyID);

            m_AutomatedTurretTrigger.onActivate();
            if (m_bIsTurretActivatedOnStart)
            {
                onLeverActivate();
            }
        }

        /// <summary>
        /// Called when the turret laser trigger is triggered
        /// enable the turret lever
        /// Start the turret
        /// </summary>
        public void onLeverActivate()
        {
            if (!m_colTurretSwitch.enabled)
            {
                m_colTurretSwitch.enabled = true;
                m_OutlineHighlighterGrp.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
                m_TurretLeverAnimator.SetTrigger(ANIM_TRIG_LEVER_ON);
            }
        }

        public void activateTurret()
        {
            m_AutomatedTurret.onTurretTriggeredToActivate();
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
                m_AutomatedTurretTrigger.onDeactivate();
                m_AutomatedTurret.onSwitchedOff();
                m_TurretLeverAnimator.SetTrigger(ANIM_TRIG_LEVER_ON_TO_OFF);
                SoundManager.PlayAudio(SoundConst.AUD_SRC_TURRET_LEVER, SoundConst.AUD_CLIP_TURRET_LEVER, false, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_SFX);
            }
        }

    #region IPointerOver
        public void onPointerEnter()
        {
            if (m_colTurretSwitch.enabled && m_AutomatedTurret != null)
            {
                m_OutlineHighlighterGrp.toggleHighlighter(true, GameManager.ColOutlineHighlighterSelected);
            }
        }

        public void onPointerExit()
        {
            if (m_colTurretSwitch.enabled && m_AutomatedTurret != null)
            {
                m_OutlineHighlighterGrp.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
            }
        }

        public void onPointerInteract()
        {
            onInteract();
            onDeactivate();
        }
    #endregion IPointerOver
    }
}