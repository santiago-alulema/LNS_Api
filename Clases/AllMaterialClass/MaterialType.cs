﻿namespace LNS_API.Clases.AllMaterialClass
{
    public class MaterialType
    {
        public string TypeMaterial { get; set; }
        public List<material> ListaMateriales { get; set; }
    }


    public class material
    {
        public string codigo_referencia_odoo { get; set; }
        public decimal costo_unitario { get; set; }
        public string fecha_ultimo_costo { get; set; }
        public string unidadcompra { get; set; }
        public string unidadconsumo { get; set; }
        public string mat_nombre { get; set; }
        public decimal costohoja_siniva { get; set; }
        public string unidadempaque_erp { get; set; }
        public string mat_id { get; set; }

    }
}
