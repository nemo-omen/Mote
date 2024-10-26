// using Mote.Api.Features.Identity;
using Mote.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Client;

namespace Mote.Api.Data;

public class MetadataModelInterceptor : SaveChangesInterceptor
{
    private readonly IUserResolverService _userResolverService;
    
    public MetadataModelInterceptor(IUserResolverService userResolverService)
    {
        _userResolverService = userResolverService;
    }
    
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken)
    {
        if(eventData.Context is not null)
        {
            UpdateMetadataEntries(eventData.Context, _userResolverService);
        }
     
        // return new ValueTask<InterceptionResult<int>>(result);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if(eventData.Context is not null)
        {
            UpdateMetadataEntries(eventData.Context, _userResolverService);
        }
        
        // return result;
        return base.SavingChanges(eventData, result);
    }
    
    private void UpdateMetadataEntries(DbContext eventDataContext, IUserResolverService userResolverService)
    {
        var appDbContext = (ApplicationDbContext)eventDataContext;
        var userIdName = userResolverService.GetUserIdentityName();
        if (string.IsNullOrWhiteSpace(userIdName))
        {
            return;
        }

        var entityEntries = appDbContext
            .ChangeTracker
            .Entries()
            .Where(e => e.Entity is MetadataModel && (e.State == EntityState.Added || e.State == EntityState.Modified));
        
        foreach (var entry in entityEntries)
        {
            var now = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedBy").CurrentValue = userIdName;
                entry.Property("CreatedAt").CurrentValue = now;
            }
            
            if(entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedBy").CurrentValue = userIdName;
                entry.Property("UpdatedAt").CurrentValue = now;
            }
        }
    }
}