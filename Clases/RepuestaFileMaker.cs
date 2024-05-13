namespace LNS_API.Clases
{
   

    public class RepuestaFileMaker
    {
        public ResponseFile response { get; set; }
        public List<MessageFile> messages { get; set; } =new List<MessageFile>();
    }

    public class ResponseFile
    {
        public string modId { get; set; }
    }

    public class MessageFile
    {
        public string code { get; set; }
        public string message { get; set; }
    }

}
