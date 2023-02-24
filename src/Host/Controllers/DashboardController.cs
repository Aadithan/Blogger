using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MyBlog.Infrastructure.Identity;
using MyBlog.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Cors;
using NSwag.Annotations;
using MyBlog.Host.Controllers.Common;

namespace MyBlog.Host.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[EnableCors("MyPolicy")]
public class DashboardController : VersionedApiController
{
    private readonly ClaimsPrincipal _caller;
    private readonly ApplicationDbContext _appDbContext;

    public DashboardController(UserManager<ApplicationUser> userManager, ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _caller = httpContextAccessor.HttpContext.User;
        _appDbContext = appDbContext;
    }

    // GET api/dashboard/home
    [HttpGet]
    public IActionResult Home()
    {
        // retrieve the user info
        //HttpContext.User
        //var userId = _caller.Claims.Single(c => c.Type == "id");
        //var customer = await _appDbContext.u.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

        //return new OkObjectResult(new
        //{
        //  Message = "This is secure API and user data!",
        //  customer.Identity.FirstName,
        //  customer.Identity.LastName,
        //  customer.Identity.PictureUrl,
        //  customer.Identity.FacebookId,
        //  customer.Location,
        //  customer.Locale,
        //  customer.Gender
        //});
        return new OkObjectResult(null);
    }


    [HttpGet]
    [OpenApiTag("sdfasadf")]
    [OpenApiOperation("1","GetOperation", "GetOperation Description")]
    public string Get()
    {
        return "Test";
    }
}