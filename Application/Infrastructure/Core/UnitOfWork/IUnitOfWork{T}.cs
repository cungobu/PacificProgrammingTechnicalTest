using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    public interface IUnitOfWork<TDbContext> : Domain.IUnitOfWork, IDisposable
        where TDbContext : DbContext, new()
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        void SaveChanges();
        void Cancel();
    }
}
