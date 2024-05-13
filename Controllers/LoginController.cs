using LNS_API.Clases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LNS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly EndPointLNS endPointLNS;

        private readonly HttpClient _httpClient;
        public LoginController(IConfiguration config)
        {
            Configuration = config;
            endPointLNS = new EndPointLNS();
            _httpClient = new HttpClient();
        }


        [HttpPost]
        public async Task<IActionResult> LogInAsync(LoginUser user)
        {
            try
            {
                string urlBase = Configuration["URLBASE"];
                _httpClient.BaseAddress = new Uri(urlBase);
                string authHeaderValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user.UserName}:{user.Password}"));
                RequestBody res = new RequestBody();
                FmDataSource ss = new FmDataSource
                {
                    database = "default",
                    password = user.Password,
                    username = user.UserName
                };
                res.fmDataSource.Add(ss);
                string json = JsonConvert.SerializeObject(res);
                var todoItemJson = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                todoItemJson.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeaderValue);

                using var httpResponseMessage = await _httpClient.PostAsync(endPointLNS.LOGIN, todoItemJson);
                string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                if (!httpResponseMessage.IsSuccessStatusCode)
                    BadRequest($"ERROR: {responseContent}");
                ResponseLogin responseLn = JsonConvert.DeserializeObject<ResponseLogin>(responseContent);
                return Ok(new { token = responseLn.response.token });
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = 500
                }; ;
            }
        }
    }
}
