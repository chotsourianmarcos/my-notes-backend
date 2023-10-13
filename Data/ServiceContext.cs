using Microsoft.EntityFrameworkCore;
using Entities.Entities;
using Entities.Items;
using System.Reflection;

namespace Data
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options) { }
        public DbSet<UserRolItem> UserRols { get; set; }
        public DbSet<UserItem> Users { get; set; }
        public DbSet<TagItem> Tags { get; set; }
        public DbSet<NoteItem> Notes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}