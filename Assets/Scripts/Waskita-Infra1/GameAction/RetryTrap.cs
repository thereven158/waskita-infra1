using UnityEngine;

namespace GameAction
{
    [CreateAssetMenu(menuName = "WaskitaInfra1/GameAction/Retry Trap", fileName = "RetryTrap")]
    public class RetryTrap : ScriptableGameAction
    {
        [TextArea]
        public string _warningMessage;
        [TextArea]
        public string _failureMessage;
        public bool _isContinueCorrect;
        private RetryTrapControl _stormControl;
        
        public override void Init(GameActionSystem system)
        {
            _stormControl = system.GetActionComponent<RetryTrapControl>();
        }

        public override void Invoke()
        {
            _stormControl.Invoke(this);
        }

        public override bool Ready => _stormControl && _stormControl.Ready;
    }
}