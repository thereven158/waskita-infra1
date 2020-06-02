using System.Collections.Generic;

namespace Agate.WaskitaInfra1.Server.Request
{
    [System.Serializable]
    public class EndGameRequest : BasicRequest
    {
        public List<bool> correctAnswers;

        public bool isSuccess;

    }
}
