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
        public event Action OnRetryToCheckpoint;


        public void LoadData(ILevelProgressData data)
        {
            if (data == null) return;
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
            if(_data.CurrentDay > _data.Level.DayDuration)
                FinishLevel();
        }

        public void AnswerQuestion(IQuestion item, object answer)
        {
            int index = _data.Level.Questions.IndexOf(item);
            if(index != -1) AnswerQuestion(index,answer);
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
            OnRetryToCheckpoint?.Invoke();
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