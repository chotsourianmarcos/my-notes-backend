using API.Attributes;
using API.MyNotes.IServices;
using Entities.Items;
using Entities.Models.Requests;
using Logic.Exceptions;
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
            try
            {
                var result = await _noteService.GetAllNotes();
                return Ok(result);
            }
            catch (ControlledException ex)
            {
                return ex.ToObjectResult();
            }
        }
        [EndpointAuthorize(AllowedUserRols = "Usuario")]
        [HttpGet(Name = "GetByCriteria")]
        public async Task<ActionResult<List<NoteItem>>> GetNotesByCriteria([FromQuery] GetNotesByCriteriaRequest requestData)
        {
            try
            {
                var result = await _noteService.GetNotesByCriteria(requestData.ToNoteFilter());
                return Ok(result);
            }
            catch (ControlledException ex)
            {
                return ex.ToObjectResult();
            }
        }
        [EndpointAuthorize(AllowedUserRols = "Usuario")]
        [HttpPost(Name = "AddNote")]
        public async Task<ActionResult<Guid>> AddNote([FromBody] AddUpdateNoteRequest newNoteRequest)
        {
            try
            {
                var idWeb = await _noteService.AddNote(newNoteRequest);
                return Ok(idWeb);
            }
            catch (ControlledException ex)
            {
                return ex.ToObjectResult();
            }
        }
        [EndpointAuthorize(AllowedUserRols = "Usuario")]
        [HttpPatch(Name = "UpdateNote")]
        public async Task<ActionResult> UpdateNote([FromBody] AddUpdateNoteRequest updateNoteRequest)
        {
            try
            {
                await _noteService.UpdateNote(updateNoteRequest);
                return Ok();
            }
            catch (ControlledException ex)
            {
                return ex.ToObjectResult();
            }
        }
        [EndpointAuthorize(AllowedUserRols = "Usuario")]
        [HttpDelete(Name = "DeleteNote")]
        public async Task<ActionResult> DeleteNote([FromQuery] Guid idWeb)
        {
            try
            {
                await _noteService.DeleteNote(idWeb);
                return Ok();
            }
            catch (ControlledException ex)
            {
                return ex.ToObjectResult();
            }
        }
    }
}