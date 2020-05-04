using A3.DataDrivenEvent;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    [CreateAssetMenu(fileName = "debugEvent", menuName = "WaskitaInfra1/Event/Debug")]
    public class DebugLogEvent: ScriptableDayEventData
    {
        public uint Day;
        public override bool IsSuitable(EventTriggerData data)
        {
            return data.Day == Day;
        }

        public override void Trigger(EventTriggerData data)
        {
            Debug.Log("EVENT INVOKED");
        }

        public override void Init(IEventTriggerSystem<EventTriggerData> triggerSystem)
        {
            // do nothing
        }

        public override bool IsReady => true;
    }
}