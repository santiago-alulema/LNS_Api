using LNS_API.Clases;
using LNS_API.Clases.AllMaterialClass;
using LNS_API.Clases.CajasClass;
using LNS_API.Clases.InsumosClass;
using LNS_API.Clases.PapelesClass;
using LNS_API.Clases.PlacasClass;
using LNS_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LNS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllMateriaPrimaController : ControllerBase
    {
        private readonly ICajas _Updates;
        private readonly IInsumos _UpdatesInsumos;
        private readonly IPlacas _UpdatesPlacas;
        private readonly IPapel_FileMaker _UpdatesPapeles;
        private readonly ILogin _Login;
        int cantidadCreada = 0;
        int cantidadNoCreada = 0;
        int cantidadActualizados = 0;
        string mensaje;

        public AllMateriaPrimaController(ICajas Updates, 
                                         ILogin login,
                                         IInsumos updatesInsumos,
                                         IPlacas updatesPlacas,
                                         IPapel_FileMaker updatesPapeles)
        {
            _Updates = Updates;
            _Login = login;
            _UpdatesInsumos = updatesInsumos;
            _UpdatesPlacas = updatesPlacas;
            _UpdatesPapeles = updatesPapeles;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateMateriaAsync(MaterialType Materiales)
        {
            RepuestaApiLNS response = new RepuestaApiLNS();
            foreach (var materia in Materiales.ListaMateriales)
            {
                switch (materia.TypeMaterial)
                {
                    case "CAJAS":
                        response = await CajasProcessAsync(new List<material>() { materia });
                        break;
                    case "INSUMOS":
                        response = await InsumosProcessAsync(new List<material>() { materia });
                        break;
                    case "PLACAS":
                        response = await PlacasProcessAsync(new List<material>() { materia });
                        break;
                    case "PAPELES":
                        response = await PapelesProcessAsync(new List<material>() { materia });
                        break;
                    default:
                        break;
                }
            }
            return Ok(new RepuestaApiLNS
            {
                success = cantidadNoCreada > 0 || mensaje.Trim().Length > 0 ? false : true,
                message = $"{mensaje}" ,
                registros_creados = cantidadCreada,
                registros_actualizados = cantidadActualizados,
                responseCode = '2'

            });
        }

        private async Task<RepuestaApiLNS> CajasProcessAsync(List<material> listaMateriales)
        {
            
            string token = await _Login.GetTokeAsync("Procesos");
            string respuesta = string.Empty;
            PapelesUpdate productosUpdate = new PapelesUpdate();

            foreach (var item in listaMateriales)
            {

                string CodigoOdoo = await _Updates.ObtenerIdCajaAsync(item.codigo_referencia_odoo, token);
                if (CodigoOdoo.Contains("ERROR"))
                {
                    CajasCreate CAJAS = new CajasCreate();
                    CajasC cajasC = new CajasC()
                    {
                        caja_vlor_sin_iva = item.costo_unitario,
                        CODIGO_REFERENCIA_ODOO = item.codigo_referencia_odoo,
                        cdgo_caja = item.mat_id,
                        Descripcion = item.mat_nombre,
                        FECHA_ULTIMO_COSTO = item.fecha_ultimo_costo
                    };
                    CAJAS.fieldData = cajasC;
                    respuesta += await _Updates.CreateCajas(CAJAS, token);
                    if (respuesta.Contains("ERROR"))
                    {
                        cantidadNoCreada++;
                    }
                    else
                    {
                        cantidadCreada++;
                    }
                }
                else
                {
                    Producto p = new Producto() { codigo = item.codigo_referencia_odoo, costo = item.costo_unitario };
                    productosUpdate.productos.Add(p);
                }
            }

            messajeClaseUpdates respuestaUpdate;
            if (productosUpdate.productos.Count()>0)
            {
                respuestaUpdate = await _Updates.UpdateCajas(productosUpdate);
                cantidadActualizados += respuestaUpdate.cantidadUpdate;
                mensaje += respuestaUpdate.message;
            }
            mensaje += respuesta;

            return new RepuestaApiLNS();
            //{
            //    success = cantidadNoCreada > 0 || respuestaUpdate.message.Trim().Length > 0 ? false : true,
            //    message = $"{respuesta} {respuestaUpdate.message}" ,
            //    registros_creados = cantidadCreada,
            //    registros_actualizados = respuestaUpdate.cantidadUpdate,
            //    responseCode = '2'

            //};
        }



        private async Task<RepuestaApiLNS> InsumosProcessAsync(List<material> listaMateriales)
        {
           
            string token = await _Login.GetTokeAsync("Procesos");
            string respuesta = string.Empty;
            PapelesUpdate productosUpdate = new PapelesUpdate();

            foreach (var item in listaMateriales)
            {

                string CodigoOdoo = await _UpdatesInsumos.ObtenerIDInsumosAsync(item.codigo_referencia_odoo, token);
                if (CodigoOdoo.Contains("ERROR"))
                {
                    NewInsumos insumos = new NewInsumos();
                    InsumoFileMaker newInsumo = new InsumoFileMaker()
                    {
                        CODIGO_REFERENCIA_ODOO = item.codigo_referencia_odoo,
                        COSTO = item.costo_unitario,
                        Fecha_Ultimo_Costo = item.fecha_ultimo_costo,
                        UnidadCompra = item.unidadcompra,
                        UnidadConsumo = item.unidadconsumo,
                        MAT_Nombre = item.mat_nombre,
                        Mat_Codigo = item.mat_id
                        
                    };
                    insumos.fieldData = newInsumo;
                    respuesta += await _UpdatesInsumos.CreateInsumos(insumos, token);
                    if (respuesta.Contains("ERROR"))
                    {
                        cantidadNoCreada++;
                    }
                    else
                    {
                        cantidadCreada++;
                    }
                }
                else
                {
                    Producto p = new Producto() { codigo = item.codigo_referencia_odoo, costo = item.costo_unitario };
                    productosUpdate.productos.Add(p);
                }
            }
            messajeClaseUpdates respuestaUpdate;
            if (productosUpdate.productos.Count() > 0)
            { 
                 respuestaUpdate = await _UpdatesInsumos.UpdateInsumos(productosUpdate);
                cantidadActualizados += respuestaUpdate.cantidadUpdate;
                mensaje += respuestaUpdate.message;
            }
            mensaje += respuesta;

            return new RepuestaApiLNS()
            //{
            //    success = cantidadNoCreada > 0 || respuestaUpdate.message.Trim().Length > 0 ? false : true,
            //    message = $"{respuesta} {respuestaUpdate.message}",
            //    registros_creados = cantidadCreada,
            //    registros_actualizados = respuestaUpdate.cantidadUpdate,
            //    responseCode = '2'

            //}
            ;
        }

        

        private async Task<RepuestaApiLNS> PlacasProcessAsync(List<material> listaMateriales)
        {
            
            string token = await _Login.GetTokeAsync("Procesos");
            string respuesta = string.Empty;
            PapelesUpdate productosUpdate = new PapelesUpdate();

            foreach (var item in listaMateriales)
            {

                string CodigoOdoo = await _UpdatesPlacas.ObtenerIdPlacaAsync(item.codigo_referencia_odoo, token);
                if (CodigoOdoo.Contains("ERROR"))
                {
                    CrearPlaca placa = new CrearPlaca();
                    PlacaNew newPlacas = new PlacaNew()
                    {
                        CODIGO_REFERENCIA_ODOO = item.codigo_referencia_odoo,
                        cdgo_placa = item.mat_id,
                        placa_vlor = item.costo_unitario,
                        FECHA_ULTIMO_COSTO = item.fecha_ultimo_costo,
                        placa_dscrpcion = item.mat_nombre,
                    };
                    placa.fieldData = newPlacas;
                    respuesta += await _UpdatesPlacas.CreatePlacas(placa, token);
                    if (respuesta.Contains("ERROR"))
                    {
                        cantidadNoCreada++;
                    }
                    else
                    {
                        cantidadCreada++;
                    }
                }
                else
                {
                    Producto p = new Producto() { codigo = item.codigo_referencia_odoo, costo = item.costo_unitario };
                    productosUpdate.productos.Add(p);
                }
            }
            messajeClaseUpdates respuestaUpdate;
            if (productosUpdate.productos.Count() > 0)
            { 
                 respuestaUpdate = await _UpdatesPlacas.UpdatePlacas(productosUpdate);
                cantidadActualizados += respuestaUpdate.cantidadUpdate;
                mensaje += respuestaUpdate.message;
            }
            mensaje += respuesta;

            return new RepuestaApiLNS()
            //{
            //    success = cantidadNoCreada > 0 || respuestaUpdate.message.Trim().Length > 0 ? false : true,
            //    message = $"{respuesta} {respuestaUpdate.message}",
            //    registros_creados = cantidadCreada,
            //    registros_actualizados = respuestaUpdate.cantidadUpdate,
            //    responseCode = '2'

            //}
            ;
        }



        private async Task<RepuestaApiLNS> PapelesProcessAsync(List<material> listaMateriales)
        {
           
            string token = await _Login.GetTokeAsync("Papeles");
            string respuesta = string.Empty;
            PapelesUpdate productosUpdate = new PapelesUpdate();

            foreach (var item in listaMateriales)
            {

                string CodigoOdoo = await _UpdatesPapeles.ObtenerIDPapelesAsync(item.codigo_referencia_odoo, token);
                if (CodigoOdoo.Contains("ERROR"))
                {
                    newPapel placa = new newPapel();
                    InsumoPapel newPlacas = new InsumoPapel()
                    {
                        CODIGO_REFERENCIA_ODOO = item.codigo_referencia_odoo,
                        Mat_id = item.mat_id,
                        COSTOHoja_sinIVA = item.costohoja_siniva,
                        UNIDADEMPAQUE_ERPtxt = item.unidadempaque_erp,
                        pro_Nombre = item.mat_nombre,
                    };
                    placa.fieldData = newPlacas;
                    respuesta += await _UpdatesPapeles.CreatePapeles(placa, token);
                    if (respuesta.Contains("ERROR"))
                    {
                        cantidadNoCreada++;
                    }
                    else
                    {
                        cantidadCreada++;
                    }
                }
                else
                {
                    Producto p = new Producto() { codigo = item.codigo_referencia_odoo, costo = item.costo_unitario };
                    productosUpdate.productos.Add(p);
                }
            }

            messajeClaseUpdates respuestaUpdate;
            if (productosUpdate.productos.Count() > 0)
            { 
                 respuestaUpdate = await _UpdatesPapeles.UpdatePapeles(productosUpdate);
                cantidadActualizados += respuestaUpdate.cantidadUpdate;
                mensaje += respuestaUpdate.message;
            }
            mensaje += respuesta;

            return new RepuestaApiLNS()
            //{
            //    success = cantidadNoCreada > 0 || respuestaUpdate.message.Trim().Length > 0 ? false : true,
            //    message = $"{respuesta} {respuestaUpdate.message}",
            //    registros_creados = cantidadCreada,
            //    registros_actualizados = respuestaUpdate.cantidadUpdate,
            //    responseCode = '2'

            //}
            ;
        }


    }
}
