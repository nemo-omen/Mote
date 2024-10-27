namespace Mote.Api.Features.Notes.Types;

public record UpdateNoteRequest
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Content { get; init; }
    public Guid? ParentId { get; init; }
}