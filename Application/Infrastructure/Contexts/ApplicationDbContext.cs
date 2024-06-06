using Domain.Entities;
using Infrastructure.Core.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contexts
{
    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public ApplicationDbContext() : base()
        {

        }
    }
}
