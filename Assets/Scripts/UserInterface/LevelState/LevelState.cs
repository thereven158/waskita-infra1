using Agate.WaskitaInfra1.Object;

namespace Agate.WaskitaInfra1.UserInterface.LevelState
{
    public struct LevelState
    {
        public string LevelName;
        public uint ProjectDuration;
        public float WindStrength;
        public SoilCondition SoilCondition;
        public Weather Weather;
    }
}