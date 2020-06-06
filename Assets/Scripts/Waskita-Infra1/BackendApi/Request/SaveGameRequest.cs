using Agate.WaskitaInfra1.Server.Request.Data;
using System.Collections.Generic;

namespace Agate.WaskitaInfra1.Server.Request
{
    [System.Serializable]
    public class SaveGameRequest : BasicRequest
    {
        public int lastCheckPoint;
        public int currentDay;
        public int tryCount;
        public DayCondition dayCondition;
        public List<int> storedAnswers;

    }
}

