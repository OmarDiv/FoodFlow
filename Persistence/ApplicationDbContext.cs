using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodFlow.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Restaurant> Restaurants { get; set; } = null!;
        // public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<MenuItem> MenuItems { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            var cascade = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);
            foreach (var fk in cascade)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();
            foreach (var entityEntry in entries)
            {
                var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(e => e.CreatedById).CurrentValue = CurrentUserId;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(e => e.UpdatedById).CurrentValue = CurrentUserId;
                    entityEntry.Property(e => e.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}