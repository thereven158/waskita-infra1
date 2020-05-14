using System;
using System.Collections.Generic;
using A3.Quiz;
using Agate.SugiSuma.Quiz;
using UnityEngine;

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

    public interface IMultipleChoiceQuestion<TAnswer>:ITAnswerQuestion<TAnswer>
    {
        List<TAnswer> AnswerOptions { get; }
    }

    public interface IMessageQuestion
    {
        string Message { get; }
    }

    [Serializable]
    public class BasicMultipleChoiceQuestion<TAnswer> : IMessageQuestion, IMultipleChoiceQuestion<TAnswer>
    {
        [SerializeField]
        private string _message;

        [SerializeField]
        private TAnswer _answer;

        [SerializeField]
        private List<TAnswer> _answerOptions;

        public string Message => _message;
        public TAnswer Answer => _answer;
        public List<TAnswer> AnswerOptions => _answerOptions;
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