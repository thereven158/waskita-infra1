using System;
using A3.Quiz;
using Agate.SugiSuma.Quiz;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    [Serializable]
    public class SerializableQuestion: IQuestion
    {
        [SerializeField]
        private ScriptableQuiz _quiz = default;

        [SerializeField]
        private string _displayName = default;
        
        [SerializeField]
        private string _category = default;

        [SerializeField]
        [TextArea]
        private string _wrongExplanation = default;

        public string WrongExplanation => _wrongExplanation;
        public string Category => _category;
        public string DisplayName => _displayName;
        public IQuiz Quiz => _quiz.Quiz;
    }
}