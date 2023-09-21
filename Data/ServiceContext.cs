using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Entities.Entities;
using Entities.Items;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

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
            builder.Entity<UserRolItem>(e =>
            {
                e.ToTable("t_user_rols");
                e.Property(e => e.Id).ValueGeneratedNever();
            });

            builder.Entity<UserItem>(e =>
            {
                e.ToTable("t_users");
                e.HasOne<UserRolItem>().WithMany().HasForeignKey(u => u.RolId);
            });

            builder.Entity<TagItem>(e =>
            {
                e.ToTable("t_tags");
            });

            builder.Entity<NoteItem>(e =>
            {
                e.ToTable("t_notes");
                e.HasOne<UserItem>(e => e.User).WithMany(u => u.Notes).HasForeignKey(n => n.UserId).HasConstraintName("FK_t_notes_t_users_UserId");
                e.HasMany<TagItem>(e => e.Tags).WithMany().UsingEntity(join => join.ToTable("notes_tags"));
            });

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }
    }
    public class ServiceContextFactory : IDesignTimeDbContextFactory<ServiceContext>
    {
        public ServiceContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", false, true);
            var config = builder.Build();
            var connectionString = config.GetConnectionString("ServiceContext");
            var optionsBuilder = new DbContextOptionsBuilder<ServiceContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("ServiceContext"));

            return new ServiceContext(optionsBuilder.Options);
        }
    }
}