using System;

namespace BlockDrift
{
    public enum PowerUpType { Boost, Swap, Oil, Skydive }

    public class PowerUp
    {
        public PowerUpType Type;
        public float Duration;
    }
}
