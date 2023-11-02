using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models.DataModels
{
    public class ValidationResultModel
    {
        public ValidationResultModel(bool isSuccess, List<ValidationResult>? dataAnnotationErrors = null)
        {
            IsSuccess = isSuccess;
            DataAnnotationErrors = dataAnnotationErrors;
        }
        public bool IsSuccess { get; set; }
        public List<ValidationResult>? DataAnnotationErrors { get; set; }
        public string ErrorsToString()
        {
            if(DataAnnotationErrors is null || DataAnnotationErrors.Count == 0)
            {
                throw new ArgumentException();
            }
            StringBuilder sb = new StringBuilder();
            foreach (var e in DataAnnotationErrors)
            {
                sb.Append(e.MemberNames);
                sb.Append("-");
                sb.Append(e.ErrorMessage);
                sb.Append("--");
            }
            return sb.ToString();
        }
    }
}