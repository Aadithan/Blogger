 
using MyBlog.Application.Common.Models;
using MyBlog.Application.Identity.Users;
using MyBlog.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization; 
using AutoMapper;
using MyBlog.Application.Common.FileStorage;
using MyBlog.Application.Common.Mailing;

namespace MyBlog.Infrastructure.Identity;

internal partial class UserService : IUserService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;
    private readonly IMailService _mailService; 
    private readonly IEmailTemplateService _templateService;
    private readonly IFileStorageService _fileStorage;

    public UserService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMailService mailService,
        IEmailTemplateService templateService,
        IFileStorageService fileStorage,
        ApplicationDbContext db,
        IStringLocalizer<UserService> localizer)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _mailService = mailService;
        _templateService = templateService;
        _fileStorage = fileStorage;
        _db = db;
        _t = localizer; 
    }

    public async Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
    {
        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<ApplicationUser, UserDetailsDto>()
        );

        var mapper = new Mapper(config);
        

        var users = await _userManager.Users.ToListAsync(cancellationToken);

        var userDTO = mapper.Map<List<UserDetailsDto>>(users);

           
        int count = await _userManager.Users
            .CountAsync(cancellationToken);

        //todo page size

        return new PaginationResponse<UserDetailsDto>(userDTO, count, filter.PageNumber, 1);
    }

    public async Task<bool> ExistsWithNameAsync(string name)
    { 
        return await _userManager.FindByNameAsync(name) is not null;
    }

    public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
    { 
        return await _userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user && user.Id != exceptId;
    }

    public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
    { 
        return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
    } 

    public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
    {
        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<ApplicationUser, UserDetailsDto>()
        );

        var mapper = new Mapper(config);


        var users = await _userManager.Users.AsNoTracking().ToListAsync(cancellationToken);

        var userDTO = mapper.Map<List<UserDetailsDto>>(users);

        return userDTO;
    }
     

    public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
        _userManager.Users.AsNoTracking().CountAsync(cancellationToken);

    public async Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken)
    {

        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<ApplicationUser, UserDetailsDto>()
        );

        var mapper = new Mapper(config);


        var users = await _userManager.Users.AsNoTracking().Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        var userDTO = mapper.Map<UserDetailsDto>(users);

        return userDTO; 
    } 
}