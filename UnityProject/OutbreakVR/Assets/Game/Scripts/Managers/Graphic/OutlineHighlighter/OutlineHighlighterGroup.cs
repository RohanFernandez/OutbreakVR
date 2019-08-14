using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class OutlineHighlighterGroup : OutlineHighlighterBase
    {
        /// <summary>
        /// The list of all highlighters that need to be managed
        /// </summary>
        [SerializeField]
        private List<OutlineHighlighter> m_lstOutlineHighlighter = null;

        /// <summary>
        /// Activates/ Deactivates the outline highlighters
        /// </summary>
        /// <param name="a_bIsOn"></param>
        /// <param name="a_Color"></param>
        public override void toggleHighlighter(bool a_bIsOn, Color a_Color)
        {
            int l_iHighlighterCount = m_lstOutlineHighlighter.Count;
            for (int l_iHighlighterIndex = 0; l_iHighlighterIndex < l_iHighlighterCount; l_iHighlighterIndex++)
            {
                m_lstOutlineHighlighter[l_iHighlighterIndex].toggleHighlighter(a_bIsOn, a_Color);
            }
        }
    }
}