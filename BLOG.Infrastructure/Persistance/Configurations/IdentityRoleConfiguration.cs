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
    public partial class IdentityRoleConfiguration : IEntityTypeConfiguration<Microsoft.AspNetCore.Identity.IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> entity)
        {
            entity.ToTable("AspNetRoles", (string)null);

            entity.Property<string>("Id")
                .HasColumnType("nvarchar(450)");

            entity.Property<string>("ConcurrencyStamp")
                .IsConcurrencyToken()
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("Name")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            entity.Property<string>("NormalizedName")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            entity.HasKey("Id");

            entity.HasIndex("NormalizedName")
                .IsUnique()
                .HasDatabaseName("RoleNameIndex")
                .HasFilter("[NormalizedName] IS NOT NULL");
        }

        partial void OnConfigurePartial(EntityTypeBuilder<IdentityRole> entity);
    }
}
