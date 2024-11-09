using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Slugify;

namespace Mote.Api.Models;

public class Note : MetadataModel
{
    [MaxLength(255)]
    public string Title { get; set; }
    public string Slug { get; set; }

    public string Path { get; set; }
    
    public string? Content { get; set; }
    public bool IsArchived { get; set; }
    public bool IsTask { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ParentId { get; set; }
    public Note? Parent { get; set; }
    public IList<Note> Children { get; set; } = new List<Note>();
    [NotMapped]
    public bool IsRoot => ParentId == null;
    
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}