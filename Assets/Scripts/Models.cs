using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class Models
{
    #region - Player -
    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float ViewXSens;
        public float ViewYSens;

        public bool ViewXInverted;
        public bool ViewYInverted;

        [Header("Movement")]
        public float walkingForwardSpeed;
        public float walkingStrafeSpeed;
        public float walkingBackwardsSpeed;
    }

    #endregion
}
