namespace MyBlog.Application.Common.Exceptions;

public class NotFoundException : CustomException
{ 
    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, new List<string>(){ innerException.ToString()})
    {
    }

    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}
