namespace LNS_API.Clases
{

    public class Papeles
    {
        public ResponsePapeles Response { get; set; }
        public MessagePapeles[] MessagePapeles { get; set; }
    }

    public class ResponsePapeles
    {
        public Datainfo DataInfo { get; set; }
        public Datum[] Data { get; set; }
    }

    public class Datainfo
    {
        public string Database { get; set; }
        public string Layout { get; set; }
        public string Table { get; set; }
        public int TotalRecordCount { get; set; }
        public int FoundCount { get; set; }
        public int ReturnedCount { get; set; }
    }

    public class Datum
    {
        public Fielddata FieldData { get; set; }
        public Portaldata PortalData { get; set; }
        public string RecordId { get; set; }
        public string ModId { get; set; }
    }

    public class Fielddata
    {
        // Propiedades correspondientes a los campos en el JSON de respuesta
    }

    public class Portaldata
    {
        // No hay campos en PortalData según el JSON proporcionado
    }

    public class MessagePapeles
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

}
