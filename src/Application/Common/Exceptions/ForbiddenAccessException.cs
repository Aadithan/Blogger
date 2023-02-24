namespace MyBlog.Application.Common.Exceptions;

public class ForbiddenAccessException : CustomException
{
    public ForbiddenAccessException(string message)
        : base(message, null, HttpStatusCode.Forbidden)
    {
    }
}