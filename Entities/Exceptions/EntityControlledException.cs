using Microsoft.AspNetCore.Mvc;

namespace Entities.Exceptions
{
    public class EntityControlledException : Exception
    {
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public EntityControlledException(HttpResponseMessage resp)
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
            objectResult.StatusCode = (int)HttpResponseMessage.StatusCode;

            return objectResult;
        }
    }
}