using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class InteractiveSelectiveLocationBase : AbsEnvironmentInteractableObject, IPointerOver
    {
        [SerializeField]
        protected INVENTORY_ITEM_ID m_InventoryItemIDToPlace;

        [SerializeField]
        protected Collider m_Collider = null;

        [SerializeField]
        protected string m_strTriggerSequenceOnInteract = string.Empty;

        [SerializeField]
        protected OutlineHighlighterBase m_OutLineHighlighter = null;

        [SerializeField]
        protected UnpooledAudioSource m_AudSrc = null;

        [SerializeField]
        protected string m_strAudioClipOnInteract = string.Empty;

        public virtual void onPointerEnter()
        {
            if (m_OutLineHighlighter != null && m_Collider.enabled)
            {
                m_OutLineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterSelected);
            }
        }

        public virtual void onPointerExit()
        {
            if (m_OutLineHighlighter != null && m_Collider.enabled)
            {
                m_OutLineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
            }
        }

        public virtual void onPointerInteract()
        {
            if (!string.IsNullOrEmpty(m_strTriggerSequenceOnInteract))
            {
                TaskManager.ExecuteSequence(m_strTriggerSequenceOnInteract);
            }

            if (!string.IsNullOrEmpty(m_strAudioClipOnInteract) && (m_AudSrc != null))
            { 
                m_AudSrc.play(m_strAudioClipOnInteract, false, 1.0f);
            }

            m_Collider.enabled = false;
            m_OutLineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterDeactivated);
        }

        public override void resetValues()
        {
            base.resetValues();
            if (m_AudSrc != null)
            {
                m_AudSrc.stop();
            }
            m_Collider.enabled = true;
            m_OutLineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
        }

        protected bool isInventoryItemUsed()
        {
            return InventoryManager.IsInventoryItemUsed(m_InventoryItemIDToPlace);
        }
    }
}