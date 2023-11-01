namespace Entities.Items
{
    public class TagItem
    {
        public int Id { get; set; }
        public Guid IdWeb { get; set; }
        public string Name { get; set; }
        public bool ValidateValues()
        {
            return true;
        }
    }
    public static class TagConstants
    {
        public static readonly string IncludeAllTagsRef = "all";
    }
}