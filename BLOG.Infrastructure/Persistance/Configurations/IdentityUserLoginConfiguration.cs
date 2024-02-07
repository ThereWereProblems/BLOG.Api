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
    public partial class IdentityUserLoginConfiguration : IEntityTypeConfiguration<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> entity)
        {
            entity.ToTable("AspNetUserLogins", (string)null);

            entity.Property<string>("LoginProvider")
                .HasColumnType("nvarchar(450)");

            entity.Property<string>("ProviderKey")
                .HasColumnType("nvarchar(450)");

            entity.Property<string>("ProviderDisplayName")
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("UserId")
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            entity.HasKey("LoginProvider", "ProviderKey");

            entity.HasIndex("UserId");

            entity.HasOne("BLOG.Domain.Model.ApplicationUser.ApplicationUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }

        partial void OnConfigurePartial(EntityTypeBuilder<IdentityUserLogin<string>> entity);
    }
}
