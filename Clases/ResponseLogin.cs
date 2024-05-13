namespace LNS_API.Clases
{

    public class ResponseLogin
    {
        public Response response { get; set; }
        public Message[] messages { get; set; }
    }

    public class Response
    {
        public string token { get; set; }
    }

    public class Message
    {
        public string code { get; set; }
        public string message { get; set; }
    }

}
