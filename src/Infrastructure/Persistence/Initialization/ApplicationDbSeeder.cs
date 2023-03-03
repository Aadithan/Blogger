using MyBlog.Infrastructure.Identity;
using MyBlog.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBlog.Domain.Entities;

namespace MyBlog.Infrastructure.Persistence.Initialization;

internal class ApplicationDbSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CustomSeederRunner _seederRunner;
    private readonly ILogger<ApplicationDbSeeder> _logger;
    private readonly ApplicationDbContext _db;


    public ApplicationDbSeeder(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger)
    {
        _db = db;
        _roleManager = roleManager;
        _userManager = userManager;
        _seederRunner = seederRunner;
        _logger = logger;
    }

    public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        await SeedAdminUserAsync();
        await SeedBlogTables();
        await _seederRunner.RunSeedersAsync(cancellationToken);
    } 

    private async Task SeedAdminUserAsync()
    {
        if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == "admin@gmail.com")
            is not ApplicationUser adminUser)
        {
            adminUser = new ApplicationUser
            {
                FirstName = "AdminFirstName",
                LastName = "AdminLastName",
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = "admin@gmail.com".ToUpperInvariant(),
                NormalizedUserName = "admin@gmail.com".ToUpperInvariant(),
                IsActive = true
            };
            _logger.LogInformation("Seeding Default Admin User");
            _logger.LogInformation("User name: admin@gmail.com");
            _logger.LogInformation("Password: Welcome@123");
            var password = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, "Welcome@123");
            await _userManager.CreateAsync(adminUser);
        }
    }

    private async Task SeedBlogTables()
    {
        if (_db.Posts.Count() <= 0)
        {
            var post = new Posts()
            {
                PostRowID = 1,
                BlogID = Guid.NewGuid(),
                PostID = Guid.NewGuid(),
                Title = "Sample Post",
                Description = "Sample Description",
                PostContent = "Sample Content",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Author = "Admin",
                IsPublished = true,
                IsCommentEnabled = true,
                Raters = 4,
                Rating = (float?)4.5,
                Slug = "Slug",
                IsDeleted = false
            };

            _logger.LogInformation("Sample post seeded.");
            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();
        } 
    }
}