using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class HelmetDrop : InventoryDrop
    {
        /// <summary>
        /// The condition/strength of the helmet in percentage between 0 - 100
        /// </summary>
        [SerializeField]
        private int m_iStrengthPercentage = 100;
        public int StrengthPercentage
        {
            get { return m_iStrengthPercentage; }
            set { m_iStrengthPercentage = Mathf.Clamp(value, 0, 100); }
        }
    }
}