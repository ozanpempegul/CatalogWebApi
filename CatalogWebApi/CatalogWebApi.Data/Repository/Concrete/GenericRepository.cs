using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogWebApi.Data
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        protected readonly AppDbContext Context;
        private DbSet<Entity> entities;

        public GenericRepository(AppDbContext dbContext)
        {
            this.Context = dbContext;
            this.entities = Context.Set<Entity>();
        }

        public async Task<IEnumerable<Entity>> FindAsync(Expression<Func<Entity, bool>> expression)
        {
            return await entities.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<Entity>> GetAllAsync()
        {
            return await entities.AsNoTracking().ToListAsync();
        }

        public virtual async Task<Entity> GetByIdAsync(int entityId)
        {
            //return await entities.FindAsync(entityId);
            return await entities.Where(entity => EF.Property<int>(entity, "Id").Equals(entityId)).SingleOrDefaultAsync();
        }

        public async Task InsertAsync(Entity entity)
        {
            await entities.AddAsync(entity);
        }

        public void RemoveAsync(Entity entity)
        {
            //entities.Remove(entity);
            entity.GetType().GetProperty("isdeleted").SetValue(entity, true);
        }

        public void Update(Entity entity)
        {
            entities.Update(entity);
        }
    }
}
