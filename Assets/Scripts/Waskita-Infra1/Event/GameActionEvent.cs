using A3.DataDrivenEvent;
using GameAction;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class GameActionEvent : ScriptableDayEventData
    {
        [SerializeField]
        private uint _day = default;

        [SerializeField]
        private ScriptableGameAction _action = default;

        public override bool IsSuitable(EventTriggerData data)
        {
            return data.Day == _day;
        }

        public override void Trigger(EventTriggerData data)
        {
            _action.Invoke();
        }

        public override void Init(IEventTriggerSystem<EventTriggerData> triggerSystem)
        {
            _action.Init(triggerSystem.GetEventComponent<GameActionSystem>());
        }

        public override bool IsReady => _action.Ready;
    }
}