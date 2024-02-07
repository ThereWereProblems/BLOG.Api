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
    public partial class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> entity)
        {
            entity.ToTable("AspNetRoleClaims", (string)null);

            entity.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(entity.Property<int>("Id"));

            entity.Property<string>("ClaimType")
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("ClaimValue")
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("RoleId")
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            entity.HasKey("Id");

            entity.HasIndex("RoleId");

            entity.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                .WithMany()
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }

        partial void OnConfigurePartial(EntityTypeBuilder<IdentityRoleClaim<string>> entity);
    }
}
