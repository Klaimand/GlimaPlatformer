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
        public static float maxPoints = 50f;
        public static float pointsPerInput = 6f;
        public static float pointsLostPerSecond = 3f;
    }

    public class MediumButtonValues
    {
        public static float maxPoints = 80f;
        public static float pointsPerInput = 6f;
        public static float pointsLostPerSecond = 8f;
    }

    public class HardButtonValues
    {
        public static float maxPoints = 100f;
        public static float pointsPerInput = 3f;
        public static float pointsLostPerSecond = 10f;
    }
