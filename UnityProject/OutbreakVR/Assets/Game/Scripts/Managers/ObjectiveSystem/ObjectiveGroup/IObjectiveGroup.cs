﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IObjectiveGroup: IReusable
    {
        /// <summary>
        /// Sets the initial data when creating the group
        /// </summary>
        /// <param name="a_strID"></param>
        /// <param name="a_strType"></param>
        /// <param name="a_strStateChangeOnComplete"></param>
        void onInitialize(string a_strID, string a_strType);

        /// <summary>
        /// Adds an objective to its list
        /// </summary>
        /// <param name="a_Objective"></param>
        void addObjective(ObjectiveBase a_Objective);

        /// <summary>
        /// Gets the objective group class type name
        /// </summary>
        /// <returns></returns>
        string getObjGroupType();

        /// <summary>
        /// Called when all objectives in this group is complete
        /// Raise event alerting of completion
        /// </summary>
        void onComplete();

        /// <summary>
        /// Returns the unique ID of this group
        /// </summary>
        /// <returns></returns>
        string getObjGroupID();
    }
}