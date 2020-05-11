using UnityEngine;

namespace GameAction
{
    public abstract class ScriptableGameAction: ScriptableObject, IGameActionData
    {
        public abstract void Init(GameActionSystem system);

        public abstract void Invoke();
        public abstract bool Ready { get; }
    }
}