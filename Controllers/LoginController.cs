using LNS_API.Clases;
using LNS_API.Clases.LoginClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
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


        [HttpPost("login-ws")]
        public object Login( [FromBody] Login_WS login)
        {
            string usuario = Configuration["USERAPI"];
            string password = Configuration["PASSWORDAPI"];

            if (usuario == login.username && password == login.password)
            {
                string JWTtoken = GenerateJWTToken(login.username);
                return Ok(new
                {
                    status = true,
                    token = JWTtoken
                });
            }
            return BadRequest(new
            {
                status = true,
                token = ""
            });
        }

        private string GenerateJWTToken(string user)
        {
            var claims = new List<Claim> {
        new Claim(ClaimTypes.Name, user),
    };
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"])
                        ),
                    SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
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
