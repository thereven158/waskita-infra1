namespace Agate.Waskita.Request
{
    [System.Serializable]
    public class LoginRequest: BasicRequest
    {
        public string userId;
        public string password;
        public string clientID;
        public int gameVersion;
    }
}
