using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class MeleeWeaponBase : WeaponBase
    {
        /// <summary>
        /// On the chainsaw blade's collider is triggerd by an enemy
        /// </summary>
        /// <param name="a_Collider"></param>
        protected virtual void OnTriggerEnter(Collider a_Collider)
        {
        }

        /// <summary>
        /// On trigger exit from the enemy or default
        /// </summary>
        /// <param name="a_Collider"></param>
        protected virtual void OnTriggerExit(Collider a_Collider)
        {
        }
    }
}