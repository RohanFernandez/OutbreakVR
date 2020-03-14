using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public static class SystemConsts
    {
        public const string NAMESPACE_MASHMO = "ns_Mashmo.";

        #region SCENE NAMES
        public const string SCENE_NAME_INIT_SCENE = "Init";
        public const string SCENE_NAME_HOME_SCENE = "Home";
        public const string SCENE_NAME_CINEMATIC1 = "Cinematic1";
        public const string SCENE_NAME_CINEMATIC2 = "Cinematic2";
        #endregion SCENE NAMES

        #region OCULUS
        /// <summary>
        /// The app id for oculus gear vr and go
        /// </summary>
        public const string OCULUS_MOBILE_APP_ID = "2964746063538386";
        #endregion

        #region SYSTEM CONFIGURABLES
        public const float DEFAULT_NOTIFICATION_TIME = 5.0f;
        #endregion

        #region NOTIFICATION MESSAGES

        //Entitlement Check Failure
        public const string ERROR_TITLE_ENTITLEMENT_CHECK_FAILURE = "ERROR : ENTITLEMENT CHECK!";
        public const string ERROR_MSG_ENTITLEMENT_CHECK_FAILURE = "Failed to authenticate the user.";

        //User login data retrieval failure
        public const string ERROR_TITLE_LOGIN_RETRIEVAL_FAILURE = "ERROR : LOGIN FAILURE!";
        public const string ERROR_MSG_LOGIN_RETRIEVAL_FAILURE = "Failed to retrieve the users login information.";
        #endregion
    }
}