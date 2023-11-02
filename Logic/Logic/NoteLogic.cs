using Data;
using Entities.Items;
using Entities.Models.Filters;
using Logic.Exceptions;
using Logic.ILogic;
using Microsoft.EntityFrameworkCore;

namespace Logic.Logic
{
    public class NoteLogic : INoteLogic
    {
        private readonly ServiceContext _serviceContext;
        private readonly IUserSessionAccessLogic _userSessionAccessLogic;
        private readonly ITagLogic _tagLogic;
        public NoteLogic(ServiceContext serviceContext,
            IUserSessionAccessLogic userSessionAccessLogic,
            ITagLogic tagLogic)
        {
            _serviceContext = serviceContext;
            _userSessionAccessLogic = userSessionAccessLogic;
            _tagLogic = tagLogic;
        }
        public async Task<Guid> AddNote(NoteItem noteItem)
        {
            var currentTags = await _tagLogic.SetCurrentTags(noteItem.Tags.ToList());
            noteItem.Tags = currentTags;

            noteItem.ValidateModel(true);

            await _serviceContext.Notes.AddAsync(noteItem);
            await _serviceContext.SaveChangesAsync();

            return noteItem.IdWeb;
        }

        public async Task DeleteNote(Guid idWeb)
        {
            var noteItem = await _serviceContext.Notes
                .Include(n => n.User)
                .Where(n => n.IdWeb == idWeb)
                .FirstAsync();

            if(noteItem.User.Id != await _userSessionAccessLogic.GetCurrentUserId())
            {
                throw new BadRequestException(BadRequestExceptionType.InvalidOperation);
            }

            noteItem.IsActive = false;
            await _serviceContext.SaveChangesAsync();
        }

        public async Task<List<NoteItem>> GetAllNotes()
        {
            return await _serviceContext.Notes.Where(n => n.User.IdWeb == _userSessionAccessLogic.GetCurrentUserIdWeb() && n.IsActive).ToListAsync();
        }

        public async Task<List<NoteItem>> GetNotesByCriteria(NoteFilter noteFilter)
        {
            return await _serviceContext.Notes
                .Include(n => n.Tags)
                .Where(n => n.IsActive && n.User.IdWeb == _userSessionAccessLogic.GetCurrentUserIdWeb())
                .Where(noteFilter.ToFunction()).ToListAsync();
        }

        public async Task UpdateNote(NoteItem updatedNoteItem)
        {
            updatedNoteItem.ValidateModel(false);

            var noteItem = await _serviceContext.Notes
                .Include(n => n.User)
                .Include(n => n.Tags)
                .Where(n => n.IdWeb == updatedNoteItem.IdWeb)
                .FirstAsync();

            if (noteItem.User.Id != await _userSessionAccessLogic.GetCurrentUserId())
            {
                throw new BadRequestException(BadRequestExceptionType.InvalidOperation);
            }

            noteItem.Tags = await _tagLogic.SetCurrentTags(updatedNoteItem.Tags.ToList());
            noteItem.Content = updatedNoteItem.Content;
            noteItem.IsArchived = updatedNoteItem.IsArchived;

            _serviceContext.Set<NoteItem>().Update(noteItem);
            await _serviceContext.SaveChangesAsync();
        }
    }
}