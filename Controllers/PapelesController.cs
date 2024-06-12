﻿using LNS_API.Clases;
using LNS_API.Clases.CajasClass;
using LNS_API.Clases.PapelesClass;
using LNS_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LNS_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PapelesController : ControllerBase
    {
        private readonly IPapel_FileMaker _Updates;
        private readonly ILogin _Login;

        public PapelesController(IPapel_FileMaker Updates, ILogin login)
        {
            _Updates = Updates;
            _Login = login;

        }

        [HttpPost]
        public async Task<IActionResult> CreatePaper(List<InsumoPapel> papelesUpdate)
        {
            int cantidadCreada = 0;
            int cantidadNoCreada = 0;
            string token = await _Login.GetTokeAsync("Papeles");
            string respuesta = string.Empty;
            foreach (var item in papelesUpdate)
            {
                newPapel Papeles = new newPapel();
                Papeles.fieldData = item;
                respuesta += await _Updates.CreatePapeles(Papeles, token);
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
        public async Task<IActionResult> UpdateCostPaper(PapelesUpdate papelesUpdate)
        {
            try
            {
                messajeClaseUpdates respuesta =await _Updates.UpdatePapeles(papelesUpdate);

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
