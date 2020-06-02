namespace Agate.WaskitaInfra1.Server.Request
{
    [System.Serializable]
    public class StartGameRequest : ValidateRequest
    {
        public int level;

        public StartGameRequest()
        {
        }

        public StartGameRequest(ValidateRequest request) : base(request)
        {
        }
    }
}
