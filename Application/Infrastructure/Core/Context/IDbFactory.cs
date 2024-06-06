using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    public interface IDbFactory<TDbContext> : IDisposable where TDbContext : DbContext, new()
    {
        TDbContext Init();
    }
}
