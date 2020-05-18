namespace Agate.Waskita.Request
{
    [System.Serializable]
    public class ValidateRequest : BasicRequest
    {
        public string clientID;
        public int gameVersion;
    }
}
