using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class SmashableBase : AbsEnvironmentInteractableObject
    {
        /// <summary>
        /// The gameobject that holds the unbroken object
        /// </summary>
        [SerializeField]
        protected GameObject m_UnbrokenObject = null;

        /// <summary>
        /// The parent gameobject that holds the broken object
        /// </summary>
        [SerializeField]
        protected GameObject m_ParentBrokenObject = null;

        /// <summary>
        /// The parent of the broken and unbroken object
        /// </summary>
        [SerializeField]
        protected GameObject m_ObjectCommonParent = null;

        /// <summary>
        /// The rigid body of the unbroken object to be used
        /// </summary>
        [SerializeField]
        protected Rigidbody m_UnbrokenRigidBody = null;

        /// <summary>
        /// The collider of the unbroken object to be used
        /// </summary>
        [SerializeField]
        protected Collider m_UnbrokenCollider = null;

        /// <summary>
        /// The list of all the smashable pieces
        /// </summary>
        [SerializeField]
        protected List<SmashedPieces> m_lstSmashedPieces = null;

        /// <summary>
        /// Is the unbroken object have physics
        /// If this is true then m_UnbrokenRigidBody cannot be null
        /// </summary>
        [Tooltip("If this is true then m_UnbrokenRigidBody cannot be null")]
        [SerializeField]
        protected bool m_bIsPhysicsInUnbrokenObj = true;

        /// <summary>
        /// On smash the physics of the broken pieces are allowed for a certain amount of time
        /// </summary>
        protected bool m_bIsSmashed = false;

        /// <summary>
        /// The time after smash for which the physics is allowed
        /// </summary>
        protected float m_fAllowedPhysicsTime = 2.0f;

        /// <summary>
        /// The time after smash
        /// </summary>
        protected float m_fCurrentPhysicsTimePassed = 0.0f;

        public virtual void smash()
        { 
        
        }

        public override void resetValues()
        {
            m_UnbrokenObject.SetActive(true);
            m_ParentBrokenObject.SetActive(false);
            m_fCurrentPhysicsTimePassed = 0.0f;
            m_bIsSmashed = false;

            if (m_bIsPhysicsInUnbrokenObj)
            {
                m_UnbrokenRigidBody.isKinematic = true;
            }
            m_UnbrokenCollider.enabled = false;

            //Set the position to zero because its parent is world set position to reset to
            m_ObjectCommonParent.transform.localPosition = Vector3.zero;

            m_UnbrokenCollider.enabled = true;

            if (m_bIsPhysicsInUnbrokenObj)
            {
                m_UnbrokenRigidBody.isKinematic = false;
            }

            int l_iPiecesCount = m_lstSmashedPieces.Count;
            for (int l_iPieceIndex = 0; l_iPieceIndex < l_iPiecesCount; l_iPieceIndex++)
            {
                m_lstSmashedPieces[l_iPieceIndex].resetValues();
            }
        }
    }
}