namespace LNS_API.Clases.InsumosClass
{
  

    public class NewInsumos
    {
        public InsumoFileMaker fieldData { get; set; } =  new InsumoFileMaker();
    }

    public class InsumoFileMaker
    {
        public string Mat_Codigo { get; set; }
        public string CODIGO_REFERENCIA_ODOO { get; set; }
        public string COSTO { get; set; }
        public string UnidadCompra { get; set; }
        public string UnidadConsumo { get; set; }
        public string MAT_Nombre { get; set; }
        public string Fecha_Ultimo_Costo { get; set; }
    }
}
