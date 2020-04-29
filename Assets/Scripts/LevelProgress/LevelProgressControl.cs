using System;
using Agate.WaskitaInfra1.Level;

namespace Agate.WaskitaInfra1.LevelProgress
{
    public class LevelProgressControl
    {
        public ILevelProgressData Data => _data;
        private LevelProgressData _data;
        public event Action<ILevelProgressData> OnDataChange;
        public event Action<uint> OnDayChange;
        public event Action<int, object> OnAnswer;
        public event Action<uint> OnCheckPointUpdate;
        public event Action<LevelEvaluationData> OnFinishLevel;
        

        public void LoadData(ILevelProgressData data)
        {
            _data = new LevelProgressData(data);
            OnDataChange?.Invoke(Data);
        }

        public void StartLevel(LevelData level)
        {
            _data = new LevelProgressData(level);
            OnDataChange?.Invoke(Data);
        }

        public void NextDay(uint delta)
        {
            _data.CurrentDay += delta;
            OnDayChange?.Invoke(_data.CurrentDay);
        }

        public void AnswerQuestion(int index, object answer)
        {
            _data.Answers[index] = answer;
            OnAnswer?.Invoke(index, answer);
        }

        public void UpdateCheckPoint()
        {
            _data.LastCheckpoint = _data.CurrentDay;
            OnCheckPointUpdate?.Invoke(_data.LastCheckpoint);
        }

        public void RetryFromCheckPoint()
        {
            _data.CurrentDay = _data.LastCheckpoint;
            OnDayChange?.Invoke(_data.CurrentDay);
            _data.TryCount++;
        }

        public void FinishLevel()
        {
            OnFinishLevel?.Invoke(new LevelEvaluationData(Data));
            _data = null;
        }
    }
}