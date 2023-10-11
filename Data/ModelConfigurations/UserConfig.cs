using Entities.Entities;
using Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.ModelConfigurations
{
    public class UserConfig : IEntityTypeConfiguration<UserItem>
    {
        public void Configure(EntityTypeBuilder<UserItem> e)
        {
            e.ToTable("t_users");
            e.HasOne<UserRolItem>().WithMany().HasForeignKey(u => u.RolId);
        }
    }
}
