using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class SmashableCrate : SmashableBase
    {
        /// <summary>
        /// The gameobject that holds the unbroken object
        /// </summary>
        [SerializeField]
        private GameObject m_UnbrokenObject = null;

        /// <summary>
        /// The parent gameobject that holds the broken object
        /// </summary>
        [SerializeField]
        private GameObject m_ParentBrokenObject = null;

        /// <summary>
        /// The parent of the broken and unbroken object
        /// </summary>
        [SerializeField]
        private GameObject m_ObjectCommonParent = null;

        /// <summary>
        /// The rigid body of the unbroken object to be used
        /// </summary>
        [SerializeField]
        private Rigidbody m_UnbrokenRigidBody = null;

        /// <summary>
        /// The collider of the unbroken object to be used
        /// </summary>
        [SerializeField]
        private Collider m_UnbrokenCollider = null;

        /// <summary>
        /// Sets the unbroken object as active and the broken as deactivated
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
            m_UnbrokenObject.SetActive(true);
            m_ParentBrokenObject.SetActive(false);
            m_fCurrentPhysicsTimePassed = 0.0f;
            m_bIsSmashed = false;

            m_UnbrokenRigidBody.isKinematic = true;
            m_UnbrokenCollider.enabled = false;

            //Set the position to zero because its parent is world set position to reset to
            m_ObjectCommonParent.transform.localPosition = Vector3.zero;

            m_UnbrokenCollider.enabled = true;
            m_UnbrokenRigidBody.isKinematic = false;

            int l_iPiecesCount = m_lstSmashedPieces.Count;
            for (int l_iPieceIndex = 0; l_iPieceIndex < l_iPiecesCount; l_iPieceIndex++)
            {
                m_lstSmashedPieces[l_iPieceIndex].resetValues();
            }
        }

        /// <summary>
        /// Breaks the crate, by deactivating the unbroken gameobject and activating the broken parent
        /// </summary>
        public override void smash()
        {
            base.smash();
            m_UnbrokenRigidBody.isKinematic = true;
            m_UnbrokenCollider.enabled = false;

            m_UnbrokenObject.SetActive(false);
            m_ParentBrokenObject.SetActive(true);
            m_bIsSmashed = true;
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