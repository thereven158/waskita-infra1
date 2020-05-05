using A3.DataDrivenEvent;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    [CreateAssetMenu(menuName = "WaskitaInfra1/Event/FinishProject", fileName = "FinishProject")]
    public class FinishProjectEventData : ScriptableDayEventData
    {
        private LevelProgressControl _levelProgress;
        public uint Day;

        public override bool IsSuitable(EventTriggerData data)
        {
            return data.Day == Day;
        }

        public override void Trigger(EventTriggerData data)
        {
            _levelProgress.FinishLevel();
        }

        public override void Init(IEventTriggerSystem<EventTriggerData> triggerSystem)
        {
            _levelProgress = triggerSystem.GetEventComponent<LevelProgressControl>();
        }


        public override bool IsReady => _levelProgress != null;
    }
}