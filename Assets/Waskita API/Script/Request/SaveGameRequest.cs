using System.Collections.Generic;
using Agate.Waskita.Request.Data;

namespace Agate.Waskita.Request
{
    [System.Serializable]
    public class SaveGameRequest : BasicRequest
    {
        public int lastCheckPoint;
        public int currentDay;
        public int tryCount;
        public DayCondition dayCondition;
        public List<QuizAnswer> answer;

    }
}

