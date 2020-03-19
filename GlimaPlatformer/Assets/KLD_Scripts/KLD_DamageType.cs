using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class KLD_DamageType
    {
    
    }
    public enum DamageType
    {
       Explosion,
        Grab
    }

    public class EasyButtonValues
    {
        public static float maxPoints = 40f;
        public static float pointsPerInput = 6f;
        public static float pointsLostPerSecond = 3f;
    }

    public class MediumButtonValues
    {
        public static float maxPoints = 60f;
        public static float pointsPerInput = 6f;
        public static float pointsLostPerSecond = 6f;
    }

    public class HardButtonValues
    {
        public static float maxPoints = 80f;
        public static float pointsPerInput = 5f;
        public static float pointsLostPerSecond = 9f;
    }
