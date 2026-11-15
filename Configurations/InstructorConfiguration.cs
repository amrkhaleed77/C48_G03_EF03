using C48_G03_EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C48_G03_EF03.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(i => i.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(i => i.Bio)
                   .HasMaxLength(500);

            builder.Property(i => i.JoinedDate)
                   .IsRequired();

            builder.HasData(
                new Instructor { Id = 1, FullName = "Ahmed Hassan", Email = "ahmed.hassan@learn.com", Bio = "Backend developer with 10 years of experience.", JoinedDate = new DateTime(2023, 1, 10) },
                new Instructor { Id = 2, FullName = "Mona Ali", Email = "mona.ali@learn.com", Bio = "Database and data engineering instructor.", JoinedDate = new DateTime(2023, 6, 1) },
                new Instructor { Id = 3, FullName = "Omar Khaled", Email = "omar.khaled@learn.com", Bio = null, JoinedDate = new DateTime(2024, 2, 15) }
            );
        }
    }
}
