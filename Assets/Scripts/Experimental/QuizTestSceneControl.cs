using Agate.SugiSuma.Quiz;
using Agate.WaskitaInfra1.UserInterface.Quiz;
using UnityEngine;

namespace Experimental
{
    public class QuizTestSceneControl :MonoBehaviour
    {
        [SerializeField]
        private QuizDisplay _quizDisplay = default;
        [SerializeField]
        private ScriptableQuiz quiz = default;

        private void Start()
        {
            _quizDisplay.Init();
            _quizDisplay.DisplayQuiz(quiz.Quiz);
        }
    }
}