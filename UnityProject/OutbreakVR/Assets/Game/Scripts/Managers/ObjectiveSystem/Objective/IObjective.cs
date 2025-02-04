﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IObjective : IReusable
    {
        /// <summary>
        /// called on start of initialization
        /// Sets the attributes to the member variable
        /// </summary>
        /// <param name="a_hashAttributes"></param>
        void onInitialize(Hashtable a_hashAttributes);

        /// <summary>
        /// Returns the objective type name
        /// </summary>
        /// <returns></returns>
        string getObjectiveType();

        /// <summary>
        /// Called on objective complete
        /// </summary>
        void onComplete();

        /// <summary>
        /// is objective complete
        /// </summary>
        /// <returns></returns>
        bool isComplete();
    }
}