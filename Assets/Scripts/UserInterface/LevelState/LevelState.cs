using Agate.WaskitaInfra1.Level;

namespace UserInterface.LevelState
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