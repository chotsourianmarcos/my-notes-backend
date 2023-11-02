using Entities.Items;

namespace Entities.Models.Requests.NoteRequests
{
    public class UpdateNoteRequest
    {
        public UpdateNoteRequest()
        {
            TagsNames = new List<string>();
        }
        public string NoteIdWeb { get; set; } = "";
        public string Content { get; set; } = "";
        public List<string> TagsNames { get; set; }
        public bool IsArchived { get; set; }

        public NoteItem ToNoteItem(int userId)
        {
            var noteItem = new NoteItem();

            noteItem.UserId = userId;
            var tags = new List<TagItem>();
            foreach (var n in TagsNames)
            {
                var tag = new TagItem();
                tag.Name = n;
                tags.Add(tag);
            }
            noteItem.Tags = tags;
            noteItem.Content = Content;
            noteItem.IsArchived = IsArchived;
            noteItem.IsActive = true;

            return noteItem;
        }
    }
}