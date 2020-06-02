using Agate.WaskitaInfra1.Server.Responses.Data;

namespace Agate.WaskitaInfra1.Server.Responses
{
    [System.Serializable]
    public class LoginResponse : BasicResponse
    {
        public ServerGameProgressData gameProgress;
        public ServerLevelProgressData levelProgress;

        public string name;
        public string token;
        public int tokenDuration;
    }
}
