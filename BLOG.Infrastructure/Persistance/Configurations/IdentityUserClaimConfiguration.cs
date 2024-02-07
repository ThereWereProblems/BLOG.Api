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
    public partial class IdentityUserClaimConfiguration : IEntityTypeConfiguration<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> entity)
        {
            entity.ToTable("AspNetUserClaims", (string)null);

            entity.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(entity.Property<int>("Id"));

            entity.Property<string>("ClaimType")
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("ClaimValue")
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("UserId")
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            entity.HasKey("Id");

            entity.HasIndex("UserId");

            entity.HasOne("BLOG.Domain.Model.ApplicationUser.ApplicationUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }

        partial void OnConfigurePartial(EntityTypeBuilder<IdentityUserClaim<string>> entity);
    }
}
