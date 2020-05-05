using A3.UserInterface;
using Agate.WaskitaInfra1.UserInterface;
using Agate.WaskitaInfra1.UserInterface.ChecklistList;
using Agate.WaskitaInfra1.UserInterface.LevelList;
using Agate.WaskitaInfra1.UserInterface.Quiz;
using UserInterface.LevelState;
using UnityEngine;

namespace Agate.WaskitaInfra1.SceneControl
{
    public class UserInterfaceSceneControl : MonoBehaviour
    {
        [SerializeField]
        private QuizDisplay _quizDisplay = default;

        [SerializeField]
        private LevelStateDisplay _levelStateDisplay = default;

        [SerializeField]
        private LevelDataListDisplay _levelDataListDisplay = default;

        [SerializeField]
        private LevelDataDisplay _levelDataDisplay = default;

        [SerializeField]
        private LevelProgressCheckListDisplay _checklistDisplay = default;

        [SerializeField]
        private UiDisplaysSystemBehavior _displaySystem = default;

        private void Start()
        {
            Main main = Main.Instance;
            _displaySystem.Init();
            Main.RegisterComponents(_quizDisplay, _levelDataListDisplay, _levelStateDisplay, _levelDataDisplay, _checklistDisplay, _displaySystem);
            _quizDisplay.Close();
            _levelStateDisplay.ToggleDisplay(false);
            _levelDataListDisplay.Close();
            _levelDataDisplay.ToggleDisplay(false);
            _checklistDisplay.Close();
            main.UiLoaded = true;
        }
    }
}