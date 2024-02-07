using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Infrastructure.Persistance.Configurations
{
    public partial class IdentityUserRoleConfiguration : IEntityTypeConfiguration<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> entity)
        {
            entity.ToTable("AspNetUserRoles", (string)null);

            entity.Property<string>("UserId")
                .HasColumnType("nvarchar(450)");

            entity.Property<string>("RoleId")
                .HasColumnType("nvarchar(450)");

            entity.HasKey("UserId", "RoleId");

            entity.HasIndex("RoleId");

            entity.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                .WithMany()
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasOne("BLOG.Domain.Model.ApplicationUser.ApplicationUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }

        partial void OnConfigurePartial(EntityTypeBuilder<IdentityUserRole<string>> entity);
    }
}
