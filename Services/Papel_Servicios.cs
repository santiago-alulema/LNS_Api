using LNS_API.Clases;
using LNS_API.Clases.PapelesClass;
using LNS_API.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace LNS_API.Services
{
    public class Papel_Servicios : IPapel_FileMaker
    {
        private readonly IConfiguration Configuration;
        private readonly ILogin _Login;
        private  HttpClient _httpClient;
        public Papel_Servicios(IConfiguration config,
                                ILogin Login)
        {
            _Login = Login;
            Configuration = config;
            _httpClient = new HttpClient();
        }
     

       

        //public Task<string> UpdatePapeles(List<PapelesUpdate> papelesUp)
        //{
        //    string codigosConFallas = String.Empty;
        //    for (int i = 0; i < papelesUp.Count; i++)
        //    {

        //    }

            
        //}

        public async Task<string> ObtenerIDPapelesAsync(string parameterSearch, string token)
        {
            try
            {
                _httpClient = new HttpClient();
                string urlBase = Configuration["URLBASE"];
                _httpClient.BaseAddress = new Uri(urlBase);
                RequestBody res = new RequestBody();
                ActualizarPapeles ac = new ActualizarPapeles();
                ac.query.Add(new Query() { CODIGO_REFERENCIA_ODOO = $"={parameterSearch}" });
                string json = JsonConvert.SerializeObject(ac);
                var todoItemJson = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                todoItemJson.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                using var httpResponseMessage = await _httpClient.PostAsync("databases/Papeles/layouts/Papeles%20del%20ERP/_find", todoItemJson);
                if (!httpResponseMessage.IsSuccessStatusCode)
                    return $"ERROR: {parameterSearch}";
                string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
               
                Papeles responseLn = JsonConvert.DeserializeObject<Papeles>(responseContent);

                return responseLn.Response.Data[0].RecordId;
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
           
        }



        public async Task<string> UpdatePapelProductAsync(string recordID, string newValue, string token)
        {
            try
            {
                _httpClient = new HttpClient();
                string urlBase = Configuration["URLBASE"];
                _httpClient.BaseAddress = new Uri(urlBase);

                EnviarJsonUpdatePapeles updateRecord = new EnviarJsonUpdatePapeles();
                updateRecord.fieldData.COS_COSTO = newValue;

                string json = JsonConvert.SerializeObject(updateRecord);
                var todoItemJson = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                todoItemJson.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                using var httpResponseMessage = await _httpClient.PatchAsync($"databases/Papeles/layouts/Papeles%20del%20ERP/records/{recordID}", todoItemJson);
                if (!httpResponseMessage.IsSuccessStatusCode)
                    return $"ERROR: {recordID}";
                string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

                RepuestaFileMaker responseLn = JsonConvert.DeserializeObject<RepuestaFileMaker>(responseContent);

                return responseLn.messages[0].message;
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }

        }



        public async Task<messajeClaseUpdates> UpdatePapeles(PapelesUpdate papelesUp)
        {
            _httpClient = new HttpClient();
            string urlBase = Configuration["URLBASE"];
            _httpClient.BaseAddress = new Uri(urlBase);
            string token = await _Login.GetTokeAsync("Papeles");
            string respusta = string.Empty;
            int registrosActualizados = 0;
            foreach (var item in papelesUp.productos)
            {
                var recordID = await ObtenerIDPapelesAsync(item.codigo, token);
                string respuestaUpdate = String.Empty;
                if (!recordID.Contains("ERROR")) {
                    respuestaUpdate = await UpdatePapelProductAsync(recordID, item.costo, token);
                }
                else
                {
                    respusta += $"Inconveniente en:  {recordID} ";
                }
                if (!respuestaUpdate.Contains("OK"))
                {
                    respusta += $"Inconveniente en:  {recordID} ";
                }
                else
                {
                    registrosActualizados++;
                }

            }
            return new messajeClaseUpdates() { cantidadUpdate= registrosActualizados , message= respusta };
        }

        public async Task<string> CreatePapeles(newPapel papelesUp, string token)
        {
            try
            {
                _httpClient = new HttpClient();
                string urlBase = Configuration["URLBASE"];
                string json = JsonConvert.SerializeObject(papelesUp);
                var todoItemJson = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                todoItemJson.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                using var httpResponseMessage = await _httpClient.PostAsync(urlBase+"databases/Papeles/layouts/Papeles%20del%20ERP/records", todoItemJson);
                if (!httpResponseMessage.IsSuccessStatusCode)
                    return $"ERROR: {papelesUp.fieldData.CODIGO_REFERENCIA_ODOO}";
                string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

                Papeles responseLn = JsonConvert.DeserializeObject<Papeles>(responseContent);

                return "";
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

        //public Task<messajeClaseUpdates> CreatePapeles(newPapel papelesUp)
        //{
        //    try
        //    {
        //        string urlBase = Configuration["URLBASE"];
        //        _httpClient.BaseAddress = new Uri(urlBase);
        //        RequestBody res = new RequestBody();
        //        ActualizarPapeles ac = new ActualizarPapeles();
        //        ac.query.Add(new Query() { CODIGO_REFERENCIA_ODOO = $"={parameterSearch}" });
        //        string json = JsonConvert.SerializeObject(ac);
        //        var todoItemJson = new StringContent(
        //            json,
        //            Encoding.UTF8,
        //            "application/json");

            //        todoItemJson.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            //        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //        using var httpResponseMessage = await _httpClient.PostAsync("databases/Papeles/layouts/Papeles%20del%20ERP/_find", todoItemJson);
            //        if (!httpResponseMessage.IsSuccessStatusCode)
            //            return $"ERROR: {parameterSearch}";
            //        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            //        Papeles responseLn = JsonConvert.DeserializeObject<Papeles>(responseContent);

            //        return responseLn.Response.Data[0].RecordId;
            //    }
            //    catch (Exception ex)
            //    {
            //        return $"ERROR: {ex.Message}";
            //    }
            //}
    }
}
