using BLOG.Domain.Model.Comment;
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
    public partial class CommentConfiguration : IEntityTypeConfiguration<Domain.Model.Comment.Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> entity)
        {
            entity.ToTable("Comments");

            entity.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(entity.Property<int>("Id"));

            entity.Property<string>("Content")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property<int>("PostId")
                .HasColumnType("int");

            entity.Property<DateTime>("PublishedAt")
                .HasColumnType("datetime2");

            entity.Property<string>("UserId")
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            entity.HasKey("Id");

            entity.HasIndex("PostId");

            entity.HasIndex("UserId");

            entity.HasOne("BLOG.Domain.Model.Post.Post", "Post")
                .WithMany("Comments")
                .HasForeignKey("PostId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasOne("BLOG.Domain.Model.ApplicationUser.ApplicationUser", "User")
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.Navigation("Post");

            entity.Navigation("User");
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Comment> entity);
    }
}
