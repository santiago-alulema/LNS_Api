using LNS_API.Clases.InsumosClass;
using LNS_API.Clases;
using LNS_API.Clases.PlacasClass;

namespace LNS_API.Interfaces
{
    public interface IPlacas
    {
        public Task<messajeClaseUpdates> UpdatePlacas(PapelesUpdate papelesUp);
        public Task<String> CreatePlacas(CrearPlaca papelesUp, string token);
        public Task<string> ObtenerIdPlacaAsync(string parameterSearch, string token);
    }
}
