﻿namespace LNS_API.Clases
{
    public class EnviarJsonUpdatePapeles
    {
        public FielddataUp fieldData { get; set; } = new FielddataUp();
    }

    public class FielddataUp
    {
        public Decimal COS_COSTO { get; set; }
    }


    //---------------------


    public class EnviarJsonUpdateInsumos
    {
        public FielddataUpInsumos fieldData { get; set; } = new FielddataUpInsumos();
    }

    public class FielddataUpInsumos
    {
        public Decimal Costo_Unitario { get; set; }
    }



    //---------------

    public class EnviarJsonUpdatePlaca
    {
        public FielddataUpPlaca fieldData { get; set; } = new FielddataUpPlaca();
    }

    public class FielddataUpPlaca
    {
        public Decimal placa_vlor { get; set; }
    }


    //---------------

    public class EnviarJsonUpdateCajas
    {
        public FielddataUpCaja fieldData { get; set; } = new FielddataUpCaja();
    }

    public class FielddataUpCaja
    {
        public Decimal caja_vlor_sin_iva { get; set; }
    }
}
