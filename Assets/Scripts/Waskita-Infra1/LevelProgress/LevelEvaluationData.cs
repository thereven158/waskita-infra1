using Agate.WaskitaInfra1.Animations;
using Agate.WaskitaInfra1.Level;
using System.Collections.Generic;

namespace Agate.WaskitaInfra1.LevelProgress
{
    public class LevelEvaluationData
    {
        public LevelEvaluationData(ILevelProgressData progressData)
        {
            Level = progressData.Level;
            AnswerEvaluations = new List<bool>();
            DayFinished = progressData.CurrentDay;
            TryCount = progressData.TryCount;
            for (int i = 0; i < progressData.Level.Questions.Count; i++)
                AnswerEvaluations.Add(progressData.Level.Questions[i].Quiz.IsCorrect(progressData.Answers[i]));
        }
        public LevelData Level;
        public uint TryCount;
        public List<bool> AnswerEvaluations;
        public uint DayFinished;

        public Queue<string> EvaluationMessages()
        {
            Queue<string> evalMessage = new Queue<string>();
            for (int i = 0; i < AnswerEvaluations.Count; i++)
            {
                if (AnswerEvaluations[i]) continue;
                evalMessage.Enqueue(Level.Questions[i].WrongExplanation);
            }

            return evalMessage;
        }
        public Queue<AnimationSceneControl> EvaluationAnims()
        {
            Queue<AnimationSceneControl> anims = new Queue<AnimationSceneControl>();
            for (int i = 0; i < AnswerEvaluations.Count; i++)
            {
                if (AnswerEvaluations[i]) continue;
                anims.Enqueue(Level.Questions[i].WrongAnimation);
            }

            return anims;
        }
    }
}