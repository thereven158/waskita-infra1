using System;
using System.Collections.Generic;
using A3.Quiz;
using Agate.SugiSuma.Quiz;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    [CreateAssetMenu(fileName = "TextQuiz", menuName = "WaskitaInfra1/Quiz/Text", order = 0)]
    public class ScriptableTextQuiz : ScriptableQuiz
    {
        [SerializeField]
        private TextQuestion _question = default;


        private TextQuiz _quiz;

        public override IQuiz Quiz
        {
            get
            {
                _quiz = _quiz ?? new TextQuiz(_question);
                return _quiz;
            }
        }
    }

    [Serializable]
    public struct TextQuestion
    {
        public string QuestionText;
        public string Answer;
        public List<string> Options;
    }

    public class TextQuiz : Quiz<TextQuestion, string>
    {
        public override bool IsCorrect(string answer)
        {
            return answer == Question.Answer;
        }

        public override TextQuestion Question { get; }

        public TextQuiz(TextQuestion question)
        {
            Question = question;
        }
    }
}