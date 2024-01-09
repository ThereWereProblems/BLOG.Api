using BLOG.Application.Common.Abstractions;
using BLOG.Domain.Model.ApplicationUser;
using BLOG.Domain.Model.AuditLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Infrastructure.Persistance
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly Lazy<ICurentUserService> _userService;

        public ApplicationDbContext(IServiceProvider serviceProvider) : base()
        {
            _userService = new Lazy<ICurentUserService>(() =>
                serviceProvider.GetRequiredService<ICurentUserService>()); // jeśli damy bezpośrednio - A circular dependency was detected
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IServiceProvider serviceProvider) : base(options)
        {
            _userService = new Lazy<ICurentUserService>(() =>
                serviceProvider.GetRequiredService<ICurentUserService>()); // jeśli damy bezpośrednio - A circular dependency was detected
        }

        public virtual DbSet<Domain.Model.AuditLog.AuditLog> AuditLogs { get; set; }
        public virtual DbSet<Domain.Model.Comment.Comment> Comments { get; set; }
        public virtual DbSet<Domain.Model.Post.Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Audit setup
            // https://code-maze.com/aspnetcore-audit-trail/

            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added
                || e.State == EntityState.Modified
                || e.State == EntityState.Deleted)
                .ToList();

            foreach (var modifiedEntity in modifiedEntities)
            {
                var auditLog = new AuditLog
                {
                    EntityName = modifiedEntity.Entity.GetType().Name,
                    Action = modifiedEntity.State.ToString(),
                    Timestamp = DateTime.Now,
                    Changes = GetChanges(modifiedEntity),
                    User = _userService.Value.UserId
                };
                AuditLogs.Add(auditLog);
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        private static string GetChanges(EntityEntry entity)
        {
            var changes = new StringBuilder();
            foreach (var property in entity.OriginalValues.Properties)
            {
                var originalValue = entity.OriginalValues[property];
                var currentValue = entity.CurrentValues[property];
                if (!Equals(originalValue, currentValue))
                {
                    changes.AppendLine($"{property.Name}: From '{originalValue}' to '{currentValue}'");
                }
            }
            return changes.ToString();
        }
    }
}
