using Agate.WaskitaInfra1.UserInterface.LevelList;
using Agate.WaskitaInfra1.UserInterface.Quiz;
using UserInterface.LevelState;
using UnityEngine;

namespace Agate.WaskitaInfra1.SceneControl
{
    public class UserInterfaceSceneControl:MonoBehaviour
    {
        [SerializeField]
        private QuizDisplay _quizDisplay = default;

        [SerializeField]
        private LevelStateDisplay _levelStateDisplay = default;

        [SerializeField]
        private LevelDataListDisplay _levelDataListDisplay = default;

        private void Start()
        {
            Main main = Main.Instance;
            _quizDisplay.Init();
            _levelDataListDisplay.Init();
            Main.RegisterComponents(_quizDisplay, _levelDataListDisplay, _levelStateDisplay);
            main.UiLoaded = true;
        }
    }
}