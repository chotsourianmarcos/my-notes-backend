using Logic.Exceptions;
using Resources.Strings.ErrorMessages;
using System.ComponentModel.DataAnnotations;

namespace Entities.Items
{
    public class TagItem : GenericItem<TagItem>
    {
        [Range(0, int.MaxValue, ErrorMessage = ModelErrorMsg.ValueNotNegative)]
        public int Id { get; set; }
        public Guid IdWeb { get; set; }
        [StringLength(20, MinimumLength = 1)]
        public string Name { get; set; } = "";
        public bool ValidateModel(bool toBeInserted)
        {
            var validDataAnnotations = ValidateDataAnnotations(this);
            if (!validDataAnnotations.IsSuccess)
            {
                throw new InvalidModelException(InvalidModelExceptionType.InvalidDataModel, validDataAnnotations.ErrorsToString());
            }
            else
            {
                return true;
            }
        }
    }
    public static class TagConstants
    {
        public static readonly string IncludeAllTagsRef = "all";
    }
}