using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class HelmetStructure
    {
        [SerializeField]
        public bool m_bIsHelmetCracked = false;

        [SerializeField]
        public bool m_bIsHelmetCarried = false;
    }

    [System.Serializable]
    public class ItemInventoryStructure
    {
        [SerializeField]
        public HelmetStructure m_HelmetStructure = null;
    }
}