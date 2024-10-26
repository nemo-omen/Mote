using FluentResults;
using Microsoft.EntityFrameworkCore;
using Mote.Api.Data;
using Mote.Api.Models;

namespace Mote.Api.Features.Notes;

public interface INotesService
{
    Task<Result<List<Note>>> GetAllNotesAsync();
    Task<Result<List<Note>>> GetNotesByUserAsync(Guid userId);
}

public class NotesService : INotesService
{
    private readonly ApplicationDbContext _context;
    
    public NotesService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<List<Note>>> GetAllNotesAsync()
    {
        var notes = await _context.Notes.ToListAsync();
        return Result.Ok(notes);
    }
    
    public async Task<Result<List<Note>>> GetNotesByUserAsync(Guid userId)
    {
        var notes = await _context.Notes.Where(n => n.CreatedBy == userId).ToListAsync();
        return Result.Ok(notes);
    }
}