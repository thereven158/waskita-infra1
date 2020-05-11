namespace GameAction
{
    public interface IGameActionData
    {
        void Init(GameActionSystem system);
        void Invoke();
        bool Ready { get; }
    }
}