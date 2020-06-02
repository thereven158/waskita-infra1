using System.Collections.Generic;

namespace Agate.WaskitaInfra1.Server.Responses.Data
{
    [System.Serializable]
    public class ServerLevelProgressData
    {
        public int lastCheckpoint;
        public int currentDay;
        public int tryCount;
        public ServerDayCondition dayCondition;
        public int level;
        public List<int> storedAnswers;

    }
}