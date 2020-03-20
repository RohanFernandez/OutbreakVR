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
        public const string STATE_NAME_TRAINING = "TRAINING_100";
        public const string STATE_NAME_FIRST_LEVEL = "Level1_100";
        public const string STATE_NAME_LAST_LEVEL = "Level1_101";

        public const string STATE_NAME_TESTING1_LEVEL = "TESTING1_000";
        #endregion STATE NAMES


        #region GAME LAYERS
        /// <summary>
        /// Interactive item in the environment, to interactive with the controller on pointing at it.
        /// Interactive item should have IPointerOver attached to it.
        /// </summary>
        public const string LAYER_NAME_INTERACTIVE = "INTERACTIVE";


        /// <summary>
        /// The layer on the enemy
        /// </summary>
        public const string LAYER_NAME_ENEMY = "ENEMY";

        /// <summary>
        /// The layer on smashable objects
        /// </summary>
        public const string LAYER_NAME_SMASHABLE = "SMASHABLE";
        #endregion GAME LAYERS

        #region AUDIO SOURCE

        /// <summary>
        /// The major ambient audio source
        /// </summary>
        public const string AUD_SRC_AMBIENT = "AudSrc_AmbientDefault"; 

        /// <summary>
        /// Audio source of the gun fire
        /// </summary>
        public const string AUD_SRC_GUN_FIRE = "AudSrc_GunFire";

        /// <summary>
        /// Audio source of the gun fire
        /// </summary>
        public const string AUD_SRC_GUN_FIRE_1 = "AudSrc_GunFire1";

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

        /// <summary>
        /// Audio clip to be played on door open
        /// </summary>
        public const string AUD_CLIP_DOOR_OPEN = "AudClip_DoorOpen";

        /// <summary>
        /// Audio clip to be played on door close
        /// </summary>
        public const string AUD_CLIP_DOOR_CLOSE = "AudClip_DoorClose";

        /// <summary>
        /// Audio clip to be played on drawer open
        /// </summary>
        public const string AUD_CLIP_DRAWER_OPEN = "AudClip_DrawerOpen";

        /// <summary>
        /// Audio clip to be played on door keypad click
        /// </summary>
        public const string AUD_CLIP_KEYPAD_CLICK = "AudClip_KeypadClick";

        /// <summary>
        /// Audio clip to be played on door keypad entered is wrong
        /// </summary>
        public const string AUD_CLIP_KEYPAD_WRONG_CODE = "AudClip_KeypadWrongCode";

        /// <summary>
        /// Audio clip to be played on door keypad entered is correct
        /// </summary>
        public const string AUD_CLIP_KEYPAD_CORRECT_CODE = "AudClip_KeypadCorrectCode";

        #endregion AUDIO CLIP

        #region TAG
        /// <summary>
        /// Tag on the Player
        /// </summary>
        public const string TAG_PLAYER = "Player";
        #endregion TAG
    }
}