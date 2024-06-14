namespace LNS_API.Clases.PapelesClass
{
    public class newPapel
    {
        public InsumoPapel fieldData { get; set; }
    }

    public class InsumoPapel
    {
        public string Mat_id { get; set; }
        public string CODIGO_REFERENCIA_ODOO { get; set; }
        public string COSTOHoja_sinIVA { get; set; }
        public string pro_Nombre { get; set; }
        public string UNIDADEMPAQUE_ERPtxt { get; set; }
    }

}
