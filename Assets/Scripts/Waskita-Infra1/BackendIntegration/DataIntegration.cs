using System.Collections.Generic;
using Agate.Waskita.Request;
using Agate.Waskita.Responses;
using Agate.Waskita.Responses.Data;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.PlayerAccount;
using DayCondition = Agate.WaskitaInfra1.LevelProgress.DayCondition;
using LevelData = Agate.WaskitaInfra1.Level.LevelData;

namespace BackendIntegration
{
    public static class DataIntegration
    {
        public static PlayerAccountData AccountData(this LoginResponse response)
        {
            return new PlayerAccountData()
            {
                Username = response.name,
                AuthenticationToken = response.token
            };
        }

        public static IGameProgressData GameData(this Agate.Waskita.Responses.Data.GameProgressData data)
        {
            return new GameProgress()
            {
                CompletionCount = (uint) data.completionCount,
                PlayTime = data.playTime.TotalSeconds,
                MaxCompletedLevelIndex = (short) data.maxCompleteLevelIndex
            };
        }

        public static LevelProgress LevelProgress(this LevelControl levelControl, LevelProgressData data)
        {
            LevelProgress progress = new LevelProgress
            {
                LastCheckpoint = (uint) data.lastCheckpoint,
                CurrentDay = (uint) data.currentDay,
                TryCount = (uint) data.tryCount,
                Level = levelControl.GetLevel(data.level),
                Answers = new List<object>(),
            };
            for (int i = 0; i < progress.Level.Questions.Count; i++)
                progress.Answers.Add(null);
            if (data.storedAnswers == null) return progress;
            for (int i = 0; i < data.storedAnswers.Count; i++)
                if (progress.Level.Questions[i].Quiz.Question is IMultipleChoiceQuestion multipleChoice)
                    progress.Answers[i] = multipleChoice.AnswerOptions[data.storedAnswers[i]];

            return progress;
        }

        public static SaveGameRequest SaveRequest(LevelControl control, LevelProgress data)
        {
            SaveGameRequest request = new SaveGameRequest()
            {
                currentDay = (int) data.CurrentDay,
                tryCount = (int) data.TryCount,
                lastCheckPoint = (int) data.LastCheckpoint,
                answer = new List<int>()
            };
            for (int i = 0; i < data.Level.Questions.Count; i++)
                request.answer.Add(-1);
            if (data.Answers == null) return request;
            for (int i = 0; i < data.Answers.Count; i++)
                if (data.Level.Questions[i].Quiz.Question is IMultipleChoiceQuestion multipleChoice)
                    request.answer[i] = multipleChoice.AnswerOptions.IndexOf(data.Answers[i]);
            return request;
        }
    }

    public class LevelProgress : ILevelProgressData
    {
        public uint LastCheckpoint { get; set; }
        public uint CurrentDay { get; set; }
        public uint TryCount { get; set; }
        public List<object> Answers { get; set; }
        public DayCondition Condition { get; set; }
        public LevelData Level { get; set; }

        public LevelProgress()
        {
        }

        public LevelProgress(ILevelProgressData data)
        {
            LastCheckpoint = data.LastCheckpoint;
            CurrentDay = data.CurrentDay;
        }
    }

    public class GameProgress : IGameProgressData
    {
        public short MaxCompletedLevelIndex { get; set; }
        public uint CompletionCount { get; set; }
        public double PlayTime { get; set; }
    }
}