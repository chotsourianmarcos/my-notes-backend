using Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.ModelConfigurations
{
    public class UserRolConfig : IEntityTypeConfiguration<UserRolItem>
    {
        public void Configure(EntityTypeBuilder<UserRolItem> e)
        {
            e.ToTable("t_user_rols");
            e.Property(e => e.Id).ValueGeneratedNever();
        }
    }
}