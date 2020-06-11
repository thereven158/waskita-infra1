using Agate.WaskitaInfra1.Animations;
using GameAction;

namespace Agate.WaskitaInfra1.GameAction
{
    public class ToggleAnimAction : ScriptableGameAction
    {
        AnimationScenesManager _animManager;

        public bool Play;
        public override bool Ready => _animManager != null;

        public override void Init(GameActionSystem system)
        {
            _animManager = system.GetActionComponent<AnimationScenesManager>();
        }

        public override void Invoke()
        {
            if (Play) _animManager.ResumeAnimation();
            else _animManager.PauseAnimation();
        }

    }
}