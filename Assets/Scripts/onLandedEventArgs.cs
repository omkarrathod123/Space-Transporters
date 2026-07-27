using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class onLandedEventArgs : EventArgs
    {
        public Lander.LandingType landingType;
        public int score;
        public float landingAngle;
        public float landingSpeed;
        public float scoreMultiplier;
    }
}