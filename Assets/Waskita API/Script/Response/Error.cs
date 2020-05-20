namespace Agate.Waskita.Responses
{

    
    [System.Serializable]
    public class Error
    {
        public string code;
        public object[] details;
        public string id;
        public InnerError innererror;
        public string target;
        public Error()
        {
            code = null;
            id = null;
            target = null;
            details = null;
            innererror = null;
        }
    }

    [System.Serializable]
    public class InnerError
    {
        public string code;
        public InnerError innererror;
    }
}
