using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    public class DbFactory<TDbContext> : IDbFactory<TDbContext> where TDbContext : DbContext, new()
    {
        private readonly IServiceProvider serviceProvider;
        TDbContext dbContext;
        
        public DbFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        public TDbContext Init()
        {
            return dbContext ?? (dbContext = serviceProvider.GetService<TDbContext>());
        }
    }
}
