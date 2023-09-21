using API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        public TestController()
        {
        }

        [EndpointAuthorize(AllowsAnonymous = true)]
        [HttpGet(Name = "GetTestString")]
        public string GetTestString()
        {
            return "Test String";
        }
    }
}