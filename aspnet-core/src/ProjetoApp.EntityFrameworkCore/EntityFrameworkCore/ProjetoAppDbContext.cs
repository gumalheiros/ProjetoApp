using Microsoft.EntityFrameworkCore;
using ProjetoApp.Domain;
using ProjetoApp.Domain.Shared;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace ProjetoApp.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ProjetoAppDbContext :
    AbpDbContext<ProjetoAppDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<TaskHistory> TaskHistories { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public ProjetoAppDbContext(DbContextOptions<ProjetoAppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(ProjetoAppConsts.DbTablePrefix + "YourEntities", ProjetoAppConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        builder.Entity<Project>(b =>
        {
            b.ToTable(ProjetoAppConsts.DbTablePrefix + "Projects", ProjetoAppConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ProjectConsts.MaxNameLength);

            b.Property(x => x.Description)
                .HasMaxLength(ProjectConsts.MaxDescriptionLength);

            b.HasMany(x => x.Tasks)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired();

            b.HasIndex(x => x.UserId);
        });



        builder.Entity<ProjectTask>(b =>
        {
            b.ToTable(ProjetoAppConsts.DbTablePrefix + "ProjectTasks", ProjetoAppConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ProjectTaskConsts.MaxTitleLength);

            b.Property(x => x.Description)
                .HasMaxLength(ProjectTaskConsts.MaxDescriptionLength);

            b.Property(x => x.DueDate)
                .IsRequired();

            b.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            b.HasMany<TaskHistory>()
             .WithOne(h => h.Task)
             .HasForeignKey(h => h.TaskId);

            b.HasMany<TaskComment>()
             .WithOne(c => c.Task)
             .HasForeignKey(c => c.TaskId);

            b.HasIndex(x => x.ProjectId);
        });



        builder.Entity<Customer>(b =>
        {
            b.ToTable(ProjetoAppConsts.DbTablePrefix + "Customers", ProjetoAppConsts.DbSchema);
            b.ConfigureByConvention(); 
            

            /* Configure more properties here */
        });
    }
}
