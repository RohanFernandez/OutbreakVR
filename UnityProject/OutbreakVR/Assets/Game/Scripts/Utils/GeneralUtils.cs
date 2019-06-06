﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public static class GeneralUtils
    {
        #region Hashtable Utils
        /// <summary>
        /// Returns string from hashtable object
        /// Returns string.Empty if object is null
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public static string GetString(Hashtable a_Hashtable, string a_strAttributeKey)
        {
            System.Object l_Obj = a_Hashtable[a_strAttributeKey];
            return (l_Obj == null) ? string.Empty : l_Obj.ToString();
        }

        /// <summary>
        /// Returns string from hashtable object
        /// Returns 0 if object is null
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public static int GetInt(Hashtable a_Hashtable, string a_strAttributeKey)
        {
            System.Object l_Obj = a_Hashtable[a_strAttributeKey];
            string l_strAtrributeValue = (l_Obj == null) ? null : l_Obj.ToString();

            int l_iReturn = 0;
            if (!string.IsNullOrEmpty(l_strAtrributeValue))
            {
                int.TryParse(l_strAtrributeValue, out l_iReturn);
            }
            return l_iReturn;
        }

        /// <summary>
        /// Returns string from hashtable object
        /// Returns 0.0f if object is null
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public static float GetFloat(Hashtable a_Hashtable, string a_strAttributeKey)
        {
            System.Object l_Obj = a_Hashtable[a_strAttributeKey];
            string l_strAtrributeValue = (l_Obj == null) ? null : l_Obj.ToString();

            float l_fReturn = 0.0f;
            if (!string.IsNullOrEmpty(l_strAtrributeValue))
            {
                float.TryParse(l_strAtrributeValue, out l_fReturn);
            }
            return l_fReturn;
        }

        /// <summary>
        /// Returns string from hashtable object
        /// Return false if key is null or empty
        /// </summary>
        /// <param name="a_strAttributeKey"></param>
        /// <returns></returns>
        public static bool GetBool(Hashtable a_Hashtable,  string a_strAttributeKey)
        {
            System.Object l_Obj = a_Hashtable[a_strAttributeKey];
            string l_strAtrributeValue = (l_Obj == null) ? null : l_Obj.ToString();

            bool l_bReturn = false;
            if (!string.IsNullOrEmpty(l_strAtrributeValue))
            {
                bool.TryParse(l_strAtrributeValue, out l_bReturn);
            }
            return l_bReturn;
        }
        #endregion Hashtable Utils
    }
}