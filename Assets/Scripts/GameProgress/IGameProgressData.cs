using System;

namespace Agate.WaskitaInfra1.GameProgress
{
    public interface IGameProgressData
    {
        short MaxCompletedLevelIndex { get; }
        uint CompletionCount { get; }
        double PlayTime { get; }
    }
}