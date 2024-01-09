using BLOG.Domain.Model.ApplicationUser;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Common.Abstractions
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Model.AuditLog.AuditLog> AuditLogs { get; set; }
        DbSet<Domain.Model.Comment.Comment> Comments { get; set; }
        DbSet<ApplicationUser> Users { get; set; }
        DbSet<Domain.Model.Post.Post> Posts { get; set; }

        Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default);
    }
}
