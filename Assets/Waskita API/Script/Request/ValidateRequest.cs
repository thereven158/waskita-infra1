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
            Set(request);
        }

        public void Set(ValidateRequest request)
        {
            clientID = request.clientID;
            gameVersion = request.gameVersion;
            base.Set(request);
        }
    }
}
