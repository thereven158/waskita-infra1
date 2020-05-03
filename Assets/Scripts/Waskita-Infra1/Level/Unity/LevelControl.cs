using System.Collections.Generic;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class LevelControl : MonoBehaviour
    {
        [SerializeField]
        private List<LevelDataScriptableObject> _Levels;

        public LevelData GetLevel(int index)
        {
            return _Levels[index].Object;
        }
    }
}