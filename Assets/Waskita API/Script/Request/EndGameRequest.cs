using System.Collections.Generic;

namespace Agate.Waskita.Request
{
    [System.Serializable]
    public class EndGameRequest : BasicRequest
    {
        public List<bool> correctAnswers;

        public bool isSuccess;

    }
}
