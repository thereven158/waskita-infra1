using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class StringSpriteChoiceQuiz : ScriptableMultipleChoiceQuiz<ScriptableImgText>
    {
        [SerializeField]
        private ImgTextChoiceQuestion _question = default;

        protected override BasicMultipleChoiceQuestion<ScriptableImgText> Question => _question;
    }

    [Serializable]
    public class ImgTextChoiceQuestion : BasicMultipleChoiceQuestion<ScriptableImgText>
    {
        [SerializeField]
        [TextArea]
        private string _message = default;

        [SerializeField]
        private ScriptableImgText _answer = default;

        [SerializeField]
        private List<ScriptableImgText> _answerOptions = default;

        public override string Message => _message;
        public override ScriptableImgText Answer => _answer;
        public override List<ScriptableImgText> AnswerOptions => _answerOptions;
    }
}