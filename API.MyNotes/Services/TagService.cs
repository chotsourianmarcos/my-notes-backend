using API.MyNotes.IServices;
using Logic.ILogic;

namespace API.MyNotes.Services
{
    public class TagService : ITagService
    {
        private readonly ITagLogic _tagLogic;
        public TagService(ITagLogic tagLogic)
        {
            _tagLogic = tagLogic;
        }
        public async Task<List<string>> GetAllTags()
        {
            return await _tagLogic.GetAllTags();
        }
    }
}