using A3.DataDrivenEvent;
using Agate.WaskitaInfra1.Object;
using System.Collections.Generic;

namespace Agate.WaskitaInfra1.Level
{
    public class LevelData
    {
        public string Name;
        public string Location;
        public string Description;
        public uint DayDuration;
        public List<IQuestion> Questions = new List<IQuestion>();
        public List<IEventTriggerData<EventTriggerData>> Events;
        public Weather WeatherForecast;
        public SoilCondition SoilCondition;
        public float WindStrength;
    }
}