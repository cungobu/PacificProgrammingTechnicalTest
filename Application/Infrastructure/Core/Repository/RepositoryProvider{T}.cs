using Domain.Repositories;
using Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    internal class RepositoryProvider<TDbContext> where TDbContext : DbContext, new()
    {
        private readonly IDbFactory<TDbContext> dbFactory;
        private readonly IUnitOfWork<TDbContext> unitOfWork;
        private readonly ConcurrentDictionary<Type,dynamic> Repositories = new ConcurrentDictionary<Type,dynamic>();
        
        public RepositoryProvider(IDbFactory<TDbContext> dbFactory, IUnitOfWork<TDbContext> unitOfWork)
        {
            this.dbFactory = dbFactory;
            this.unitOfWork = unitOfWork;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return Repositories.GetOrAdd(typeof(TEntity), _ => InitRepository<TEntity>());
        }

        private IRepository<T> InitRepository<T>() where T : class
        {
            return new Repository<T, TDbContext>(this.dbFactory, this.unitOfWork);
        }
    }
}
