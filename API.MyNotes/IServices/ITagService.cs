namespace API.MyNotes.IServices
{
    public interface ITagService
    {
        Task<List<string>> GetAllTags();
    }
}