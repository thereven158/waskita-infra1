using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.UserInterface;
using UnityEngine;

namespace Experimental
{
    public class LevelDataDisplayTestScene : MonoBehaviour
    {
        [SerializeField]
        private LevelDataDisplay _levelDisplay = default;
        [SerializeField]
        private LevelDataScriptableObject _level = default;

        private void Start()
        {
            _levelDisplay.OpenDisplay(_level.Object, Debug.Log, null);
        }
    }
}