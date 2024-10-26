using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Mote.Api.Models;

public class Note : MetadataModel
{
    [MaxLength(255)]
    public string Title { get; set; } = "untitled";
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