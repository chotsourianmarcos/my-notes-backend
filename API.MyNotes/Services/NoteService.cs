using API.MyNotes.IServices;
using Entities.Items;
using Entities.Models.Filters;
using Entities.Models.Requests.NoteRequests;
using Logic.ILogic;

namespace API.MyNotes.Services
{
    public class NoteService : INoteService
    {
        private readonly IUserSessionAccessLogic _userSessionAccessLogic;
        private readonly INoteLogic _noteLogic;
        private readonly ITagLogic _tagLogic;
        public NoteService(IUserSessionAccessLogic userAccessSessionLogic, INoteLogic noteLogic, ITagLogic tagLogic)
        {
            _userSessionAccessLogic = userAccessSessionLogic;
            _noteLogic = noteLogic;
            _tagLogic = tagLogic;
        }
        public async Task<Guid> AddNote(AddNoteRequest newNoteRequest)
        {
            var userId = await _userSessionAccessLogic.GetCurrentUserId();
            var tagList = await _tagLogic.AssignTagsByNames(newNoteRequest.TagsNames);
            var newNoteItem = newNoteRequest.ToNoteItem(userId, tagList);
            
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
        public async Task UpdateNote(UpdateNoteRequest updateNoteRequest)
        {
            var userId =  await _userSessionAccessLogic.GetCurrentUserId();
            var noteItem = updateNoteRequest.ToNoteItem(userId);
            await _noteLogic.UpdateNote(noteItem);
        }
    }
}