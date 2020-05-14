using System;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class StringChoiceQuiz : ScriptableMultipleChoiceQuiz<string>
    {
        [SerializeField]
        private StringChoiceQuestion _question;

        protected override BasicMultipleChoiceQuestion<string> Question => _question;
    }

    [Serializable]
    public class StringChoiceQuestion : BasicMultipleChoiceQuestion<string> { };
}