using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MyBlog.Infrastructure.Identity;
using MyBlog.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Cors;
using NSwag.Annotations;
using MyBlog.Host.Controllers.Common;
using System.Threading.Tasks;
using System;
using MyBlog.Application.Common.Models;
using MyBlog.Application.Models;

namespace MyBlog.Host.Controllers;

//[Authorize]
[Route("api/[controller]/[action]")]
[EnableCors("MyPolicy")]
public class PostsController : VersionedApiController
{
    private readonly ClaimsPrincipal _caller;
    private readonly ApplicationDbContext _appDbContext;

    public PostsController(UserManager<ApplicationUser> userManager, ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _caller = httpContextAccessor.HttpContext.User;
        _appDbContext = appDbContext;
    } 

    [HttpGet]
    [OpenApiTag("Blog")]
    [OpenApiOperation("1","GetBlogPosts", "To Get all Posts by the user")]
    public Task<PaginationResponse<PostsDto>> SearchAsync(GetPostsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet]
    [OpenApiTag("Blog")]
    [OpenApiOperation("1", "GetBlogPosts", "To Get all Posts by the user")]
    public Task<PaginationResponse<PostsDto>> Search234()
    {
        return Mediator.Send(new GetPostsRequest());
    }
}