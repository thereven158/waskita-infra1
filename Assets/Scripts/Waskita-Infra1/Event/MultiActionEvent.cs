using A3.DataDrivenEvent;
using GameAction;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    [Serializable]
    public struct MultiActionEvent : IEventTriggerData<EventTriggerData>
    {
        [SerializeField]
        private uint _day;

        [SerializeField]
        private List<ScriptableGameAction> _actions;

        public MultiActionEvent(uint day, List<ScriptableGameAction> actions) : this()
        {
            _day = day;
            _actions = actions;
        }

        public bool IsSuitable(EventTriggerData data)
        {
            return data.Day == _day;
        }

        public void Trigger(EventTriggerData data)
        {
            foreach (ScriptableGameAction action in _actions)
                action.Invoke();
        }

        public void Init(IEventTriggerSystem<EventTriggerData> triggerSystem)
        {
            foreach (ScriptableGameAction action in _actions)
                action.Init(triggerSystem.GetEventComponent<GameActionSystem>());
        }

        public bool IsReady => _actions.All(act => act.Ready);
    }
}