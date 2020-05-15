using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class StringChoiceQuiz : ScriptableMultipleChoiceQuiz<string>
    {
        [SerializeField]
        private StringChoiceQuestion _question = default;

        protected override BasicMultipleChoiceQuestion<string> Question => _question;
    }

    [Serializable]
    public class StringChoiceQuestion : BasicMultipleChoiceQuestion<string>
    {
        [SerializeField]
        [TextArea]
        private string _message = default;

        [SerializeField]
        private string _answer = default;

        [SerializeField]
        private List<string> _answerOptions = default;

        public override string Message => _message;
        public override string Answer => _answer;
        public override List<string> AnswerOptions => _answerOptions;
    };
}