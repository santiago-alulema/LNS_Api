using LNS_API.Clases.PapelesClass;
using LNS_API.Clases;
using LNS_API.Clases.InsumosClass;

namespace LNS_API.Interfaces
{
    public interface IInsumos
    {
        public Task<messajeClaseUpdates> UpdateInsumos(PapelesUpdate papelesUp);
        public Task<String> CreateInsumos(NewInsumos papelesUp, string token);
        public Task<string> ObtenerIDInsumosAsync(string parameterSearch, string token);
    }
}
