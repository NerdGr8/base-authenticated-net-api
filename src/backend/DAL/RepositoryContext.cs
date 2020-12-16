using JK.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace JK.DAL
{
    public class RepositoryContext:DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRecord> UserRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        /*public IConfiguration Configuration { get; }
        public ApplicationContextDbFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }*/

        RepositoryContext IDesignTimeDbContextFactory<RepositoryContext>.CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseNpgsql("User ID=JK;Password=JK101;Server=JK.ci9q1fyeq0lm.us-east-2.rds.amazonaws.com;Port=5432;Database=JK;Integrated Security=true;Pooling=true;");

            return new RepositoryContext(builder.Options);
        }
    }
}
