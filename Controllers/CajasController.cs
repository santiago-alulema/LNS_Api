﻿using LNS_API.Clases.PapelesClass;
using LNS_API.Clases;
using LNS_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LNS_API.Clases.CajasClass;
using LNS_API.Clases.PlacasClass;
using Microsoft.AspNetCore.Authorization;

namespace LNS_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CajasController : ControllerBase
    {

        private readonly ICajas _Updates;
        private readonly ILogin _Login;

        public CajasController(ICajas Updates, ILogin login)
        {
            _Updates = Updates;
            _Login = login;

        }


        [HttpPost]
        public async Task<IActionResult> CreateCajas(List<CajasC> papelesUpdate)
        {
            int cantidadCreada = 0;
            int cantidadNoCreada = 0;
            string token = await _Login.GetTokeAsync("Procesos");
            string respuesta = string.Empty;
            foreach (var item in papelesUpdate)
            {
                CajasCreate CAJAS = new CajasCreate();
                CAJAS.fieldData = item;
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

            return Ok(new RepuestaApiLNS()
            {
                success = cantidadNoCreada > 0 ? false : true,
                message = respuesta,
                registros_creados = cantidadCreada,

            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCostCajas(PapelesUpdate papelesUpdate)
        {
            try
            {
                messajeClaseUpdates respuesta = await _Updates.UpdateCajas(papelesUpdate);

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
