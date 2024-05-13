using LNS_API.Clases;
using LNS_API.Clases.InsumosClass;
using LNS_API.Clases.PapelesClass;
using LNS_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LNS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsumosController : ControllerBase
    {
        private readonly IInsumos _Updates;
        private readonly ILogin _Login;

        public InsumosController(IInsumos Updates, ILogin login)
        {
            _Updates = Updates;
            _Login = login;

        }

        [HttpPost]
        public async Task<IActionResult> CreateInsumo(List<LNS_API.Clases.InsumosClass.Insumo> papelesUpdate)
        {
            int cantidadCreada = 0;
            int cantidadNoCreada = 0;
            string token = await _Login.GetTokeAsync("Procesos");
            string respuesta = string.Empty;
            foreach (var item in papelesUpdate)
            {
                NewInsumos insumos = new NewInsumos();
                insumos.fieldData = item;
                respuesta += await _Updates.CreateInsumos(insumos, token);
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
                messajeClaseUpdates respuesta = await _Updates.UpdateInsumos(papelesUpdate);
                
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
