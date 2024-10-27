using FluentResults;
using Microsoft.EntityFrameworkCore;
using Mote.Api.Data;
using Mote.Api.Features.Notes.Types;
using Mote.Api.Models;
using Npgsql;

namespace Mote.Api.Features.Notes;

public interface INotesService
{
    Task<Result<List<Note>>> GetAllNotesAsync();
    Task<Result<List<Note>>> GetNotesByUserAsync(Guid userId);
    Task<Result<Note>> SaveNoteAsync(CreateNoteRequest noteDto);
    Task<Result<List<Note>>> GetChildren(Guid noteId);
    Task<Result<Note>> UpdateNoteAsync(UpdateNoteRequest noteDto);
    Task<Result> DeleteNoteAsync(Guid noteId);
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
        var notes = await _context.Notes.Where(n => n.ParentId == null).ToListAsync();
        foreach (var note in notes)
        {
            var childrenResult = await GetChildren(note.Id);
            if(!childrenResult.IsFailed)
            {
                note.Children = childrenResult.Value;
            }
        }
        return Result.Ok(notes);
    }
    
    public async Task<Result<List<Note>>> GetNotesByUserAsync(Guid userId)
    {
        var notes = await _context.Notes.Where(n => n.CreatedBy == userId).ToListAsync();
        return Result.Ok(notes);
    }
    
    public async Task<Result<Note>> SaveNoteAsync(CreateNoteRequest noteDto)
    {
        var note = new Note
        {
            Title = noteDto.Title,
            Content = noteDto.Content,
            ParentId = noteDto.ParentId,
            CreatedBy = Guid.Empty
        };
        
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
        
        return Result.Ok(note);
    }
    
    public async Task<Result<List<Note>>> GetChildren(Guid noteId)
    {
        var query = @"
            WITH RECURSIVE NoteHierarchy AS (
            SELECT * FROM public.""Notes"" WHERE ""Notes"".""Id"" = @noteId
            UNION ALL
            SELECT n.* FROM ""Notes"" n
            JOIN NoteHierarchy nh ON nh.""Id"" = n.""ParentId""
        )
        SELECT * FROM NoteHierarchy;
        ";
        
        var res = await _context.Notes.FromSqlRaw(query, new NpgsqlParameter("noteId", noteId))
            .ToListAsync();
        var notes = res.Select(n => n).ToList();
        return Result.Ok(notes);
    }

    public async Task<Result<Note>> UpdateNoteAsync(UpdateNoteRequest noteDto)
    {
        var note = await _context.Notes.FindAsync(noteDto.Id);
        if (note is null)
        {
            return Result.Fail<Note>($"Note with id {noteDto.Id} not found");
        }
        
        note.Title = noteDto.Title;
        note.Content = noteDto.Content;
        note.ParentId = noteDto.ParentId;
        
        if (noteDto.ParentId.HasValue)
        {
            if (note.ParentId.HasValue)
            {
                if (note.ParentId != noteDto.ParentId)
                {
                    note.ParentId = noteDto.ParentId;
                    var oldParent = await _context.Notes
                        .Include(p => p.Children)
                        .FirstOrDefaultAsync(p => p.Id == note.ParentId);
                    
                    if (oldParent is not null)
                    {
                        oldParent.Children.Remove(note);
                    }
                }
            }
            else
            {
                var parent = await _context.Notes.FindAsync(noteDto.ParentId);
                if (parent is not null)
                {
                    parent.Children.Add(note);
                }
                else
                {
                    return Result.Fail<Note>($"Parent note with id {noteDto.ParentId} not found");
                }
            }
        }
        
        await _context.SaveChangesAsync();
        
        return Result.Ok(note);
    }
    
    public async Task<Result> DeleteNoteAsync(Guid noteId)
    {
        var note = await _context.Notes
            .Include(n => n.Parent)
            .FirstOrDefaultAsync(n => n.Id == noteId);
        
        if (note is null)
        {
            return Result.Fail($"Note with id {noteId} not found");
        }

        var childrenResult = await GetChildren(note.Id);
        if (!childrenResult.IsFailed)
        {
            var children = childrenResult.Value;
            if (note.Parent is not null)
            {
                foreach(var child in children)
                {
                    child.ParentId = note.ParentId;
                    note.Parent.Children.Add(child);
                }
                note.Parent.Children.Remove(note);
            }
        }
        
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        
        return Result.Ok();
    }
}