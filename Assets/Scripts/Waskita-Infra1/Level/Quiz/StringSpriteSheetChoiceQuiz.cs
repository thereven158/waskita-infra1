using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class StringSpriteSheetChoiceQuiz : ScriptableMultipleChoiceQuiz<ITextSpriteSheet>
    {
        [SerializeField]
        private TextSpriteSheetQuestion _question = default;

        protected override BasicMultipleChoiceQuestion<ITextSpriteSheet> Question => _question;
    }

    [Serializable]
    public class TextSpriteSheetQuestion : BasicMultipleChoiceQuestion<ITextSpriteSheet>
    {
        [SerializeField]
        [TextArea]
        private string _message = default;

        [SerializeField]
        private ScriptableTextSpriteSheet _answer = default;

        [SerializeField]
        private List<ScriptableTextSpriteSheet> _answerOptions = default;

        private List<ITextSpriteSheet> _castedAnswerOptions;

        public override string Message => _message;
        public override ITextSpriteSheet Answer => _answer;

        public override List<ITextSpriteSheet> AnswerOptions
        {
            get
            {
                _castedAnswerOptions = _castedAnswerOptions ?? _answerOptions.Cast<ITextSpriteSheet>().ToList();
                return _castedAnswerOptions;
            }
        }
    }
}