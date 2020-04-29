using Agate.WaskitaInfra1.GameProgress;
using UnityEngine;

namespace Agate.WaskitaInfra1
{
    [CreateAssetMenu(fileName = "GameProgressData", menuName = "WaskitaInfra1/GameProgressData")]
    public class ScriptableGameProgress: ScriptableObject, IGameProgressData
    {
        [SerializeField]
        private short _maxCompletedLevelIndex;
        [SerializeField]
        private uint _completionCount;
        [SerializeField]
        private double _playTime;

        public short MaxCompletedLevelIndex => _maxCompletedLevelIndex;
        public uint CompletionCount => _completionCount;
        public double PlayTime => _playTime;
        public bool Equals(IGameProgressData data)
        {
            return base.Equals(data);
        }
    }
}