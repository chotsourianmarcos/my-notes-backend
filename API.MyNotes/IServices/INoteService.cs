using Entities.Items;
using Entities.Models.Filters;
using Entities.Models.Requests.NoteRequests;

namespace API.MyNotes.IServices
{
    public interface INoteService
    {
        Task<Guid> AddNote(AddNoteRequest newNoteRequest);
        Task DeleteNote(Guid idWeb);
        Task<List<NoteItem>> GetAllNotes();
        Task<List<NoteItem>> GetNotesByCriteria(NoteFilter noteFilter);
        Task UpdateNote(UpdateNoteRequest updateNoteRequest);
    }
}