namespace CatalogWebApi.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task CompleteAsync();
    }
}
