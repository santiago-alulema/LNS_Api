namespace LNS_API.Clases
{


    public class updateInsumos
    {
        public List<Insumo> insumos { get; set; }
    }

    public class Insumo
    {
        public string codigo { get; set; }
        public float costo { get; set; }
    }

}
