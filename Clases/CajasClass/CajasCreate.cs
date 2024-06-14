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
        public string caja_vlor_sin_iva { get; set; }
        public string Descripcion { get; set; }
        public string FECHA_ULTIMO_COSTO { get; set; }
        
    }


}
