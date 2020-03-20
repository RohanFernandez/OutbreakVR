using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class GunShootEvent : MonoBehaviour
    {
        public void shootGun()
        {
            WeaponManager.FireProjectileWithCurrentGun();
        }
    }
}