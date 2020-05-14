using System;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class ScriptableImgTextChoiceQuiz : ScriptableMultipleChoiceQuiz<ScriptableImgText>
    {
        [SerializeField]
        private ImgTextChoiceQuestion _question;

        protected override BasicMultipleChoiceQuestion<ScriptableImgText> Question => _question;
    }

    [Serializable]
    public class ImgTextChoiceQuestion : BasicMultipleChoiceQuestion<ScriptableImgText>
    {
    }
}