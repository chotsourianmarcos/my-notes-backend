using API.Attributes;
using API.MyNotes.IServices;
using Entities.Items;
using Entities.Models.Requests.NoteRequests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [EndpointAuthorize(AllowedUserRols = "Usuario")]
        [HttpGet(Name = "GetAll")]
        public async Task<ActionResult<List<NoteItem>>> GetAllNotes([FromQuery] Guid UserIdWeb)
        {
            var result = await _noteService.GetAllNotes();
            return Ok(result);
        }
        [EndpointAuthorize(AllowedUserRols = "Usuario")]
        [HttpGet(Name = "GetByCriteria")]
        public async Task<ActionResult<List<NoteItem>>> GetNotesByCriteria([FromQuery] GetNotesByCriteriaRequest requestData)
        {
            var result = await _noteService.GetNotesByCriteria(requestData.ToNoteFilter());
            return Ok(result);
        }
        [EndpointAuthorize(AllowedUserRols = "Usuario")]
        [HttpPost(Name = "AddNote")]
        public async Task<ActionResult<Guid>> AddNote([FromBody] AddNoteRequest newNoteRequest)
        {
            var idWeb = await _noteService.AddNote(newNoteRequest);
            return Ok(idWeb);
        }
        [EndpointAuthorize(AllowedUserRols = "Usuario")]
        [HttpPatch(Name = "UpdateNote")]
        public async Task<ActionResult> UpdateNote([FromBody] UpdateNoteRequest updateNoteRequest)
        {
            await _noteService.UpdateNote(updateNoteRequest);
            return Ok();
        }
        [EndpointAuthorize(AllowedUserRols = "Usuario")]
        [HttpDelete(Name = "DeleteNote")]
        public async Task<ActionResult> DeleteNote([FromQuery] Guid idWeb)
        {
            await _noteService.DeleteNote(idWeb);
            return Ok();
        }
    }
}