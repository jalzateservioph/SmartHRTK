using BiometricsIntegrationWebAPI.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BiometricsIntegrationWebAPI
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<RawData> RawData { get; set; }
    }
}
