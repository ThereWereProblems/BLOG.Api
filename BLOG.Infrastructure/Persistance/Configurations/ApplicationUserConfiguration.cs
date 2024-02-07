using BLOG.Domain.Model.ApplicationUser;
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
    public partial class ApplicationUserConfiguration : IEntityTypeConfiguration<Domain.Model.ApplicationUser.ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity.ToTable("AspNetUsers", (string)null);

            entity.Property<string>("Id")
                .HasColumnType("nvarchar(450)");

            entity.Property<int>("AccessFailedCount")
                .HasColumnType("int");

            entity.Property<string>("ConcurrencyStamp")
                .IsConcurrencyToken()
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("Email")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            entity.Property<bool>("EmailConfirmed")
                .HasColumnType("bit");

            entity.Property<bool>("LockoutEnabled")
                .HasColumnType("bit");

            entity.Property<DateTimeOffset?>("LockoutEnd")
                .HasColumnType("datetimeoffset");

            entity.Property<string>("NickName")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("NormalizedEmail")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            entity.Property<string>("NormalizedUserName")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            entity.Property<string>("PasswordHash")
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("PhoneNumber")
                .HasColumnType("nvarchar(max)");

            entity.Property<bool>("PhoneNumberConfirmed")
                .HasColumnType("bit");

            entity.Property<string>("SecurityStamp")
                .HasColumnType("nvarchar(max)");

            entity.Property<bool>("TwoFactorEnabled")
                .HasColumnType("bit");

            entity.Property<string>("UserName")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            entity.HasKey("Id");

            entity.HasIndex("NormalizedEmail")
                .HasDatabaseName("EmailIndex");

            entity.HasIndex("NormalizedUserName")
                .IsUnique()
                .HasDatabaseName("UserNameIndex")
                .HasFilter("[NormalizedUserName] IS NOT NULL");

            entity.Navigation("Posts");
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ApplicationUser> entity);
    }
}
