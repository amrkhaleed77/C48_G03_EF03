using C48_G03_EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C48_G03_EF03.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Description)
                   .HasMaxLength(300);

            builder.HasOne(c => c.ParentCategory)
                   .WithMany(c => c.SubCategories)
                   .HasForeignKey(c => c.ParentCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasData(
                new Category { Id = 1, Name = "Programming", Description = "All programming courses", ParentCategoryId = null },
                new Category { Id = 2, Name = "Backend", Description = "Server side development", ParentCategoryId = 1 },
                new Category { Id = 3, Name = "ASP.NET", Description = null, ParentCategoryId = 2 },
                new Category { Id = 4, Name = "Design", Description = "Design courses", ParentCategoryId = null },
                new Category { Id = 5, Name = "Web Design", Description = null, ParentCategoryId = 4 },
                new Category { Id = 6, Name = "Data Science", Description = null, ParentCategoryId = null }
            );
        }
    }
}
