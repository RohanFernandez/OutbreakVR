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
        public const string STATE_NAME_NEW_GAME = "Level0_000";
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
        /// The major ambient audio source
        /// </summary>
        public const string AUD_SRC_AMBIENT = "AUD_SRC_AmbientSrc1"; 

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

        /// <summary>
        /// Audio source to be played on turret lever
        /// </summary>
        public const string AUD_SRC_TURRET_LEVER = "AudSrc_TurretLever";

        /// <summary>
        /// Audio source to be played on turret trigger collided with player to activate the turret
        /// </summary>
        public const string AUD_SRC_TURRET_TRIG_ACTIVATE = "AudSrc_TurretTriggerActivate";
        

        #endregion AUDIO SOURCE

        #region AUDIO CLIP
        /// <summary>
        /// Audio source to be played on item picked up
        /// </summary>
        public const string AUD_CLIP_ITEM_PICKUP = "AudClip_ItemPickUp";

        /// <summary>
        /// Audio clip to be played on enemy is in alert mode
        /// </summary>
        public const string AUD_CLIP_ALERT_AMBIENT = "AudClip_AlertAmbient";

        /// <summary>
        /// Audio clip ambient music
        /// </summary>
        public const string AUD_CLIP_AMBIENT = "AmbientAudio1";

        /// <summary>
        /// Audio clip to be played on turret gunfire
        /// </summary>
        public const string AUD_CLIP_TURRET_FIRE = "AudClip_TurretFire";

        /// <summary>
        /// Audio clip to be played on turret lever off
        /// </summary>
        public const string AUD_CLIP_TURRET_LEVER = "AudClip_TurretLever"; 

        /// <summary>
        /// Audio clip to be played on turret trigger on activated when collided with the player
        /// </summary>
        public const string AUD_CLIP_TURRET_TRIG_ACTIVATE = "AudClip_TurretTriggerActivate";

        #endregion AUDIO CLIP

        #region TAG
        /// <summary>
        /// Tag on the Player
        /// </summary>
        public const string TAG_PLAYER = "Player";
        #endregion TAG
    }
}