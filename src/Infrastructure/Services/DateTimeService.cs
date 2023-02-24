using MyBlog.Application.Common.Interfaces;

namespace MyBlog.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
