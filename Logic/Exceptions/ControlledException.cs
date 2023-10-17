using Microsoft.AspNetCore.Mvc;

namespace Logic.Exceptions
{
    public class ControlledException : Exception
    {
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public ControlledException(HttpResponseMessage resp)
        {
            HttpResponseMessage = resp;
        }
        public async Task<ObjectResult> ToObjectResult()
        {
            var data = new
            {
                ReasonPhrase = HttpResponseMessage.ReasonPhrase,
                Message = await HttpResponseMessage.Content.ReadAsStringAsync()
            };

            ObjectResult objectResult = new ObjectResult(data);
            objectResult.StatusCode = (int) HttpResponseMessage.StatusCode;

            return objectResult;
        }
    }
}