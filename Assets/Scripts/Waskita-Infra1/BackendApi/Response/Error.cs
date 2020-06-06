namespace Agate.WaskitaInfra1.Server.Responses
{


    [System.Serializable]
    public class Error
    {
        public string code;
        public string message;
        public Error()
        {
            code = null;
            message = null;
        }
    }

    [System.Serializable]
    public class InnerError
    {
        public string code;
        public InnerError innererror;
    }
}
