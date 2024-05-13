namespace LNS_API.Clases
{
   


    public class ActualizarPapeles
    {
        public List<Query> query { get; set; } = new List<Query>();
    }

    public class Query
    {
        public string CODIGO_REFERENCIA_ODOO { get; set; }
    }

}
