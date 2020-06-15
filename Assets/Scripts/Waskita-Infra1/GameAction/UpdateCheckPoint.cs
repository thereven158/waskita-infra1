using GameAction;
using Agate.WaskitaInfra1.LevelProgress;

namespace Agate.WaskitaInfra1.GameAction
{
    public class UpdateCheckPoint : ScriptableGameAction
    {
        private LevelProgressControl _levelProgress;

        public override bool Ready => _levelProgress != null;

        public override void Init(GameActionSystem system)
        {
            _levelProgress = system.GetActionComponent<LevelProgressControl>();
        }

        public override void Invoke()
        {
            _levelProgress.UpdateCheckPoint();
        }


    }
}