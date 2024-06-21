namespace RamenGo.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetAsync(string id);

        Task AddAsync(T item);
    }
}