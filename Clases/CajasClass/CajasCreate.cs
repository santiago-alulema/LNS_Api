namespace LNS_API.Clases.CajasClass
{

    public class CajasCreate
    {
        public CajasC fieldData { get; set; }
    }

    public class CajasC
    {
        public string cdgo_caja { get; set; }
        public string CODIGO_REFERENCIA_ODOO { get; set; }
        public float caja_vlor_sin_iva { get; set; }
        public string placa_dscrpcion { get; set; }
    }


}
