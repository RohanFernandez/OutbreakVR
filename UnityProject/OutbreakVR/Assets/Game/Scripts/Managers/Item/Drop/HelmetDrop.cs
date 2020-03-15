using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class HelmetDrop : InventoryDrop
    {
        /// <summary>
        /// The sprite renderer that displays the helmet strength indication
        /// </summary>
        [SerializeField]
        private SpriteRenderer m_sprendHelmetStrengthIndicator = null;

        /// <summary>
        /// The sprite that displays a weak helmet strength
        /// </summary>
        [SerializeField]
        private Sprite m_sprWeakHelmetStrength = null;

        /// <summary>
        /// The sprite that displays a moderate helmet strength
        /// </summary>
        [SerializeField]
        private Sprite m_sprModerateHelmetStrength = null;

        /// <summary>
        /// The sprite that displays a strong helmet strength
        /// </summary>
        [SerializeField]
        private Sprite m_sprStrongHelmetStrength = null;

        /// <summary>
        /// The condition/strength of the helmet in percentage between 0 - 100
        /// </summary>
        [SerializeField]
        private int m_iStrengthPercentage = 100;
        public int StrengthPercentage
        {
            get { return m_iStrengthPercentage; }
            set { 
                m_iStrengthPercentage = Mathf.Clamp(value, 0, 100);

                HelmetStrengthIndicator l_HelmetStrengthIndicator = InventoryHelmet.GetHelmetStrengthIndicatorFromPercentage(m_iStrengthPercentage);
                if (l_HelmetStrengthIndicator == HelmetStrengthIndicator.HELMET_STRENGTH_WEAK)
                {
                    m_sprendHelmetStrengthIndicator.sprite = m_sprWeakHelmetStrength;
                }
                else if (l_HelmetStrengthIndicator == HelmetStrengthIndicator.HELMET_STRENGTH_MODERATE)
                {
                    m_sprendHelmetStrengthIndicator.sprite = m_sprModerateHelmetStrength;
                }
                else
                {
                    m_sprendHelmetStrengthIndicator.sprite = m_sprStrongHelmetStrength;
                }
            }
        }
    }
}