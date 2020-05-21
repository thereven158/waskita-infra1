namespace Agate.WaskitaInfra1.GameProgress
{
    internal class GameProgressData : IGameProgressData
    {
        public short MaxCompletedLevelIndex { get; set; }
        public uint CompletionCount { get; set; }
        public double PlayTime { get; set; }
        public bool EqualContent(IGameProgressData data)
        {
            return MaxCompletedLevelIndex.Equals(data.MaxCompletedLevelIndex) &&
                   CompletionCount.Equals(data.CompletionCount) &&
                   PlayTime.Equals(data.PlayTime);
        }

        public GameProgressData()
        {
            MaxCompletedLevelIndex = -1;
            CompletionCount = 0;
            PlayTime = 0;
        }

        public GameProgressData(IGameProgressData data)
        {
            MaxCompletedLevelIndex = data.MaxCompletedLevelIndex;
            CompletionCount = data.CompletionCount;
            PlayTime = data.PlayTime;
        }
    }
}