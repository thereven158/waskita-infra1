using Agate.WaskitaInfra1;
using Agate.WaskitaInfra1.Level;
using UnityEngine;
using UserInterface.LevelState;

namespace Experimental
{
    public class LevelStateTestScene : MonoBehaviour
    {
        [SerializeField]
        private LevelStateDisplay _stateDisplay = default;
        [SerializeField]
        private LevelDataScriptableObject _level = default;

        private void Start()
        {
            _stateDisplay.OpenDisplay(_level.Object.State());
        }
    }
}