using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class OutlineHighlighter : OutlineHighlighterBase
    {
        /// <summary>
        /// The mesh renderer that holds the material list
        /// </summary>
        [SerializeField]
        private MeshRenderer m_MeshRenderer = null;

        /// <summary>
        /// The material than will highlight the gameobject
        /// </summary>
        [SerializeField]
        private Material m_matOutlineHighlighter = null;

        /// <summary>
        /// The uniform name of the outline color
        /// </summary>
        private const string UNIF_OUTLINE_COLOR = "_OutlineColor";

        /// <summary>
        /// The highlighter is on
        /// </summary>
        private bool m_bIsHighlighterOn = false;

        /// <summary>
        /// Ativates/ Deactivates the Highlighter
        /// </summary>
        public override void toggleHighlighter(bool a_bIsOn, Color a_Color)
        {
            m_bIsHighlighterOn = a_bIsOn;

            List<Material> l_lstMats = new List<Material>(m_MeshRenderer.sharedMaterials);

            int l_iMatFoundIndex = -1;
            bool l_bIsMatInList = false;
            int l_iMatCount = l_lstMats.Count;
            for (int l_iMatIndex = 0; l_iMatIndex < l_iMatCount; l_iMatIndex++)
            {
                if (l_lstMats[l_iMatIndex].shader.name.Equals(m_matOutlineHighlighter.shader.name, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_bIsMatInList = true;
                    l_iMatFoundIndex = l_iMatIndex;
                    break;
                }
            }

            if (m_bIsHighlighterOn)
            {
                if (!l_bIsMatInList)
                {
                    l_lstMats.Add(m_matOutlineHighlighter);
                }

                m_MeshRenderer.sharedMaterials = l_lstMats.ToArray();

                if (m_MeshRenderer.materials.Length > 0)
                {
                    Material l_Mat = m_MeshRenderer.materials[m_MeshRenderer.materials.Length - 1];

                    if (l_Mat != null)
                    {
                        l_Mat.SetColor(UNIF_OUTLINE_COLOR, a_Color);
                    }
                }
            }
            else
            {
                if (l_bIsMatInList)
                {
                    l_lstMats.RemoveAt(l_iMatFoundIndex);
                    m_MeshRenderer.sharedMaterials = l_lstMats.ToArray();
                }
            }
        }
    }
}