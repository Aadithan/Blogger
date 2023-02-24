using Microsoft.AspNetCore.Identity;

namespace MyBlog.Infrastructure.Identity;

// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
public class ApplicationUser : IdentityUser
{

    //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    //{
    //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //    // Add custom user claims here
    //    return userIdentity;
    ////}

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public long? FacebookId { get; set; }
    public string? PictureUrl { get; set; } 

    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; } 
    public string? ObjectId { get; set; }
}