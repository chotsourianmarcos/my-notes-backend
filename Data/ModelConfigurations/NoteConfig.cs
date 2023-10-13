using Entities.Entities;
using Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.ModelConfigurations
{
    public class NoteConfig : IEntityTypeConfiguration<NoteItem>
    {
        public void Configure(EntityTypeBuilder<NoteItem> e)
        {
            e.ToTable("t_notes");
            e.HasOne<UserItem>(e => e.User).WithMany(u => u.Notes).HasForeignKey(n => n.UserId).HasConstraintName("FK_t_notes_t_users_UserId");
            e.HasMany<TagItem>(e => e.Tags).WithMany().UsingEntity(join => join.ToTable("notes_tags"));
        }
    }
}