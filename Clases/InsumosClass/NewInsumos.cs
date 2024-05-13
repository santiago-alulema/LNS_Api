namespace LNS_API.Clases.InsumosClass
{
  

    public class NewInsumos
    {
        public Insumo fieldData { get; set; }
    }

    public class Insumo
    {
        public string Mat_Codigo { get; set; }
        public string CODIGO_REFERENCIA_ODOO { get; set; }
        public float COSTO { get; set; }
        public string UnidadCompra { get; set; }
        public string UnidadConsumo { get; set; }
        public string MAT_Nombre { get; set; }
    }
}
