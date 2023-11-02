using API.Attributes;
using API.MyNotes.Controllers;
using API.MyNotes.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TagController : MyNotesController
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService) : base()
        {
            _tagService = tagService;
        }

        [EndpointAuthorize(AllowedUserRols = UserRolName)]
        [HttpGet(Name = "GetAllTags")]
        public async Task<ActionResult<List<string>>> GetAllTags()
        {
            var result = await _tagService.GetAllTags();
            return Ok(result);
        }
    }
}