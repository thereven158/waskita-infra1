using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class LevelControl : MonoBehaviour
    {
        [SerializeField]
        private List<LevelDataScriptableObject> _levels = default;

        public LevelData GetLevel(int index)
        {
            return _levels[index].Object;
        }

        public int IndexOf(LevelData level)
        {
            return _levels.FindIndex(lvl => lvl.Object == level);
        }

        public IEnumerable<LevelData> Levels => _levels.Select(level => level.Object);
    }
}