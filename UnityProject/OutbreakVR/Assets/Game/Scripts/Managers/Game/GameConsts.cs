using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public static class GameConsts
    {
        #region STATE NAMES
        public const string STATE_NAME_INIT = "INIT";
        public const string STATE_NAME_HOME = "HOME";
        public const string STATE_NAME_LEVEL1 = "LEVEL1";
        #endregion STATE NAMES

        #region GAME LAYERS
        /// <summary>
        /// Interactive item in the environment, to interactive with the controller on pointing at it.
        /// Interactive item should have IPointerOver attached to it.
        /// </summary>
        public const string LAYER_NAME_INTERACTIVE = "INTERACTIVE";
        #endregion GAME LAYERS

        #region AUDIO SOURCE

        /// <summary>
        /// Audio source of the gun fire
        /// </summary>
        public const string AUD_SRC_GUN_FIRE = "AudSrc_GunFire";

        /// <summary>
        /// Audio source of the gun reload
        /// </summary>
        public const string AUD_SRC_GUN_RELOAD = "AudSrc_GunReload";

        /// <summary>
        /// Audio source to be played on item picked up
        /// </summary>
        public const string AUD_SRC_ITEM_PICKUP = "AudSrc_ItemPickedUp";

        #endregion AUDIO SOURCE
        /// <summary>
        /// Audio source to be played on item picked up
        /// </summary>
        public const string AUD_CLIP_ITEM_PICKUP = "AudClip_ItemPickUp";

        #region AUDIO CLIP

        #endregion AUDIO CLIP

        #region TAG
        /// <summary>
        /// Tag on the Player
        /// </summary>
        public const string TAG_PLAYER = "Player";
        #endregion TAG
    }
}