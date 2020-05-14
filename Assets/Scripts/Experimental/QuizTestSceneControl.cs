using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.UserInterface.Quiz;
using UnityEngine;

namespace Experimental
{
    public class QuizTestSceneControl : MonoBehaviour
    {
        [SerializeField]
        private QuizDisplay _quizDisplay = default;

        [SerializeField]
        private ScriptableQuestion question = default;

        private void Start()
        {
            _quizDisplay.Display(question.Quiz, (quiz1, o) => Debug.Log(o), null);
        }

    }
}