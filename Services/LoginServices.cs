using LNS_API.Clases;
using LNS_API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace LNS_API.Services
{
    public class LoginServices : ILogin
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration Configuration;
        private readonly EndPointLNS endPointLNS;
        public LoginServices(IConfiguration config)
        {
            Configuration = config;
            _httpClient = new HttpClient();
            endPointLNS = new EndPointLNS();
        }
        public async Task<string> GetTokeAsync(string database)
        {

            try
            {
                string urlBase = Configuration["URLBASE"];
                string username = Configuration["usernameWS"];
                string password = Configuration["passwordWS"];

                _httpClient.BaseAddress = new Uri(urlBase);
                string authHeaderValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                RequestBody res = new RequestBody();
                FmDataSource ss = new FmDataSource
                {
                    database = database,
                    password = username,
                    username = password
                };
                res.fmDataSource.Add(ss);
                string json = JsonConvert.SerializeObject(res);
                var todoItemJson = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                todoItemJson.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeaderValue);

                using var httpResponseMessage = await _httpClient.PostAsync(endPointLNS.LOGIN.Replace("[DATABASE]",database), todoItemJson);
                string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                if (!httpResponseMessage.IsSuccessStatusCode)
                    return $"ERROR: {responseContent}";
                ResponseLogin responseLn = JsonConvert.DeserializeObject<ResponseLogin>(responseContent);
                return  responseLn.response.token;
            }
            catch (Exception ex)
            {
                return "ERROR: "+ex.Message;
                
            }
        }
    }
}
