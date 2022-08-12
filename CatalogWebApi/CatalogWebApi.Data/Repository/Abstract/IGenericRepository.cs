using System.Linq.Expressions;

namespace CatalogWebApi.Data
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity> GetByIdAsync(int entityId);
        Task InsertAsync(Entity entity);
        void RemoveAsync(Entity entity);
        void Update(Entity entity);
        Task<IEnumerable<Entity>> GetAllAsync();
        Task<IEnumerable<Entity>> FindAsync(Expression<Func<Entity, bool>> expression);
    }
}
