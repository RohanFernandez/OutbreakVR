using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum GAME_EVENT_TYPE
    {
        ON_CONTROLLER_CHANGED,                      /* <ID_NEW_CONTROLLER_TYPE, ID_OLD_CONTROLLER_TYPE, ID_OLD_CONTROLLER_ANCHOR, ID_NEW_CONTROLLER_ANCHOR > */
        ON_GAME_STATE_CHANGED,                      /* <ID_OLD_GAME_STATE, ID_OLD_GAME_STATE> */
        ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED,      /* <ID_NEW_WEAPON_CATEGORY_TYPE, ID_NEW_WEAPON_TYPE, ID_NEW_WEAPON_BASE, ID_OLD_WEAPON_CATEGORY_TYPE, ID_OLD_WEAPON_TYPE, ID_OLD_WEAPON_BASE> */
        ON_SCENE_CHANGED,                           /* <NAME_OLD_SCENE, NAME_NEW_SCENE> */
        ON_SEQUENCE_COMPLETE                        /* <ISEQUENCE> */
    }

    public struct GameEventTypeConst
    {
        #region ON_CONTROLLER_CHANGED
        public const string ID_NEW_CONTROLLER_TYPE      = "ID_NEW_CONTROLLER_TYPE";     /* CONTROLLER_TYPE */
        public const string ID_OLD_CONTROLLER_TYPE      = "ID_OLD_CONTROLLER_TYPE";     /* CONTROLLER_TYPE */
        public const string ID_OLD_CONTROLLER_ANCHOR    = "ID_OLD_CONTROLLER_ANCHOR";   /* GAMEOBJECT */
        public const string ID_NEW_CONTROLLER_ANCHOR    = "ID_NEW_CONTROLLER_ANCHOR";   /* GAMEOBJECT */
        #endregion ON_CONTROLLER_CHANGED

        #region ON_GAME_STATE_CHANGED
        public const string ID_OLD_GAME_STATE = "ID_OLD_GAME_STATE";     /* STRING */
        public const string ID_NEW_GAME_STATE = "ID_NEW_GAME_STATE";     /* STRING */
        #endregion ON_GAME_STATE_CHANGED

        #region ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED
        public const string ID_NEW_WEAPON_CATEGORY_TYPE = "ID_NEW_WEAPON_CATEGORY_TYPE";    /* WEAPON_CATEGORY_TYPE */
        public const string ID_NEW_WEAPON_TYPE          = "ID_NEW_WEAPON_TYPE";             /* WEAPON_TYPE */
        public const string ID_NEW_WEAPON_BASE          = "ID_NEW_WEAPON_BASE";             /* WeaponBase */
        public const string ID_OLD_WEAPON_CATEGORY_TYPE = "ID_OLD_WEAPON_CATEGORY_TYPE";    /* WEAPON_CATEGORY_TYPE */
        public const string ID_OLD_WEAPON_TYPE          = "ID_OLD_WEAPON_TYPE";             /* WEAPON_TYPE */
        public const string ID_OLD_WEAPON_BASE          = "ID_OLD_WEAPON_BASE";             /* WeaponBase */
        #endregion ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED

        #region ON_SCENE_CHANGED
        public const string ID_OLD_SCENE_NAME = "ID_OLD_SCENE_NAME";        /* STRING */
        public const string ID_NEW_SCENE_NAME = "ID_NEW_SCENE_NAME";        /* STRING */
        #endregion ON_SCENE_CHANGED

        #region ON_SEQUENCE_COMPLETE
        public const string ID_SEQUENCE_REF = "ID_SEQUENCE_REF";        /* ISEQUENCE */
        #endregion ON_SEQUENCE_COMPLETE
    }
}
