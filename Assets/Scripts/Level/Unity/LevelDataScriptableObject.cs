using System.Collections.Generic;
using System.Linq;
using A3.CodePattern.Unity;
using A3.DataDrivenEvent;
using Agate.SugiSuma.Quiz;
using as3mbus.Selfish.Source;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "WaskitaInfra1/LevelData", order = 0)]
    public class LevelDataScriptableObject : HumbleScriptableObject<LevelData>
    {
        public string name;
        public string Description;
        public uint DayDuration;

        public List<ScriptableQuiz> Quizzes;
        public List<IEventTriggerData<EventTriggerData>> Events;
        public SoilCondition SoilCondition;
        public Weather WeatherForecast;

        private void OnValidate()
        {
            Object = ToData();
        }

        private void OnEnable()
        {
            Object = Object ?? ToData();
        }

        private LevelData ToData()
        {
            return new LevelData()
            {
                Name = name,
                Description = Description,
                DayDuration = DayDuration,
                Quizzes = new List<IQuiz>(Quizzes.Select(quiz => quiz.Quiz)),
                WeatherForecast = WeatherForecast,
                SoilCondition = SoilCondition,
                Events = Events,
            };
        }
    }
}