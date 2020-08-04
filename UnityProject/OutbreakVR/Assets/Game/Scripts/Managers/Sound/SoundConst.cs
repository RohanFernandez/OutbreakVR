using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class SoundConst
    {
        #region AUDIO SOURCE
        public static string AUD_SRC_PLAYER_FOOTSTEPS           = "AudSrc_PlayerFootsteps";

        /// <summary>
        /// The audio src of the player
        /// </summary>
        public const string AUD_SRC_PLAYER_1 = "AudSrc_Player_1";

        /// <summary>
        /// The audio src of the player
        /// </summary>
        public const string AUD_SRC_PLAYER_2 = "AudSrc_Player_2";

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

        /// <summary>
        /// Audio source to be played cursor click
        /// </summary>
        public const string AUD_SRC_CURSOR_CLICK = "AudSrcClick";

        /// <summary>
        /// The aud src that plays all sounds on the worn helmet
        /// </summary>
        public const string AUD_SRC_PLAYER_HELMET = "Helmet";

        #endregion AUDIO SOURCE

        #region AUDIO CLIP

        /// <summary>
        /// Audio clip of common cursor click
        /// </summary>
        public const string AUD_CLIP_CURSOR_CLICK = "CursorClick";

        /// <summary>
        /// Audio clip to be played on item picked up
        /// </summary>
        public const string AUD_CLIP_ITEM_PICKUP = "AudClip_ItemPickUp";

        /// <summary>
        /// Audio clip to be played on helmet picked up
        /// </summary>
        public const string AUD_CLIP_HELMET_PICKUP = "HelmetPickUp";

        /// <summary>
        /// Audio clip to be played on enemy is in alert mode
        /// </summary>
        public const string AUD_CLIP_ALERT_AMBIENT = "AudClip_AlertAmbient";

        /// <summary>
        /// Audio clip ambient music
        /// </summary>
        public const string AUD_CLIP_AMBIENT = "AmbientAudio";

        /// <summary>
        /// The ambient audio that will be played if not set explicitly
        /// </summary>
        public const string AUD_CLIP_DEFAULT_AMBIENT_AUDIO = "DefaultAmbient";

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

        /// <summary>
        /// Audio clip to be played when the proximity bomb is armed and the countdown has begin
        /// </summary>
        public const string AUD_CLIP_PROXIMITY_BOMB_ARMED = "ProximityBombArmed";

        /// <summary>
        /// Audio clip to be played when the proximity bomb blasts
        /// </summary>
        public const string AUD_CLIP_PROXIMITY_BOMB_BLAST = "ProximityBombBlast";

        public static string AUD_CLIP_PLAYER_CONCRETE_FOOTSTEPS = "AudClip_PlayerFootsteps";

        #endregion AUDIO CLIP
    }
}