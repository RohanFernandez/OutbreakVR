using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class HomeInteractableItem : MonoBehaviour, IPointerOver
    {
        [SerializeField]
        private UnityEngine.Events.UnityEvent m_OnPointerEnter = null;

        [SerializeField]
        private UnityEngine.Events.UnityEvent m_OnPointerExit = null;

        [SerializeField]
        private UnityEngine.Events.UnityEvent m_OnPointerInteract = null;

        [SerializeField]
        private UnityEngine.Events.UnityEvent m_OnEnable = null;

        [SerializeField]
        private Color m_OnPointerEnterColor;

        [SerializeField]
        private Color m_OnPointerExitColor;

        [SerializeField]
        private Color m_OnPointerInteractColor;

        [SerializeField]
        private Color m_StartColor;

        [SerializeField]
        private OutlineHighlighterBase m_OutlineHighlighter;

        void OnEnable()
        {
            if (m_OnEnable != null)
            {
                m_OnEnable.Invoke();
            }

            if (m_StartColor != null)
            {
                m_OutlineHighlighter.toggleHighlighter(true, m_StartColor);
            }
        }

        public void onPointerEnter()
        {
            if (m_OutlineHighlighter != null)
            {
                m_OutlineHighlighter.toggleHighlighter(true, m_OnPointerEnterColor);
            }

            if (m_OnPointerEnter != null)
            {
                m_OnPointerEnter.Invoke();
            }
        }

        public void onPointerExit()
        {
            if (m_OutlineHighlighter != null)
            {
                m_OutlineHighlighter.toggleHighlighter(true, m_OnPointerExitColor);
            }

            if (m_OnPointerExit != null)
            {
                m_OnPointerExit.Invoke();
            }
        }

        public void onPointerInteract()
        {
            if (m_OnPointerInteract != null)
            {
                m_OnPointerInteract.Invoke();
            }
        }
    }
}