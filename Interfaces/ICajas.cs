using LNS_API.Clases.InsumosClass;
using LNS_API.Clases;
using LNS_API.Clases.CajasClass;

namespace LNS_API.Interfaces
{
    public interface ICajas
    {
        public Task<messajeClaseUpdates> UpdateCajas(PapelesUpdate papelesUp);
        public Task<String> CreateCajas(CajasCreate papelesUp, string token);

        public Task<string> ObtenerIdCajaAsync(string parameterSearch, string token);
    }
}
