namespace Agate.Waskita.Request
{
    [System.Serializable]
    public class BasicRequest
    {
        /// <summary>
        /// Request Id of message sender
        /// Used for identification of duplicate request (user resend request multiple times)
        /// </summary>
        public string requestId;

        /// <summary>
        /// Device Id of message sender
        /// Used for identification of duplicate request (user resend request multiple times)
        /// </summary>
        public string deviceId;

        public BasicRequest()
        {
        }

        public BasicRequest(BasicRequest request)
        {
            requestId = request.requestId;
            deviceId = request.deviceId;
        }
    }
}