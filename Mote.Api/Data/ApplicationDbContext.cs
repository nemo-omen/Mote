using Mote.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Mote.Api.Data;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IUserResolverService _userResolverService;
    // ReSharper disable once ConvertToPrimaryConstructor
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IUserResolverService userResolverService) : base(options)
    {
        _userResolverService = userResolverService;
    }

    public DbSet<Note> Notes { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new MetadataModelInterceptor(_userResolverService));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Note>()
            .HasOne(n => n.Parent)
            .WithMany(n => n.Children)
            .HasForeignKey(n => n.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}