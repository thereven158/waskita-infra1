using A3.Quiz;
using A3.Quiz.Unity;
using Agate.WaskitaInfra1.Animations;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    [CreateAssetMenu(fileName = "ScriptableQuestions", menuName = "WaskitaInfra1/Question")]
    public class ScriptableQuestion : ScriptableObject, IQuestion
    {
        [SerializeField]
        private ScriptableQuiz _quiz = default;

        [SerializeField]
        private string _displayName = default;

        [SerializeField]
        private string _category = default;

        [Header("Wrong Info")]
        [SerializeField]
        private AnimationSceneControl _wrongAnim = default;

        [SerializeField]
        [TextArea]
        private string _wrongExplanation = default;

        public string WrongExplanation => _wrongExplanation;
        public string Category => _category;
        public string DisplayName => _displayName;
        public IQuiz Quiz => _quiz.Quiz;

        public AnimationSceneControl WrongAnimation => _wrongAnim;
    }
}