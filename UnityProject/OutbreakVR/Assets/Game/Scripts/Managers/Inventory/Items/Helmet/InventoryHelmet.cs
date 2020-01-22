using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InventoryHelmet : InventoryItem
    {
        /// <summary>
        /// UI player helmet that displays the reloading sign and various screen effects
        /// </summary>
        [SerializeField]
        private UI_PlayerHelmet m_UIPlayerHelmet = null;

        /// <summary>
        /// sets if the helmet is cracked
        /// </summary>
        [SerializeField]
        private bool m_bIsHelmetCracked = false;
        public bool IsHelmetCracked
        {
            get { return m_bIsHelmetCracked; }
            set { m_bIsHelmetCracked = value; }
        }

        /// <summary>
        /// Disables inventory item on player
        /// </summary>
        public override void enableItem()
        {
            base.enableItem();
            m_UIPlayerHelmet.show();
        }

        /// <summary>
        /// Disables inventory item on player
        /// </summary>
        public override void disableItem()
        {
            base.disableItem();
            m_UIPlayerHelmet.hide();
        }
    }
}