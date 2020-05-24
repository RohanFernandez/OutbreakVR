using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public static class GameConsts
    {
        #region STATE NAMES
        public const string STATE_NAME_INIT             = "INIT";
        public const string STATE_NAME_HOME             = "Home_000";
        public const string STATE_NAME_NEW_GAME         = "Cinematic1_000";
        public const string STATE_NAME_TRAINING         = "Training_100";
        public const string STATE_NAME_FIRST_LEVEL      = "Level1_100";
        public const string STATE_NAME_LAST_LEVEL       = "Credits_000";

        public const string STATE_NAME_TESTING1_LEVEL   = "TESTING1_000";
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

        /// <summary>
        /// Environment task object layer
        /// </summary>
        public const string LAYER_NAME_ENVIRONMENT_TASK_OBJECT = "ENVIRONMENT_TASK_OBJECT";

        /// <summary>
        /// Player hit
        /// </summary>
        public const string LAYER_NAME_PLAYER = "PLAYER";

        /// <summary>
        /// The enemy colliders to hit
        /// </summary>
        public const string LAYER_NAME_ENEMY_HIT_COLLIDER = "ENEMY_HIT_COLLIDER";
        #endregion GAME LAYERS

        #region TAG
        #endregion TAG
    }
}