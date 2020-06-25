using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ns_Mashmo
{
    public class OnInteractEnvironmentTriggerEvent : AbsEnvironmentInteractableObject, IEnvironmentTrigger
    {
        [SerializeField]
        private OutlineHighlighterBase m_OutLineHighlighter = null;

        [SerializeField]
        private string m_strTriggerSequenceOnInteract = string.Empty;

        [SerializeField]
        private string m_strObjectiveTrigger = string.Empty;

        [SerializeField]
        private UnityEvent m_EventOnInteract = null;

        [SerializeField]
        private UnityEvent m_EventOnEnable = null;

        [SerializeField]
        private Animation m_AnimOnInteract = null;

        [SerializeField]
        protected Collider m_Collider = null;

        public void onObjectHit()
        {
        }

        private void OnEnable()
        {
            if (m_OutLineHighlighter != null)
            {
                m_OutLineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);

                if (m_EventOnEnable != null)
                {
                    m_EventOnEnable.Invoke();
                }
            }
        }

        private void OnDisable()
        {
            if (m_OutLineHighlighter != null)
            {
                m_OutLineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterDeactivated);
            }
        }

        public void onPointerEnter()
        {
            if (m_OutLineHighlighter != null && gameObject.activeSelf && m_Collider.enabled)
            {
                m_OutLineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterSelected);
            }
        }

        public void onPointerExit()
        {
            if (m_OutLineHighlighter != null && gameObject.activeSelf && m_Collider.enabled)
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

            if (!string.IsNullOrEmpty(m_strObjectiveTrigger))
            {
                ObjectiveManager.TriggerObjective(m_strObjectiveTrigger);
            }

            if (m_OutLineHighlighter != null)
            {
                m_OutLineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterDeactivated);
            }

            if (m_EventOnInteract != null)
            {
                m_EventOnInteract.Invoke();
            }

            if (m_AnimOnInteract != null)
            {
                m_AnimOnInteract.Play();
            }
        }
    }
}