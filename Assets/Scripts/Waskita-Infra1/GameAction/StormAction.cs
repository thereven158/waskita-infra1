using UnityEngine;

namespace GameAction
{
    [CreateAssetMenu(menuName = "WaskitaInfra1/GameAction/Storm", fileName = "StormAction")]
    public class StormAction : ScriptableGameAction
    {
        private StormActionControl _stormControl;
        
        public override void Init(GameActionSystem system)
        {
            _stormControl = system.GetActionComponent<StormActionControl>();
        }

        public override void Invoke()
        {
            _stormControl.Invoke();
        }

        public override bool Ready => _stormControl && _stormControl.Ready;
    }
}