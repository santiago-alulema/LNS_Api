using LNS_API.Clases;
using LNS_API.Clases.CajasClass;
using LNS_API.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace LNS_API.Services
{
    public class Cajas_Servicios : ICajas
    {
        private readonly IConfiguration Configuration;
        private readonly ILogin _Login;
        private readonly HttpClient _httpClient;
        private readonly string _DataBase = "Procesos";
        private readonly string _Layout = "CAJAS CORRUGADAS";

        public Cajas_Servicios(IConfiguration config,
                                ILogin Login)
        {
            _Login = Login;
            Configuration = config;
            _httpClient = new HttpClient();
        }

        public async Task<string> CreateCajas(CajasCreate papelesUp, string token)
        {
            try
            {
                string urlBase = Configuration["URLBASE"];
                string json = JsonConvert.SerializeObject(papelesUp);
                var todoItemJson = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                todoItemJson.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                using var httpResponseMessage = await _httpClient.PostAsync(urlBase + $"databases/{_DataBase}/layouts/{_Layout}/records", todoItemJson);
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

        public async Task<messajeClaseUpdates> UpdateCajas(PapelesUpdate papelesUp)
        {
            string token = await _Login.GetTokeAsync(_DataBase);
            string respusta = string.Empty;
            int registrosActualizados = 0;
            foreach (var item in papelesUp.productos)
            {
                var recordID = await ObtenerIdCajaAsync(papelesUp.productos[0].codigo, token);
                string respuestaUpdate = String.Empty;
                if (!recordID.Contains("ERROR"))
                {
                    respuestaUpdate = await UpdateCajaAsync(recordID, item.costo, token);
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
            return new messajeClaseUpdates() { cantidadUpdate = registrosActualizados, message = respusta };

        }

        public async Task<string> ObtenerIdCajaAsync(string parameterSearch, string token)
        {
            try
            {
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

                using var httpResponseMessage = await _httpClient.PostAsync($"databases/{_DataBase}/layouts/{_Layout}/_find", todoItemJson);
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

        public async Task<string> UpdateCajaAsync(string recordID, Decimal newValue, string token)
        {
            try
            {
                EnviarJsonUpdateCajas updateRecord = new EnviarJsonUpdateCajas();
                updateRecord.fieldData.caja_vlor_sin_iva = newValue;


                string json = JsonConvert.SerializeObject(updateRecord);
                var todoItemJson = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                todoItemJson.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                using var httpResponseMessage = await _httpClient.PatchAsync($"databases/{_DataBase}/layouts/{_Layout}/records/{recordID}", todoItemJson);
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
    }
}
