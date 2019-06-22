using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IPointerOver
    {
        void onPointerEnter();
        void onPointerExit();
        void onPointerInteract();
    }
}