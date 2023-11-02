namespace Logic.Exceptions
{
    public enum InvalidModelExceptionType
    {
        InvalidDataAnnotations = 1
    }
    public class InvalidModelExceptionResp : HttpResponseMessage
    {
        public InvalidModelExceptionResp(InvalidModelExceptionType type, string customMessage = "", string errorCode = "")
        {
            switch ((int)type)
            {
                case (int)InvalidModelExceptionType.InvalidDataAnnotations:
                    this.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    this.Content = new StringContent("Unexpected invalid data model.");
                    this.ReasonPhrase = "Invalid data model.";
                    break;
                case '_':
                    break;
            }

            if (!String.IsNullOrEmpty(customMessage))
            {
                this.Content = new StringContent(customMessage);
            }
            if (!String.IsNullOrEmpty(errorCode))
            {
                this.ReasonPhrase = errorCode;
            }
        }
    }
    public class InvalidModelException : ControlledException
    {
        public InvalidModelException(InvalidModelExceptionType type, string customMessage = "", string errorCode = "") : base(new InvalidModelExceptionResp(type, customMessage, errorCode))
        {

        }
    }
}
