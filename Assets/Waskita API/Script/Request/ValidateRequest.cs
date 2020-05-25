namespace Agate.Waskita.Request
{
    [System.Serializable]
    public class ValidateRequest : BasicRequest
    {
        public string clientID;
        public string gameVersion;

        public ValidateRequest()
        {
        }
        
        public ValidateRequest(BasicRequest request) :base(request)
        {
        }

        public ValidateRequest(ValidateRequest request) : base(request)
        {
            clientID = request.clientID;
            gameVersion = request.gameVersion;
        }
    }
}
