using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class PanelObjective : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_Text m_txtobjectiveText = null;

        /// <summary>
        /// Updates the objective description
        /// </summary>
        /// <param name="a_strObjectiveDesc"></param>
        /// <param name="a_bIsComplete"></param>
        public void updateText(string a_strObjectiveDesc, bool a_bIsComplete)
        {
            m_txtobjectiveText.text = a_strObjectiveDesc;
            m_txtobjectiveText.color = a_bIsComplete ? GameManager.ColOutlineHighlighterSelected : GameManager.ColOutlineHighlighterRestricted;
        }
    }
}