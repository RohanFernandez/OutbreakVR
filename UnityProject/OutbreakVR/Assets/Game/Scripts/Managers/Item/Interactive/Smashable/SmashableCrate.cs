using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class SmashableCrate : SmashableBase
    {
        /// <summary>
        /// Sets the unbroken object as active and the broken as deactivated
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
        }

        /// <summary>
        /// Breaks the crate, by deactivating the unbroken gameobject and activating the broken parent
        /// </summary>
        public override void smash()
        {
            base.smash();
        }

        //Uncomment if you require the physics to exist only for certain amount of time after a hit
        private void Update()
        {
            if (m_bIsSmashed &&
                m_fCurrentPhysicsTimePassed < m_fAllowedPhysicsTime)
            {
                m_fCurrentPhysicsTimePassed += Time.deltaTime;
                if (m_fCurrentPhysicsTimePassed > m_fAllowedPhysicsTime)
                {
                    //disable physics in broken pieces
                    int l_iPiecesCount = m_lstSmashedPieces.Count;
                    for (int l_iPieceIndex = 0; l_iPieceIndex < l_iPiecesCount; l_iPieceIndex++)
                    {
                        m_lstSmashedPieces[l_iPieceIndex].onSmashPhysicsComplete();
                    }

                    //deactivate the parent of the pieces
                    m_ParentBrokenObject.gameObject.SetActive(false);
                }
            }
        }
    }
}