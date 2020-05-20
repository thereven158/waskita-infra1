using Agate.Waskita.Responses;

namespace Agate.Waskita.Responses
{
    [System.Serializable]
    public class BasicResponse
    {
        public Error error;
        public string message;
        public int statusCode;
    }
}
