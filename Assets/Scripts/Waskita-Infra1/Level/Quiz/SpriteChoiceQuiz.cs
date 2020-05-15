using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class SpriteChoiceQuiz : ScriptableMultipleChoiceQuiz<Sprite>
    {
        [SerializeField]
        private SpriteChoiceQuestion _question = default;

        protected override BasicMultipleChoiceQuestion<Sprite> Question => _question;
    }

    [Serializable]
    public class SpriteChoiceQuestion : BasicMultipleChoiceQuestion<Sprite>
    {
        [SerializeField]
        [TextArea]
        private string _message = default;

        [SerializeField]
        private Sprite _answer = default;

        [SerializeField]
        private List<Sprite> _answerOptions = default;

        public override string Message => _message;
        public override Sprite Answer => _answer;
        public override List<Sprite> AnswerOptions => _answerOptions;
    };
}