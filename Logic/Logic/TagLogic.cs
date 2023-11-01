using Data;
using Entities.Items;
using Logic.ILogic;
using Microsoft.EntityFrameworkCore;

namespace Logic.Logic
{
    public class TagLogic : ITagLogic
    {
        private readonly ServiceContext _serviceContext;
        private readonly IUserSessionAccessLogic _userSessionLogic;
        public TagLogic(ServiceContext serviceContext, IUserSessionAccessLogic userSessionLogic) 
        {
            _serviceContext = serviceContext;
            _userSessionLogic = userSessionLogic;
        }

        public async Task<List<TagItem>> SetCurrentTags(List<TagItem> tagList)
        {
            var allTags = await _serviceContext.Tags.ToListAsync();
            var existingTagsList = allTags.Where(c => tagList.Select(t => t.Name).Contains(c.Name)).ToList();

            foreach(var t in tagList)
            {
                if(!existingTagsList.Select(e => e.Name).Contains(t.Name))
                {
                    existingTagsList.Add(t);
                }
            }

            return existingTagsList;
        }

        public async Task<List<TagItem>> AssignTagsByNames(List<string> tagNames)
        {
            var currentTags = await _serviceContext.Tags.Where(t => tagNames.Contains(t.Name)).ToListAsync();

            foreach(var n in tagNames)
            {
                if(currentTags.Any(t => t.Name == n))
                {
                    continue;
                }
                else
                {
                    var newTag = new TagItem();
                    newTag.IdWeb = Guid.NewGuid();
                    newTag.Name = n;
                    currentTags.Add(newTag);
                }
            }

            return currentTags;
        }

        public async Task<List<string>> GetAllTags()
        {
            var userId = await _userSessionLogic.GetCurrentUserId();
            var allNotes = await _serviceContext.Notes
                .Where(n => n.UserId == userId && n.IsActive)
                .Include(n => n.Tags)
                .ToListAsync();
            var userTags = new List<TagItem>();

            foreach(var n in allNotes)
            {
                foreach(var t in n.Tags)
                {
                    if (!userTags.Contains(t))
                    {
                        userTags.Add(t);
                    }
                }
            }

            return userTags.Select(t => t.Name).ToList();
        }
    }
}