using System;

namespace Agate.WaskitaInfra1.GameProgress
{
    public class GameProgressControl
    {
        private GameProgressData _data;

        public IGameProgressData Data => _data;

        public event Action<IGameProgressData> OnDataChange;

        public void SetData(IGameProgressData data)
        {
            if (data == null) return;
            _data = new GameProgressData(data);
            OnDataChange?.Invoke(Data);
        }
        public void NewGame()
        {
            SetData(new GameProgressData());
        }

        public void FinishGame()
        {
            _data.CompletionCount++;
            _data.MaxCompletedLevelIndex = -1;
        }

        public void AddPlayTime(double delta)
        {
            _data.PlayTime += delta;
        }
        public void UpdateCompletedLevelIndex(short index)
        {
            _data.MaxCompletedLevelIndex = Math.Max(index, _data.MaxCompletedLevelIndex);
        }
    }
}