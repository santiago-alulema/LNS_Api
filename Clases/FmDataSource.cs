using System.Text.Json.Serialization;

namespace LNS_API.Clases
{
    public class FmDataSource
    {
        [JsonPropertyName("database")]
        public string database { get; set; }

        [JsonPropertyName("username")]
        public string username { get; set; }

        [JsonPropertyName("password")]
        public string password { get; set; }


    }

    public class RequestBody
    {
        [JsonPropertyName("fmDataSource")]
        public List<FmDataSource> fmDataSource { get; set; } = new List<FmDataSource>();


    }
}
