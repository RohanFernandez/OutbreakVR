using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TestUserInput : MonoBehaviour
    {
        [SerializeField]
        TMPro.TMP_Text m_txt = null;

        private Vector2 m_v2Swipe = Vector2.zero;

        // Update is called once per frame
        void Update()
        {
            Vector2 l_v2SwipeLatest = ControllerManager.GetSwipe();
            if (l_v2SwipeLatest != Vector2.zero)
            {
                m_v2Swipe = l_v2SwipeLatest;
            }
            m_txt.text = "X: "+ m_v2Swipe.x +"   ,Y: " + m_v2Swipe.y;
        }
    }
}