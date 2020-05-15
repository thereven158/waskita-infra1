using System;
using System.Collections.Generic;
using Agate.SpriteSheet;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class SpriteSheetChoiceQuiz : ScriptableMultipleChoiceQuiz<ISpriteSheet>
    {
        [SerializeField]
        private SpriteSheetChoiceQuestion _question = default;

        protected override BasicMultipleChoiceQuestion<ISpriteSheet> Question => _question;
    }

    [Serializable]
    public class SpriteSheetChoiceQuestion : BasicMultipleChoiceQuestion<ISpriteSheet>
    {
        [SerializeField]
        [TextArea]
        private string _message = default;

        [SerializeField]
        private ISpriteSheet _answer = default;

        [SerializeField]
        private List<ISpriteSheet> _answerOptions = default;

        public override string Message => _message;
        public override ISpriteSheet Answer => _answer;
        public override List<ISpriteSheet> AnswerOptions => _answerOptions;
    };
}