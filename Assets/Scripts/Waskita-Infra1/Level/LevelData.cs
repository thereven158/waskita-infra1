using System;
using System.Collections.Generic;
using A3.DataDrivenEvent;
using as3mbus.Selfish.Source;

namespace Agate.WaskitaInfra1.Level
{
    public class LevelData
    {
        public string Name;
        public string Description;
        public uint DayDuration;
        public List<IQuiz> Quizzes = new List<IQuiz>();
        public List<IEventTriggerData<EventTriggerData>> Events;
        public Weather WeatherForecast;
        public SoilCondition SoilCondition;
        public float WindStrength;
    }
}