using Agate.WaskitaInfra1.Server.Responses;

namespace Agate.WaskitaInfra1.Server.Responses
{
    [System.Serializable]
    public class BasicResponse
    {
        public Error error;
        public string message;
        public int statusCode;
    }
}
