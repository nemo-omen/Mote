using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mote.Api.Data;
using Mote.Api.Features.Notes.Types;
using Mote.Api.Models;

namespace Mote.Api.Features.Notes;

[ApiController]
[Route("api/notes")]
public class NotesController : Controller
{
    private readonly INotesService _notesService;
    private readonly IUserResolverService _userResolverService;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public NotesController(INotesService notesService, IUserResolverService userResolverService, UserManager<ApplicationUser> userManager)
    {
        _notesService = notesService;
        _userResolverService = userResolverService;
        _userManager = userManager;
    }
    
    // GET
    [HttpGet]
    [ProducesResponseType<List<Note>>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAllNotes()
    {
        // var userIdResponse = _userResolverService.GetUserId();
        // if (userIdResponse is null)
        // {
        
        //     return BadRequest("No user found");
        // }
        
        // var notesResult = await _notesService.GetNotesByUserAsync(new Guid(userIdResponse));
        var notesResult = await _notesService.GetAllNotesAsync();
        if (notesResult.IsFailed)
        {
            return BadRequest(notesResult.Errors.First());
        }
        
        return Ok(notesResult.Value);
    }
    
    [HttpGet("{noteId}")]
    [ProducesResponseType<Note>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetNoteById([FromRoute] Guid noteId)
    {
        var noteResult = await _notesService.GetNoteByIdAsync(noteId);
        if (noteResult.IsFailed)
        {
            return NotFound();
        }
        
        return Ok(noteResult.Value);
    }
    
    [HttpGet("path")]
    [ProducesResponseType<Note>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetNoteByPath(string path)
    {
        var noteResult = await _notesService.GetNoteByPathAsync(path);
        if (noteResult.IsFailed)
        {
            return NotFound();
        }
        
        return Ok(noteResult.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest noteDto)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        // var userIdResponse = _userResolverService.GetUserId();
        // if (userIdResponse is null)
        // {
        //     return BadRequest("No user found");
        // }
        
        // var userId = new Guid(userIdResponse);
        var userId = Guid.Empty;
        var noteResult = await _notesService.SaveNoteAsync(noteDto);
        if (noteResult.IsFailed)
        {
            return BadRequest(noteResult.Errors.First());
        }
        
        return Ok(noteResult.Value);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateNote([FromBody] UpdateNoteRequest noteDto)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        
        var noteResult = await _notesService.UpdateNoteAsync(noteDto);
        if (noteResult.IsFailed)
        {
            return BadRequest(noteResult.Errors.First());
        }
        
        return Ok(noteResult.Value);
    }
    
    [HttpDelete("{noteId}")]
    public async Task<IActionResult> DeleteNote(Guid noteId)
    {
        var deleteResult = await _notesService.DeleteNoteAsync(noteId);
        if (deleteResult.IsFailed)
        {
            return BadRequest(deleteResult.Errors.First());
        }
        
        return Ok();
    }
}