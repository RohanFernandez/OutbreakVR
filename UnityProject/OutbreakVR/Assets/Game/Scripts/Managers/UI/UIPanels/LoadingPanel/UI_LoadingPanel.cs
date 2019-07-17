using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_LoadingPanel : AbsUISingleton
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_LoadingPanel s_Instance = null;

        /// <summary>
        /// initializes, sets singleton to this
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
        }

        /// <summary>
        /// sets singleton to null
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            
            s_Instance = null;
        }

        public static void Show()
        {
            s_Instance.show();
        }

        public static void Hide()
        {
            s_Instance.hide();
        }
    }
}