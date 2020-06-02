using Agate.WaskitaInfra1;
using Agate.WaskitaInfra1.UserInterface.QuestionList;
using Agate.WaskitaInfra1.Utilities;
using UnityEngine;

namespace Experimental
{
    public class QuestionListTestSceneControl : MonoBehaviour
    {
        [SerializeField]
        private QuestionListInteractionDisplay _questionListInteractionDisplay = default;

        [SerializeField]
        private ScriptableLevelProgress _checklistsProgress = default;

        private void Start()
        {
            _questionListInteractionDisplay.Open(
                _checklistsProgress.QuestionListViewData(),
                new QuestionListInteractionData()
                {
                    OnDataInteraction = Debug.Log,
                    OnFinishButton = () => Debug.Log("Finish"),
                }
            );
        }
    }
}