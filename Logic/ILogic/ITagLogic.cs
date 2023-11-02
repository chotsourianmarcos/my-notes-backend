using Entities.Items;

namespace Logic.ILogic
{
    public interface ITagLogic
    {
        Task<List<TagItem>> AssignTagsByNames(List<string> tagNames);
        Task<List<string>> GetAllTags();
        Task<List<TagItem>> SetCurrentTags(List<TagItem> tagList);
    }
}