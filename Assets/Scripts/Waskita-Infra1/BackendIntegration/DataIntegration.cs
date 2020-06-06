using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.PlayerAccount;
using Agate.WaskitaInfra1.Server.Request;
using Agate.WaskitaInfra1.Server.Responses;
using Agate.WaskitaInfra1.Server.Responses.Data;
using System.Collections.Generic;
using System.Linq;

namespace Agate.WaskitaInfra1.Backend.Integration
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

        public static IGameProgressData GameData(this ServerGameProgressData data)
        {
            return new GameProgress()
            {
                CompletionCount = (uint)data.completionCount,
                PlayTime = data.playTime.TotalSeconds,
                MaxCompletedLevelIndex = (short)data.maxCompleteLevelIndex
            };
        }

        public static LevelProgressIntegration LevelProgress(this LevelControl levelControl, ServerLevelProgressData data)
        {
            if (data.level == 0)
            {
                return null;
            }
            LevelProgressIntegration progress = new LevelProgressIntegration
            {
                LastCheckpoint = (uint)data.lastCheckpoint,
                CurrentDay = (uint)data.lastCheckpoint,
                TryCount = (uint)data.tryCount,
                Level = levelControl.GetLevel(data.level - 1),
                Answers = new List<object>(),
            };
            for (int i = 0; i < progress.Level.Questions.Count; i++)
                progress.Answers.Add(null);
            if (data.storedAnswers == null) return progress;

            for (int i = 0; i < data.storedAnswers.Count; i++)
            {
                if (data.storedAnswers[i] < 0) continue;
                if (progress.Level.Questions[i].Quiz.Question is IMultipleChoiceQuestion multipleChoice)
                    progress.Answers[i] = multipleChoice.AnswerOptions[data.storedAnswers[i]];
            }


            return progress;
        }

        public static SaveGameRequest SaveRequest(this ILevelProgressData data)
        {
            SaveGameRequest request = new SaveGameRequest()
            {
                currentDay = (int)data.CurrentDay,
                tryCount = (int)data.TryCount,
                lastCheckPoint = (int)data.LastCheckpoint,
                storedAnswers = new List<int>()
            };
            for (int i = 0; i < data.Level.Questions.Count; i++)
                request.storedAnswers.Add(-1);
            if (data.Answers == null) return request;
            for (int i = 0; i < data.Answers.Count; i++)
                if (data.Level.Questions[i].Quiz.Question is IMultipleChoiceQuestion multipleChoice)
                    request.storedAnswers[i] = multipleChoice.AnswerOptions.IndexOf(data.Answers[i]);
            return request;
        }

        public static StartGameRequest StartLevelRequest(this LevelControl levelControl, LevelData level)
        {
            return new StartGameRequest() { level = levelControl.IndexOf(level) + 1 };
        }

        public static EndGameRequest EndLevelRequest(this LevelEvaluationData data)
        {
            EndGameRequest request = new EndGameRequest()
            {
                correctAnswers = new List<bool>(data.AnswerEvaluations),
                isSuccess = data.AnswerEvaluations.All(eval => eval)
            };

            return request;
        }

        public static EndGameRequest AbortLevelRequest(this ILevelProgressData data)
        {
            EndGameRequest request = new EndGameRequest()
            {
                correctAnswers = new List<bool>(),
                isSuccess = false
            };

            return request;
        }
    }

    public class LevelProgressIntegration : ILevelProgressData
    {
        public uint LastCheckpoint { get; set; }
        public uint CurrentDay { get; set; }
        public uint TryCount { get; set; }
        public List<object> Answers { get; set; }
        public DayCondition Condition { get; set; }
        public LevelData Level { get; set; }

        public LevelProgressIntegration()
        {
        }

        public LevelProgressIntegration(ILevelProgressData data)
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