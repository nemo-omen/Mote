using Microsoft.EntityFrameworkCore;
using Moq;
using Mote.Api.Data;

namespace Mote.Test;

public static class InMemoryContextFactory
{
    public static ApplicationDbContext Create()
    {
        var mockUserResolver = new Mock<IUserResolverService>();
        mockUserResolver.Setup(x => x.GetUserIdentityName()).Returns("test-user");
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        return new ApplicationDbContext(options, mockUserResolver.Object);
    }
}