using System.Collections.Generic;
using A3.DataDrivenEvent;
using A3.Quiz;
using Agate.WaskitaInfra1.Object;

namespace Agate.WaskitaInfra1.Level
{
    public class LevelData
    {
        public string Name;
        public string Description;
        public uint DayDuration;
        public List<IQuestion> Questions = new List<IQuestion>();
        public List<IEventTriggerData<EventTriggerData>> Events;
        public Weather WeatherForecast;
        public SoilCondition SoilCondition;
        public float WindStrength;
    }
}