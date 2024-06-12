using LNS_API.Clases;
using LNS_API.Clases.PapelesClass;

namespace LNS_API.Interfaces
{
    public interface IPapel_FileMaker
    {
        public Task<messajeClaseUpdates> UpdatePapeles(PapelesUpdate papelesUp);
        public Task<String> CreatePapeles(newPapel papelesUp, string token);
        public Task<string> ObtenerIDPapelesAsync(string parameterSearch, string token);

    }
}
