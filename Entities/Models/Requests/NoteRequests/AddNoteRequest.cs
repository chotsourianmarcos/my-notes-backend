using Entities.Items;

namespace Entities.Models.Requests.NoteRequests
{
    public class AddNoteRequest
    {
        public AddNoteRequest()
        {
            TagsNames = new List<string>();
        }
        public string Content { get; set; }
        public List<string> TagsNames { get; set; }

        public NoteItem ToNoteItem(int userId, List<TagItem> tags)
        {
            var noteItem = new NoteItem();

            noteItem.IdWeb = Guid.NewGuid();
            noteItem.UserId = userId;
            noteItem.InsertedDate = DateTime.Now;
            //var tags = new List<TagItem>();
            //foreach (var n in TagsNames)
            //{
            //    var tag = new TagItem();
            //    tag.Name = n;
            //    tags.Add(tag);
            //}
            noteItem.Tags = tags;
            noteItem.Content = Content;
            noteItem.IsArchived = true;
            noteItem.IsActive = true;

            return noteItem;
        }
    }
}
