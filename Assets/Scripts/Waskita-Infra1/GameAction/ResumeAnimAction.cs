using Agate.WaskitaInfra1.Animations;
using GameAction;

namespace Agate.WaskitaInfra1.GameAction
{
    public class ResumeAnimAction : ScriptableGameAction
    {
        AnimationScenesManager _animManager;
        public override bool Ready => _animManager != null;

        public override void Init(GameActionSystem system)
        {
            _animManager = system.GetActionComponent<AnimationScenesManager>();
        }

        public override void Invoke()
        {
            _animManager.ResumeAnimation();
        }

    }
}