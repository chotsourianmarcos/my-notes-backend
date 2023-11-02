namespace Logic.Exceptions
{
    public enum BadRequestExceptionType
    {
        InvalidData = 1,
        InvalidOperation = 2,
        RepeatedOperation = 3
    }
    public class BadRequestResp : HttpResponseMessage
    {
        public BadRequestResp(BadRequestExceptionType type, string customMessage = "", string errorCode = "")
        {
            switch ((int)type)
            {
                case (int)BadRequestExceptionType.InvalidData:
                    this.StatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
                    this.Content = new StringContent("Submitted data is not valid.");
                    this.ReasonPhrase = "Invalid data.";
                    break;
                case (int)BadRequestExceptionType.InvalidOperation:
                    this.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    this.Content = new StringContent("Pretended operation is not valid.");
                    this.ReasonPhrase = "Invalid operation.";
                    break;
                case (int)BadRequestExceptionType.RepeatedOperation:
                    this.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    this.Content = new StringContent("The operation has already been processed.");
                    this.ReasonPhrase = "Repeated operation.";
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
    public class BadRequestException : ControlledException
    {
        public BadRequestException(BadRequestExceptionType type, string customMessage = "", string errorCode = "") : base(new BadRequestResp(type, customMessage, errorCode))
        {

        }
    }
}