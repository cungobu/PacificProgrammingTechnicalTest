using Domain.Repositories;
using Infrastructure.Contexts;
using Infrastructure.Core;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class RepositoryProxy<TEntity> : Domain.Repositories.IRepository<TEntity> where TEntity : class
    {
        private readonly Domain.Repositories.IRepository<TEntity> repository;

        public RepositoryProxy(IServiceProvider serviceProvider)
        {
            Type repositoryConcreteType = GetRepositoryType();
            this.repository = serviceProvider.GetService(repositoryConcreteType) as IRepository<TEntity>;
        }

        private static Type GetRepositoryType()
        {
            Type entityType = typeof(TEntity);

            Type repositoryType = typeof(Repository<,>);

            var repositoryConcreteType = repositoryType.MakeGenericType(typeof(TEntity), typeof(ApplicationDbContext));
            return repositoryConcreteType;
        }

        public void Add(TEntity entity)
        {
            ((IRepository<TEntity>)repository).Add(entity);
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return ((IReadRepository<TEntity>)repository).Any(expression);
        }

        public int Count(Expression<Func<TEntity, bool>> expression)
        {
            return ((IReadRepository<TEntity>)repository).Count(expression);
        }

        public void Delete(TEntity entity)
        {
            ((IRepository<TEntity>)repository).Delete(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            ((IRepository<TEntity>)repository).Delete(expression);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            ((IRepository<TEntity>)repository).DeleteRange(entities);
        }

        public void ExecuteSQL(string sql, params object[] paramertes)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> expression)
        {
            return ((IReadRepository<TEntity>)repository).Filter(expression);
        }

        public TEntity Find(object entityId)
        {
            return ((IReadRepository<TEntity>)repository).Find(entityId);
        }

        public IQueryable<TEntity> FromSqlRaw(string sql, params object[] parameters)
        {
            return ((IRepository<TEntity>)repository).FromSqlRaw(sql, parameters);
        }

        public IQueryable<TEntity> GetAll()
        {
            return ((IReadRepository<TEntity>)repository).GetAll();
        }

        public IQueryable<TEntity> Queryable()
        {
            return ((IReadRepository<TEntity>)repository).Queryable();
        }

        public void Update(TEntity entity)
        {
            ((IRepository<TEntity>)repository).Update(entity);
        }
    }
}
