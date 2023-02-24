using MyBlog.Domain.Common;
using MyBlog.Infrastructure.Persistence.Context;

namespace MyBlog.Infrastructure.Persistence;

// Inherited from Ardalis.Specification's RepositoryBase<T>
public class ApplicationDbRepository<T> : IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public ApplicationDbRepository(ApplicationDbContext dbContext) 
    {
    } 
}