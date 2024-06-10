using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class Models
{
    #region - Player -

    public enum PlayerStance
    {
        Stand,
        Crouch,
        Prone,
        Sliding,
        Wallrunning
    }
    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float ViewXSens;
        public float ViewYSens;

        public bool ViewXInverted;
        public bool ViewYInverted;

        [Header("Movement Settings")]
        public bool sprintingHold;
        public float MovementSmoothing;

        [Header("Movement - Running")]
        public float runningForwardSpeed;
        public float runningStrafeSpeed;

        [Header("Movement - Walking")]
        public float walkingForwardSpeed;
        public float walkingStrafeSpeed;
        public float walkingBackwardsSpeed;
        

        [Header("Jumping")]
        public float jumpingHeight;
        public float jumpingFallOff;
        public float fallingSmoothing;

        [Header("Speed Effectors")]
        public float speedEffector = 1;
        public float crouchSpeedEffector;
        public float proneSpeedEffector;
        public float fallingSpeedEffector;
        public float slidingSpeedEffector;

        [Header("Movement - Sliding")]
        public float slideStamina;

        [Header("Movement - Wallrunning")]
        public float wallRunSpeed;
        public bool wallrunning;
    }

    [Serializable]

    public class CharacterStance
    {
        public float cameraHeight;
        public CapsuleCollider stanceCollider;
    }

    #endregion

    #region - Weapons - 

    [Serializable]
    public class WeaponSettingsModel
    {
        [Header("Sway")]
        public float swayAmount;
        public bool swayYInverted;
        public bool swayXInverted;
        public float swaySmoothing;
    }
    #endregion
}
