using MyBlog.Infrastructure.Identity;
using MyBlog.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging; 

namespace MyBlog.Infrastructure.Persistence.Initialization;

internal class ApplicationDbInitializer
{ 
    private readonly ILogger<ApplicationDbInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly ApplicationDbSeeder _dbSeeder;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbInitializer(ApplicationDbContext context, ApplicationDbSeeder dbSeeder, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<ApplicationDbInitializer> logger)
    {
        _logger = logger; 
        _context = context;
        _userManager = userManager;
        _dbSeeder = dbSeeder;
        _roleManager = roleManager;
    }  

    public async Task InitialiseAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_context.Database.GetMigrations().Any())
            { 
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            } 

            if (await _context.Database.CanConnectAsync(cancellationToken))
            { 
                await _dbSeeder.SeedDatabaseAsync(_context, cancellationToken);
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
     
}
