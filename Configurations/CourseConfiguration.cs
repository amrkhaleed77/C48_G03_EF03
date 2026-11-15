using C48_G03_EF03.Enums;
using C48_G03_EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C48_G03_EF03.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.Description)
                   .HasMaxLength(1000);

            builder.Property(c => c.Price)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(c => c.Level)
                   .IsRequired();

            builder.Property(c => c.IsPublished)
                   .IsRequired();


            builder.HasOne(c => c.Instructor)
                   .WithMany(i => i.Courses)
                   .HasForeignKey(c => c.InstructorId);

            builder.HasOne(c => c.Category)
                   .WithMany(cat => cat.Courses)
                   .HasForeignKey(c => c.CategoryId);

            builder.HasData(
                new Course { Id = 1, Title = "C# Fundamentals", Description = "Learn C# from scratch", Price = 49.99m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2024, 1, 20), InstructorId = 1, CategoryId = 1 },
                new Course { Id = 2, Title = "ASP.NET Core Web API", Description = "Build REST APIs", Price = 89.99m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedDate = new DateTime(2024, 3, 5), InstructorId = 1, CategoryId = 3 },
                new Course { Id = 3, Title = "EF Core Deep Dive", Description = "Advanced EF Core topics", Price = 99.99m, Level = CourseLevel.Advanced, IsPublished = true, PublishedDate = new DateTime(2024, 4, 12), InstructorId = 2, CategoryId = 3 },
                new Course { Id = 4, Title = "SQL Server for Beginners", Description = null, Price = 39.99m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2024, 2, 1), InstructorId = 2, CategoryId = 2 },
                new Course { Id = 5, Title = "UI Design Basics", Description = "Intro to UI design", Price = 59.99m, Level = CourseLevel.Beginner, IsPublished = false, PublishedDate = null, InstructorId = 3, CategoryId = 5 },
                new Course { Id = 6, Title = "Microservices Architecture", Description = null, Price = 149.99m, Level = CourseLevel.Advanced, IsPublished = false, PublishedDate = null, InstructorId = 3, CategoryId = 2 }
            );
        }
    }
}
