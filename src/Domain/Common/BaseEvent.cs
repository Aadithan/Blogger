using MediatR;

namespace MyBlog.Domain.Common;

public abstract class DomainEvent : INotification
{
    public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}
