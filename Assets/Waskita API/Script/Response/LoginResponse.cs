using Agate.Waskita.Responses.Data;

namespace Agate.Waskita.Responses
{
    [System.Serializable]
    public class LoginResponse : BasicResponse
    {
        public GameProgressData gameProgress;
        public LevelProgressData levelProgress;

        public string name;
        public string token;
        public int tokenDuration;
    }
}
