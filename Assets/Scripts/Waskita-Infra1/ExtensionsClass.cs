using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using UserInterface.LevelState;

namespace Agate.WaskitaInfra1
{
    public static class ExtensionsClass
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

        public static LevelState State(this ILevelProgressData progressData)
        {
            LevelState state = progressData.Level.State();
            state.Weather = progressData.Condition._weather;
            state.WindStrength = progressData.Condition._windStrength;
            return state;
        }
    }
}