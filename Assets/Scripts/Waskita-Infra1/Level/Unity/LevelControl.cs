using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class LevelControl : MonoBehaviour
    {
        [SerializeField]
        private List<LevelDataScriptableObject> _levels;

        public LevelData GetLevel(int index)
        {
            return _levels[index].Object;
        }

        public IEnumerable<LevelData> Levels => _levels.Select(level => level.Object);
    }
}