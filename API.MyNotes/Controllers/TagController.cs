using API.Attributes;
using API.MyNotes.IServices;
using Logic.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [EndpointAuthorize(AllowedUserRols = "Usuario")]
        [HttpGet(Name = "GetAllTags")]
        public async Task<ActionResult<List<string>>> GetAllTags()
        {
            try
            {
                var result = await _tagService.GetAllTags();
                return Ok(result);
            }
            catch (ControlledException ex)
            {
                return ex.ToObjectResult();
            }
        }
    }
}