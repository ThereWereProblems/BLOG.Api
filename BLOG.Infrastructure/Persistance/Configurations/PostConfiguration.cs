using BLOG.Domain.Model.Post;
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
    public partial class PostConfiguration : IEntityTypeConfiguration<Domain.Model.Post.Post>
    {
        public void Configure(EntityTypeBuilder<Post> entity)
        {
            entity.ToTable("Posts");

            entity.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(entity.Property<int>("Id"));

            entity.Property<string>("Content")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("Description")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("Image")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property<DateTime>("PublishedAt")
                .HasColumnType("datetime2");

            entity.Property<string>("Title")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property<string>("UserId")
                .HasColumnType("nvarchar(450)");

            entity.HasKey("Id");

            entity.HasIndex("UserId");

            entity.HasOne("BLOG.Domain.Model.ApplicationUser.ApplicationUser", "User")
                .WithMany("Posts")
                .HasForeignKey("UserId");

            entity.Navigation("User");

            entity.Navigation("Comments");
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Post> entity);
    }
}
