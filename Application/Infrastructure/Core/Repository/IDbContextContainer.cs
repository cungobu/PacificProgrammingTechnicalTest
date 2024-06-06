using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core
{
    internal interface IDbContextContainer
    {
        DbContext Context { get; }
    }
}
