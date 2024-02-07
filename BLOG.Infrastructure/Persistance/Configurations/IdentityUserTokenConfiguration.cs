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
    public partial class IdentityUserTokenConfiguration : IEntityTypeConfiguration<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<string>> entity)
        {
            entity.ToTable("AspNetUserTokens", (string)null);

            entity.Property<string>("UserId")
                .HasColumnType("nvarchar(450)");

            entity.Property<string>("LoginProvider")
                .HasColumnType("nvarchar(450)");

            entity.Property<string>("Name")
                .HasColumnType("nvarchar(450)");

            entity.Property<string>("Value")
                .HasColumnType("nvarchar(max)");

            entity.HasKey("UserId", "LoginProvider", "Name");

            entity.HasOne("BLOG.Domain.Model.ApplicationUser.ApplicationUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }

        partial void OnConfigurePartial(EntityTypeBuilder<IdentityUserToken<string>> entity);
    }
}
