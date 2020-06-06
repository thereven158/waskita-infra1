namespace Agate.WaskitaInfra1.Server.Request
{
    [System.Serializable]
    public class LoginRequest : ValidateRequest
    {
        public string userId;
        public string password;

        public LoginRequest()
        {
        }
        public LoginRequest(ValidateRequest request) : base(request)
        {
        }
    }
}
