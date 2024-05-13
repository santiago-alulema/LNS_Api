namespace LNS_API.Interfaces
{
    public interface ILogin
    {
        public Task<string> GetTokeAsync(string database);
    }
}
