using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.UserInterface.Display;
using Agate.WaskitaInfra1.UserInterface.Quiz;
using UnityEngine;

namespace Experimental
{
    public class QuizTestSceneControl : MonoBehaviour
    {
        [SerializeField]
        private QuizDisplay _quizDisplay = default;

        [SerializeField]
        private PopUpDisplay ResultDisplay = default;

        [SerializeField]
        private ScriptableQuestion question = default;



        private void Start()
        {
            _quizDisplay.Init();
            ResultDisplay.Init();
            OpenQuizDisplay();
        }
        private void OpenPopUp(string message)
        {
            ResultDisplay.Open(message, OpenQuizDisplay);
        }
        private void OpenQuizDisplay()
        {
            _quizDisplay.Display(question, (quiz1, o) => OpenPopUp($"answer is {quiz1.IsCorrect(o)}"), null);
        }

    }
}