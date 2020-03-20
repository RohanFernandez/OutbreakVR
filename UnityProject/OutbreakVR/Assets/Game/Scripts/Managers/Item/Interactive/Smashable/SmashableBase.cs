using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class SmashableBase : AbsEnvironmentInteractableObject
    {
        /// <summary>
        /// The list of all the smashable pieces
        /// </summary>
        [SerializeField]
        protected List<SmashedPieces> m_lstSmashedPieces = null;

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

        }
    }
}