namespace LNS_API.Clases.PlacasClass
{
    public class CrearPlaca
    {
        public PlacaNew fieldData { get; set; }
    }

    public class PlacaNew
    {
        public string Mat_Codigo { get; set; }
        public string CODIGO_REFERENCIA_ODOO { get; set; }
        public float COSTO { get; set; }
        public string MAT_Nombre { get; set; }
    }
}
