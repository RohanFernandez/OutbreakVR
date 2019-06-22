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
    }
}