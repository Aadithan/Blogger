using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using MyBlog.Application.Models;
using MyBlog.Infrastructure.Common.Helpers;
using MyBlog.Infrastructure.Identity;
using MyBlog.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Cors;
using MyBlog.Host.Controllers.Common;

namespace MyBlog.Host.Controllers.Identity;

public class Credentials
{
    public string Email { get; set; }
    public string Password { get; set; }
}

[Produces("application/json")]
[Route("api/Account")]
[EnableCors("MyPolicy")]
public class AccountController : VersionNeutralApiController
{
    private readonly ApplicationDbContext _appDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public AccountController(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext appDbContext)
    {
        _userManager = userManager;
        _mapper = mapper;
        _appDbContext = appDbContext;
    }


    // POST api/account
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //var userIdentity = _mapper.Map<ApplicationUser>(model);
        var userIdentity = new ApplicationUser();
        userIdentity.FirstName = model.FirstName;
        userIdentity.LastName = model.LastName;
        userIdentity.UserName = model.Email;
        userIdentity.Email = model.Email;
        userIdentity.PasswordHash = model.Password;
        var result = await _userManager.CreateAsync(userIdentity, model.Password);

        if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

        //await _appDbContext.Customers.AddAsync(new Customer { IdentityId = userIdentity.Id, Location = model.Location });
        await _appDbContext.SaveChangesAsync();

        return new OkObjectResult("Account created");
    }
}
