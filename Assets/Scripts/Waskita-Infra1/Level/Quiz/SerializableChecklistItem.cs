using System;
using A3.Quiz;
using Agate.SugiSuma.Quiz;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    [Serializable]
    public class SerializableChecklistItem: IChecklistItem
    {
        [SerializeField]
        private ScriptableQuiz _quiz;

        [SerializeField]
        private string _category;

        public string Category => _category;
        public IQuiz Quiz => _quiz.Quiz;
    }
}