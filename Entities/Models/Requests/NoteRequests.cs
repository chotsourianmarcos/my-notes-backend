using Entities.Items;
using Entities.Models.Filters;

namespace Entities.Models.Requests
{
    public class AddUpdateNoteRequest
    {
        public AddUpdateNoteRequest()
        {
            TagsNames = new List<string>();
        }
        public string NoteIdWeb { get; set; }
        public string UserIdWeb { get; set; }
        public string Content { get; set; }
        public List<string> TagsNames { get; set; }
        public DateTime InsertedDate { get; set; }
        public bool IsArchived { get; set; }

        public NoteItem ToNoteItem(bool isNew)
        {
            var noteItem = new NoteItem();

            if (isNew)
            {
                noteItem.IdWeb = Guid.NewGuid();
                noteItem.InsertedDate = DateTime.Now;
            }
            else
            {
                noteItem.IdWeb = Guid.Parse(NoteIdWeb);
            }
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
    public class GetNotesByCriteriaRequest
    {
        public Guid UserIdWeb { get; set; }
        public string TagsIncluded { get; set;}
        public bool IsArchived { get; set; }
        public NoteFilter ToNoteFilter()
        {
            var noteFilter = new NoteFilter();

            if (TagsIncluded == "all")
            {
                noteFilter.TagsIncluded = new List<string>();
            }
            else
            {
                noteFilter.TagsIncluded = TagsIncluded.Split(',').ToList();
            }
            noteFilter.IsArchived = IsArchived;

            return noteFilter;
        }
    }
}