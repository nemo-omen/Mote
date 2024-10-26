using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mote.Api.Data;
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
    public async Task<IActionResult> Index()
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
}