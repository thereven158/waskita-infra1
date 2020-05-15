using System.Collections.Generic;
using A3.Quiz;
using A3.Quiz.Unity;

namespace Agate.WaskitaInfra1.Level
{
    public abstract class ScriptableMultipleChoiceQuiz<TData> : ScriptableQuiz
    {
        protected abstract BasicMultipleChoiceQuestion<TData> Question { get; }

        private BasicMultipleChoiceQuiz<TData> _quiz;

        public override IQuiz Quiz
        {
            get
            {
                _quiz = _quiz ?? new BasicMultipleChoiceQuiz<TData>(Question);
                return _quiz;
            }
        }
    }

    public interface ITAnswerQuestion<out TAnswer>
    {
        TAnswer Answer { get; }
    }

    public interface IMultipleChoiceQuestion<TAnswer> : ITAnswerQuestion<TAnswer>
    {
        List<TAnswer> AnswerOptions { get; }
    }

    public interface IMessageQuestion
    {
        string Message { get; }
    }

    public abstract class BasicMultipleChoiceQuestion<TAnswer> : IMessageQuestion, IMultipleChoiceQuestion<TAnswer>
    {
        public abstract string Message { get; }
        public abstract TAnswer Answer { get; }
        public abstract List<TAnswer> AnswerOptions { get; }
    }

    public class BasicMultipleChoiceQuiz<TAnswer> : Quiz<BasicMultipleChoiceQuestion<TAnswer>, TAnswer>
    {
        public BasicMultipleChoiceQuiz(BasicMultipleChoiceQuestion<TAnswer> question)
        {
            Question = question;
        }

        public override bool IsCorrect(TAnswer answer)
            => answer.Equals(Question.Answer);

        public override BasicMultipleChoiceQuestion<TAnswer> Question { get; }
    }
}