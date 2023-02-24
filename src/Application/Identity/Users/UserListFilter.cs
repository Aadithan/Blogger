namespace MyBlog.Application.Identity.Users;

public class UserListFilter 
{
    public bool? IsActive { get; set; }

    public int PageNumber { get; set; }
}