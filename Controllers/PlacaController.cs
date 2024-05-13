using LNS_API.Clases.InsumosClass;
using LNS_API.Clases;
using LNS_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LNS_API.Clases.PlacasClass;

namespace LNS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacaController : ControllerBase
    {

        private readonly IPlacas _Updates;
        private readonly ILogin _Login;

        public PlacaController(IPlacas Updates, ILogin login)
        {
            _Updates = Updates;
            _Login = login;

        }

        [HttpPost]
        public async Task<IActionResult> CreateInsumo(List<PlacaNew> papelesUpdate)
        {
            int cantidadCreada = 0;
            int cantidadNoCreada = 0;
            string token = await _Login.GetTokeAsync("Procesos");
            string respuesta = string.Empty;
            foreach (var item in papelesUpdate)
            {
                CrearPlaca insumos = new CrearPlaca();
                insumos.fieldData = item;
                respuesta += await _Updates.CreatePlacas(insumos, token);
                if (respuesta.Contains("ERROR"))
                {
                    cantidadNoCreada++;
                }
                else
                {
                    cantidadCreada++;
                }
            }

            return Ok(new RepuestaApiLNS()
            {
                success = cantidadNoCreada > 0 ? false : true,
                message = respuesta,
                registros_creados = cantidadCreada,

            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCostInsumo(PapelesUpdate papelesUpdate)
        {
            try
            {
                messajeClaseUpdates respuesta = await _Updates.UpdatePlacas(papelesUpdate);

                RepuestaApiLNS respuestaSend = new RepuestaApiLNS();
                respuestaSend.message = respuesta.message;
                if (respuesta.message.Trim().Length > 0)
                {
                    respuestaSend.success = false;
                    return BadRequest(respuestaSend);
                }
                respuestaSend.registros_actualizados = respuesta.cantidadUpdate;
                return Ok(respuestaSend);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
