using UnityEngine;
using Agate.WaskitaInfra1.UserInterface.ChecklistList;
using Agate.WaskitaInfra1;

namespace Experimental
{
    public class QuestionListTestSceneControl : MonoBehaviour
    {
        [SerializeField]
        private QuestionListInteractionDisplay _questionListInteractionDisplay;

        [SerializeField]
        private ScriptableLevelProgress _checklistsProgress;

        private void Start()
        {
            _questionListInteractionDisplay.Open(_checklistsProgress, Debug.Log, () => Debug.Log("Finish"));
            //Debug.Log(_checklistsProgress.Level.Quizzes);
        }
    }
}