using UnityEngine;
using GameAction;
using Agate.WaskitaInfra1.LevelProgress;

namespace Agate.WaskitaInfra1.GameAction
{
    public class DayConditionChange : ScriptableGameAction
    {
        [SerializeField]
        private DayCondition _dayCondition;
        private LevelProgressControl _levelProgress;
        public override bool Ready => _levelProgress != null;

        public override void Init(GameActionSystem system)
        {
            _levelProgress = system.GetActionComponent<LevelProgressControl>();
        }

        public override void Invoke()
        {
            _levelProgress.ChangeCondition(_dayCondition);
        }

    }
}