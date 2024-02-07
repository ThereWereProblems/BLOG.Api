using BLOG.Domain.Model.AuditLog;
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
    public partial class AuditLogConfiguration : IEntityTypeConfiguration<Domain.Model.AuditLog.AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> entity)
        {
            entity.ToTable("AuditLogs");

            entity.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(entity.Property<int>("Id"));

            entity.Property<string>("Action")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("Changes")
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("EntityName")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property<DateTime>("Timestamp")
                .HasColumnType("datetime2");

            entity.Property<string>("User")
                .HasColumnType("nvarchar(max)");

            entity.HasKey("Id");
        }

        partial void OnConfigurePartial(EntityTypeBuilder<AuditLog> entity);
    }
}
