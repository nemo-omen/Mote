namespace Mote.Api.Features.Notes.Types;

public record CreateNoteRequest
{
    public string Title { get; init; } = "";
    public string? Content { get; init; }
    public Guid? ParentId { get; init; }
}