using Entities.Entities;
using System.Text.Json.Serialization;

namespace Entities.Items
{
    public class NoteItem
    {
        public NoteItem() {
            this.Tags = new List<TagItem>();
        }
        public int Id { get; set; }
        public Guid IdWeb { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual UserItem User { get; set; }
        public virtual ICollection<TagItem> Tags { get; set; }
        public string Content { get; set; }
        public bool IsArchived { get; set; }
        public DateTime InsertedDate { get; set; }
        public bool IsActive { get; set; }
        public bool ValidateValues(bool isNew, bool isActive)
        {
            return true;
        }
    }
}