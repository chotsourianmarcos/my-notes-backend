using Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.ModelConfigurations
{
    public class TagConfig : IEntityTypeConfiguration<TagItem>
    {
        public void Configure(EntityTypeBuilder<TagItem> e)
        {
            e.ToTable("t_tags");
        }
    }
}
