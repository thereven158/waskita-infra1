namespace Agate.Waskita.Request
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
