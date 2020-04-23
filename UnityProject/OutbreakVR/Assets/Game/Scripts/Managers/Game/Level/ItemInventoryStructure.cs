using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class HelmetStructure
    {
        [SerializeField]
        public int m_iHelmetStrength = 0;

        [SerializeField]
        public bool m_bIsHelmetCarried = false;
    }

    [System.Serializable]
    public class ItemInventoryStructure
    {
        [SerializeField]
        public HelmetStructure m_HelmetStructure = null;

        [SerializeField]
        public int m_iPowerNodeCount = 0;

        [SerializeField]
        public int m_iC4Count = 0;
    }
}