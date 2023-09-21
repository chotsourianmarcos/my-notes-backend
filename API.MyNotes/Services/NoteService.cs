using API.MyNotes.IServices;
using Entities.Items;
using Entities.Models.Filters;
using Entities.Models.Requests;
using Logic.ILogic;
using Logic.Logic;

namespace API.MyNotes.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteLogic _noteLogic;
        private readonly ITagLogic _tagLogic;
        public NoteService(INoteLogic noteLogic, ITagLogic tagLogic)
        {
            _noteLogic = noteLogic;
            _tagLogic = tagLogic;
        }
        public async Task<Guid> AddNote(AddUpdateNoteRequest newNoteRequest)
        {
            var newNoteItem = newNoteRequest.ToNoteItem(true);
            newNoteItem.UserId = UserSessionLogic.GetCurrentUserId();
            newNoteItem.Tags = await _tagLogic.AssignTagsByNames(newNoteRequest.TagsNames);
            return await _noteLogic.AddNote(newNoteItem);
        }
        public async Task DeleteNote(Guid idWeb)
        {
            await _noteLogic.DeleteNote(idWeb);
        }
        public async Task<List<NoteItem>> GetAllNotes()
        {
            return await _noteLogic.GetAllNotes();
        }
        public async Task<List<NoteItem>> GetNotesByCriteria(NoteFilter noteFilter)
        {
            return await _noteLogic.GetNotesByCriteria(noteFilter);
        }
        public async Task UpdateNote(AddUpdateNoteRequest updateNoteRequest)
        {
            var noteItem = updateNoteRequest.ToNoteItem(false);
            noteItem.UserId = UserSessionLogic.GetCurrentUserId();
            await _noteLogic.UpdateNote(noteItem);
        }
    }
}