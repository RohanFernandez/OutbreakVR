using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class HelmetDrop : InventoryDrop
    {
        /// <summary>
        /// Is the helmet cracked or not
        /// </summary>
        [SerializeField]
        private bool m_bIsHelmetCracked = false;
        public bool IsHelmetCracked
        {
            get { return m_bIsHelmetCracked; }
            set { m_bIsHelmetCracked = value ; }
        }
    }
}