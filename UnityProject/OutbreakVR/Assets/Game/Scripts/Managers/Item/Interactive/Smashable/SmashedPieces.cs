using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class SmashedPieces : MonoBehaviour
    {
        /// <summary>
        /// The start position of the piece
        /// </summary>
        [SerializeField]
        private Vector3 m_v3StartPos = Vector3.zero;

        [SerializeField]
        private Rigidbody m_rigidBody = null;

        ///// <summary>
        ///// Unity reset, sets the local position to be used in game
        ///// </summary>
        //void Reset()
        //{
        //    m_v3StartPos = transform.localPosition;
        //}

        /// <summary>
        /// Resets the smashed game object
        /// </summary>
        public void resetValues()
        {
            m_rigidBody.isKinematic = false;
            transform.localPosition =  m_v3StartPos;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public void onSmashPhysicsComplete()
        {
            m_rigidBody.isKinematic = true;
        }
    }
}