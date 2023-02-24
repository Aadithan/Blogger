using MyBlog.Application.Common.Interfaces;
using MyBlog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Options;

namespace MyBlog.Infrastructure.Persistence.Context;

public abstract class BaseDbContext
{
    protected readonly ICurrentUser _currentUser;
    private readonly ISerializerService _serializer;
    private readonly DatabaseSettings _dbSettings;

    protected BaseDbContext(DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings)
    {
        _currentUser = currentUser;
        _serializer = serializer;
        _dbSettings = dbSettings.Value;
    }
}