using Domain.Repositories;
using Infrastructure.Contexts;
using Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    internal class ReadRepositoryProxy<TEntity> : IReadRepository<TEntity> where TEntity : class
    {
        private IReadRepository<TEntity> repository;

        public ReadRepositoryProxy(IServiceProvider serviceProvider)
        {
            Type repositoryConcreteType = GetRepositoryType();
            this.repository = serviceProvider.GetService(repositoryConcreteType) as IReadRepository<TEntity>;
        }

        private static Type GetRepositoryType()
        {
            Type entityType = typeof(TEntity);
            Type repositoryType = typeof(ReadRepository<,>);

            var repositoryConcreteType = repositoryType.MakeGenericType(typeof(TEntity), typeof(ApplicationDbContext));
            return repositoryConcreteType;
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return repository.Any(expression);
        }

        public int Count(Expression<Func<TEntity, bool>> expression)
        {
            return repository.Count(expression);
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> expression)
        {
            return repository.Filter(expression).AsNoTracking();
        }

        public TEntity Find(object entityId)
        {
            var context = (repository as IDbContextContainer)?.Context;

            if (context == null)
            {
                return repository.Find(entityId);
            }
            else
            {
                var trackingBehavior = context.ChangeTracker.QueryTrackingBehavior;

                try
                {
                    context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                    var entity = repository.Find(entityId);

                    return entity;
                }
                finally
                {
                    context.ChangeTracker.QueryTrackingBehavior = trackingBehavior;
                }
            }
        }

        public IQueryable<TEntity> FromSqlRaw(string sql, params object[] parameters)
        {
            return repository.FromSqlRaw(sql, parameters).AsNoTracking();
        }

        public IQueryable<TEntity> GetAll()
        {
            return repository.GetAll().AsNoTracking();
        }

        public IQueryable<TEntity> Queryable()
        {
            return repository.Queryable().AsNoTracking();
        }
    }
}
