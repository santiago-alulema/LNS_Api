namespace LNS_API.Clases.PlacasClass
{
    public class CrearPlaca
    {
        public PlacaNew fieldData { get; set; }
    }

    public class PlacaNew
    {
        public string cdgo_placa { get; set; }
        public string CODIGO_REFERENCIA_ODOO { get; set; }
        public string placa_vlor { get; set; }
        public String FECHA_ULTIMO_COSTO { get; set; }
        public string placa_dscrpcion { get; set; }
    }
}
