using Agate.WaskitaInfra1.Object;
using System;

namespace Agate.WaskitaInfra1.LevelProgress
{
    [Serializable]
    public struct DayCondition
    {
        public Weather _weather;
        public float _windStrength;
    }
}