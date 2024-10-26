using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace Mote.Api.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(255)]
    public string? Name { get; set; }
    
    private string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}