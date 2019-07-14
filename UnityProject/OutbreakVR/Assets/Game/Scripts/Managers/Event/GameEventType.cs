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
        ON_SEQUENCE_COMPLETE,                       /* <ISEQUENCE> */
        ON_LEVEL_SELECTED,                          /* <LEVEL_TYPE>*/
        ON_LEVEL_OBJECTIVE_TRIGGERED,               /* <OBJECTIVE_ID> */
        ON_GAME_PAUSED_TOGGLED,                     /* <BOOL>*/
        ON_GAMEPLAY_ENDED,                          /* <BOOL>*/
        ON_PLAYER_STATE_CHANGED,                    /* <OLD_PLAYER_STATE, NEW_PLAYER_STATE> */
        ON_ITEM_PICKED_UP,                          /* <ITEMID, ITEM_BASE> */
        ON_WEAPON_RELOADED,                         /* <WEAPON_TYPE>*/
        ON_WEAPON_FIRED,                            /* <WEAPON_TYPE>*/
        ON_BULLETS_ADDED,                           /* <>*/
        ON_AUDIO_MODE_TOGGLED,                      /* <AUDIO_SRC_TYPES,BOOL> */
    }

    public static class GameEventTypeConst
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

        #region ON_LEVEL_SELECTED
        public const string ID_LEVEL_TYPE = "ID_LEVEL_TYPE";        /* STRING */
        #endregion ON_LEVEL_SELECTED

        #region ON_LEVEL_OBJECTIVE_TRIGGERED
        public const string ID_OBJECTIVE_TYPE = "ID_OBJECTIVE_TYPE";                    /* STRING */
        public const string ID_OBJECTIVE_TRIGGER_ID = "ID_OBJECTIVE_TRIGGER_ID";        /* STRING */
        #endregion ON_LEVEL_OBJECTIVE_TRIGGERED

        #region ON_GAME_PAUSED_TOGGLED
        public const string ID_GAME_PAUSED = "ID_GAME_PAUSED";                    /* BOOL */
        #endregion ON_GAME_PAUSED_TOGGLED

        #region ON_GAMEPLAY_ENDED
        #endregion ON_GAMEPLAY_ENDED

        #region ON_PLAYER_STATE_CHANGED
        public const string ID_OLD_PLAYER_STATE = "ID_OLD_PLAYER_STATE";                    /* PLAYER_STATE */
        public const string ID_NEW_PLAYER_STATE = "ID_NEW_PLAYER_STATE";                    /* PLAYER_STATE */
        #endregion ON_PLAYER_STATE_CHANGED

        #region ON_ITEM_PICKED_UP
        public const string ID_ITEM_DROP_TYPE = "ID_ITEM_DROP_TYPE";                      /* ITEM_TYPE */
        public const string ID_ITEM_BASE = "ID_ITEM_DROP_BASE";                 /* ITEM DROP BASE */
        #endregion ON_ITEM_PICKED_UP

        #region ON_WEAPON_RELOADED
        public const string ID_WEAPON_TYPE = "ID_WEAPON_TYPE";                      /* WEAPON_TYPE */
        public const string ID_TOTAL_BULLETS = "ID_TOTAL_BULLETS";                  /* INT */
        public const string ID_FIRST_MAG_COUNT = "ID_FIRST_MAG_COUNT";              /* INT */
        #endregion ON_WEAPON_RELOADED

        #region ON_WEAPON_FIRED
        //public const string ID_WEAPON_TYPE = "ID_WEAPON_TYPE";                      /* WEAPON_TYPE */
        //public const string ID_TOTAL_BULLETS = "ID_TOTAL_BULLETS";                  /* INT */
        //public const string ID_FIRST_MAG_COUNT = "ID_FIRST_MAG_COUNT";              /* INT */
        #endregion ON_WEAPON_FIRED

        #region ON_WEAPON_FIRED
        public const string ID_GUN_WEAPON = "ID_GUN_WEAPON";                           /* GUN_WEAPON_BASE */
        #endregion ON_WEAPON_FIRED

        #region ON_AUDIO_MODE_TOGGLED
        public const string ID_AUDIO_SRC_TYPE = "ID_AUDIO_SRC_TYPE";                            /* AUDIO_SRC_TYPE */
        public const string ID_IS_AUDIO_MODE_ACTIVATED = "ID_IS_AUDIO_MODE_ACTIVATED";          /* BOOL */
        #endregion ON_AUDIO_MODE_TOGGLED
    }
}
