using Entities.Items;
using Entities.Models.Filters;

namespace Logic.ILogic
{
    public interface INoteLogic
    {
        Task<Guid> AddNote(NoteItem noteItem);
        Task DeleteNote(Guid idWeb);
        Task<List<NoteItem>> GetAllNotes();
        Task<List<NoteItem>> GetNotesByCriteria(NoteFilter noteFilter);
        Task UpdateNote(NoteItem noteItem);
    }
}