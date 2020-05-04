using A3.DataDrivenEvent;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public abstract class ScriptableDayEventData : ScriptableObject, IEventTriggerData<EventTriggerData>
    {
        public abstract bool IsSuitable(EventTriggerData data);

        public abstract void Trigger(EventTriggerData data);

        public abstract void Init(IEventTriggerSystem<EventTriggerData> triggerSystem);

        public abstract bool IsReady { get; }
    }
}