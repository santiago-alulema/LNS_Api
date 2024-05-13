namespace LNS_API.Clases
{

    public class PapelesUpdate
    {
        public List<Producto> productos { get; set; }
    }

    public class Producto
    {
        public string codigo { get; set; }
        public Decimal costo { get; set; }
    }

}
