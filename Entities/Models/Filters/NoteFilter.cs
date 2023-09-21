using Entities.Items;
using System.Linq.Expressions;

namespace Entities.Models.Filters
{
    public class NoteFilter
    {
        public NoteFilter()
        {
            TagsIncluded = new List<string>();
        }
        public List<string> TagsIncluded { get; set; }
        public bool IsArchived { get; set; }
        public Expression<Func<NoteItem, bool>> ToFunction()
        {
            return (n =>
                n.IsActive &&
                (TagsIncluded.Count == 0 || n.Tags.Any(t => TagsIncluded.Contains(t.Name))) &&
                n.IsArchived == IsArchived);
        }
    }
}