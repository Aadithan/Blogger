using System.Reflection;
using MyBlog.Application.Common.Interfaces;
using MyBlog.Infrastructure.Identity;
using MyBlog.Infrastructure.Persistence.Interceptors; 
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyBlog.Domain.Entities;

namespace MyBlog.Infrastructure.Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor; 

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options, 
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public virtual DbSet<Posts> Posts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        IntializeIdentityTables(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    #region MyRegion

    private void IntializeIdentityTables(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(b =>
        {
            b.Property<string>("Id")
            .ValueGeneratedOnAdd();

            b.Property<int>("AccessFailedCount");

            b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();

            b.Property<string>("Email")
                    .HasMaxLength(256);

            b.Property<bool>("EmailConfirmed");

            b.Property<bool>("LockoutEnabled");

            b.Property<DateTimeOffset?>("LockoutEnd");

            b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256);

            b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256);

            b.Property<string>("PasswordHash");

            b.Property<string>("PhoneNumber");

            b.Property<bool>("PhoneNumberConfirmed");

            b.Property<string>("SecurityStamp");

            b.Property<bool>("TwoFactorEnabled");

            b.Property<string>("UserName")
                    .HasMaxLength(256);

            b.Property<string>("FirstName")
                    .HasMaxLength(256);

            b.Property<string>("LastName")
                    .HasMaxLength(256);

            b.Property<long?>("FacebookId");

            b.Property<string>("PictureUrl")
                .HasMaxLength(256);

            b.HasKey("Id");

            b.HasIndex("NormalizedEmail")
                    .HasName("EmailIndex");

            b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasName("UserNameIndex")
                    .HasFilter("[NormalizedUserName] IS NOT NULL");

            b.ToTable("AspNetUsers");
        });
    }

    #endregion
}
