using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class OutlineHighlighterBase : MonoBehaviour
    {
        /// <summary>
        /// activates/ deactivates the outline highlighter
        /// </summary>
        /// <param name="a_bIsOn"></param>
        /// <param name="a_Color"></param>
        public abstract void toggleHighlighter(bool a_bIsOn, Color a_Color);
    }
}
