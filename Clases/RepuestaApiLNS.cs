namespace LNS_API.Clases
{
    public class RepuestaApiLNS
    {
        public bool success { get; set; } = true;
        public string message { get; set; } = "";
        public int responseCode { get; set; } = 2;
        public int registros_actualizados { get; set; } = 0;
        public float registros_creados { get; set; } = 0;
    }


}

