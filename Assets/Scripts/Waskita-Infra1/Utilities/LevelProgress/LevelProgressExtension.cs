using Agate.WaskitaInfra1.LevelProgress;
using UserInterface.LevelState;

namespace Agate.WaskitaInfra1.Utilities
{
    public static class LevelProgressExtension
    {
        public static LevelState State(this ILevelProgressData progressData)
        {
            LevelState state = progressData.Level.State();
            state.Weather = progressData.Condition._weather;
            
            
            state.WindStrength = progressData.Condition._windStrength;
            return state;
        }

        
    }
}