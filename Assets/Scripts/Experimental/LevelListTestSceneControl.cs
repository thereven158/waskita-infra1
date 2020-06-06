using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.UserInterface.LevelList;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Experimental
{
    public class LevelListTestSceneControl : MonoBehaviour
    {
        [SerializeField]
        private LevelDataListDisplay _levelDisplay = default;
        [SerializeField]
        private List<LevelDataScriptableObject> _levels = default;

        private void Start()
        {
            _levelDisplay.OpenList(_levels.Select(level => level.Object), Debug.Log);
        }
    }
}