using Entities.Items;
using Entities.Models.Filters;

namespace Entities.Models.Requests.NoteRequests
{
    public class GetNotesByCriteriaRequest
    {
        public Guid UserIdWeb { get; set; }
        public string TagsIncluded { get; set; }
        public bool IsArchived { get; set; }
        public NoteFilter ToNoteFilter()
        {
            var noteFilter = new NoteFilter();

            if (TagsIncluded == TagConstants.IncludeAllTagsRef)
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