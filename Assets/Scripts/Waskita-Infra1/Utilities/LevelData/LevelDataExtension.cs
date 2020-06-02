using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.UserInterface.LevelState;

namespace Agate.WaskitaInfra1.Utilities
{
    public static class LevelDataExtension
    {
        public static LevelState State(this LevelData level)
        {
            return new LevelState()
            {
                Weather = level.WeatherForecast,
                LevelName = level.Name,
                ProjectDuration = level.DayDuration,
                SoilCondition = level.SoilCondition,
                WindStrength = level.WindStrength
            };
        }


    }
}