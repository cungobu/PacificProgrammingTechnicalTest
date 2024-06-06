using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Core
{
    internal class ReadRepository<TEntity, TDbContext>
        : IReadRepository<TEntity>, IDbContextContainer where TEntity : class        
        where TDbContext : DbContext, new()
    {
        protected DbContext context;
        internal protected DbContext Context => context ?? (context = contextFactory.Init());
        private readonly IDbFactory<TDbContext> contextFactory;
        protected readonly IUnitOfWork<TDbContext> unitOfWork;
        protected readonly DbSet<TEntity> entities;

        public ReadRepository(IDbFactory<TDbContext> contextFactory, IUnitOfWork<TDbContext> unitOfWork)
        {
            this.contextFactory = contextFactory;
            this.unitOfWork = unitOfWork;
            this.entities = this.Context.Set<TEntity>();
        }

        #region Implementation of IDbContextContainer contract
        DbContext IDbContextContainer.Context => this.Context;
        #endregion

        public virtual bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return this.entities.Any(expression);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> expression)
        {
            return this.entities.Count(expression);
        }

        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> expression)
        {
            return this.entities.Where(expression);
        }

        public virtual TEntity Find(object entityId)
        {
            return this.entities.Find(entityId);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return this.entities;
        }

        public virtual IQueryable<TEntity> Queryable()
        {
            return this.entities;
        }

        public virtual IQueryable<TEntity> FromSqlRaw(string sql, params object[] parameters)
        {
            return this.entities.FromSqlRaw(sql, parameters);
        }
    }
}
