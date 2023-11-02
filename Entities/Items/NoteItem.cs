using Entities.Entities;
using Resources.Strings.ErrorMessages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities.Items
{
    public class NoteItem : GenericItem<NoteItem>
    {
        public NoteItem() {
            this.IdWeb = Guid.NewGuid();
            this.Tags = new List<TagItem>();
        }
        [Range(0, int.MaxValue, ErrorMessage = ModelErrorMsg.ValueNotNegative)]
        public int Id { get; set; }
        public Guid IdWeb { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = ModelErrorMsg.ValueNotNegative)]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual UserItem? User { get; set; }
        public virtual ICollection<TagItem> Tags { get; set; }
        [StringLength(2000, MinimumLength = 1)]
        public string Content { get; set; } = "";
        public DateTime InsertedDate { get; set; }
        public bool IsArchived { get; set; }
        public bool IsActive { get; set; }
        public bool ValidateModel(bool toBeInserted)
        {
            var validDataAnnotations = ValidateDataAnnotations(this);
            if (!validDataAnnotations.IsSuccess)
            {
                throw new Exception(validDataAnnotations.ErrorsToString());
            }
            else
            {
                return true;
            }
        }
    }
}