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
        private ScriptableQuiz _quiz;

        [SerializeField]
        private string _category;

        [SerializeField]
        private string _wrongExplanation;

        public string WrongExplanation => _wrongExplanation;
        public string Category => _category;
        public IQuiz Quiz => _quiz.Quiz;
    }
}