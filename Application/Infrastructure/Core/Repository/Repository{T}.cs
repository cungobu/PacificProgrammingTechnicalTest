using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Core
{
    internal class Repository<TEntity, TDbContext> : ReadRepository<TEntity, TDbContext>, IRepository<TEntity> 
        where TEntity : class
        where TDbContext : DbContext, new()
    {
        public Repository(IDbFactory<TDbContext> contextFactory, IUnitOfWork<TDbContext> unitOfWork) 
            : base(contextFactory, unitOfWork)
        {
        }

        public void Add(TEntity entity)
        {
            this.entities.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            this.entities.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            var removedEntites = this.entities.Where(expression);
            this.entities.RemoveRange(removedEntites);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            this.entities.RemoveRange(entities);
        }

        public void ExecuteSQL(string sql, params object[] paramertes)
        {
            this.Context.Database.ExecuteSqlRaw(sql, paramertes);
        }

        public void Update(TEntity entity)
        {
            this.entities.Update(entity);
        }
    }
}
