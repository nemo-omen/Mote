using Mote.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Slugify;

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

    public override int SaveChanges()
    {
        UpdatePathsAndSlugs();
        return base.SaveChanges();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdatePathsAndSlugs();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdatePathsAndSlugs()
    {
        foreach (var entry in ChangeTracker.Entries<Note>())
        {
            if(entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.Slug = GetSlug(entry.Entity.Title);
                entry.Entity.Path = GetPath(entry.Entity.Parent, entry.Entity.Slug);
            }
        }
    }
    
    private string GetPath(Note? parent, string slug)
    {
        return !string.IsNullOrWhiteSpace(parent?.Path) ? parent.Path + "/" + slug : slug;
    }
    
    private string GetSlug(string title)
    {
        var slugHelper = new SlugHelper();
        return slugHelper.GenerateSlug(title);
    }
}