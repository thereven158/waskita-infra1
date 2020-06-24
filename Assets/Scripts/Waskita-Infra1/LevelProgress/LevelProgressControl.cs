using Agate.WaskitaInfra1.Level;
using System;

namespace Agate.WaskitaInfra1.LevelProgress
{
    public class LevelProgressControl
    {
        public ILevelProgressData Data => _data;
        private LevelProgressData _data;
        public event Action<ILevelProgressData> OnDataChange;
        public event Action<uint> OnDayChange;
        public event Action<DayCondition> OnConditionChange;
        public event Action<int, object> OnAnswer;
        public event Action<uint> OnCheckPointUpdate;
        public event Action<LevelEvaluationData> OnFinishLevel;
        public event Action OnRetryToCheckpoint;

        public void ClearData()
        {
            _data = null;
        }

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
            if (_data.CurrentDay > _data.Level.DayDuration)
                FinishLevel();
        }

        public void AnswerQuestion(IQuestion item, object answer)
        {
            int index = _data.Level.Questions.IndexOf(item);
            if (index != -1) AnswerQuestion(index, answer);
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
            _data.Condition = new DayCondition()
            {
                _weather = Data.Level.WeatherForecast,
                _windStrength = Data.Level.WindStrength
            };
            OnDayChange?.Invoke(_data.CurrentDay);
            _data.TryCount++;
        }

        public void ChangeCondition(DayCondition condition)
        {
            _data.Condition = ParseCondition(condition);
            OnConditionChange?.Invoke(_data.Condition);
        }

        private DayCondition ParseCondition(DayCondition condition)
        {
            return new DayCondition()
            {
                _windStrength = condition._windStrength == default ? _data.Condition._windStrength : condition._windStrength,
                _weather = condition._weather == default ? _data.Condition._weather : condition._weather
            };
        }

        public void FinishLevel()
        {
            LevelEvaluationData result = new LevelEvaluationData(Data);
            OnFinishLevel?.Invoke(result);
            _data = null;
        }

        public static bool DataEquality(ILevelProgressData data1, ILevelProgressData data2)
        {
            return data1.CurrentDay == data2.CurrentDay &&
                   data1.TryCount == data2.TryCount &&
                   data1.Level == data2.Level &&
                   data1.LastCheckpoint == data2.LastCheckpoint;
        }

        public bool CurrentDataEquality(ILevelProgressData data)
        {
            return DataEquality(_data, data);
        }
    }
}