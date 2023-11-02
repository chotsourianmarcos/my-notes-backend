using Entities.Models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Entities.Items
{
    public abstract class GenericItem<T>
        where T : class
    {
        public ValidationResultModel ValidateDataAnnotations(T obj)
        {
            var validationErrorList = new List<ValidationResult>();
            var isSuccess = Validator.TryValidateObject(obj, new ValidationContext(obj), validationErrorList, true);
            if(!isSuccess)
            {
                return new ValidationResultModel(false, validationErrorList);
            }
            else
            {
                return new ValidationResultModel(true);
            }
        }
    }
}
