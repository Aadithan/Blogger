using MyBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Posts> Posts { get; } 

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
