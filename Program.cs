using C48_G03_EF03.Data;
using C48_G03_EF03.Enums;
using C48_G03_EF03.Models;
using Microsoft.EntityFrameworkCore;

namespace C48_G03_EF03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Migrate();
                SeedStudentsAndEnrollments(context);
            }

            PrintHeader("Create - Add an instructor");
            AddInstructor("Sara Mahmoud", "sara.mahmoud@learn.com", "Cloud and DevOps instructor", new DateTime(2025, 1, 5));

            PrintHeader("Create - Add a course");
            AddCourse("Docker Essentials", "Containers for developers", 69.99m, CourseLevel.Beginner, 1, 1);
            AddCourse("Ghost Course", null, 10m, CourseLevel.Beginner, 999, 1);   // instructor not found
            AddCourse("Ghost Course", null, 10m, CourseLevel.Beginner, 1, 999);   // category not found

            PrintHeader("Create - Add several students (AddRange)");
            AddStudents();

            PrintHeader("Create - Enroll a student");
            EnrollStudent(5, 1);     
            EnrollStudent(5, 1);     
            EnrollStudent(5, 5);     
            EnrollStudent(999, 1);   
            EnrollStudent(5, 999);   

            PrintHeader("Read - Find a course");
            FindCourse(2, "ASP.NET Core Web API");
            FindCourse(999, "No Such Course");

            PrintHeader("Read - Filter courses by price");
            FilterCourses(40m, 100m);

            PrintHeader("Read - Sub-categories");
            PrintSubCategories(1);
            PrintTopLevelCategories();

            PrintHeader("Read - Course roster");
            PrintCourseRoster(1);
            PrintCourseRoster(999);

            PrintHeader("Update - Change a price");
            ChangePrice(1, 59.99m);
            ChangePrice(999, 10m);

            PrintHeader("Update - Update progress");
            UpdateProgress(1, 2, 75);     
            UpdateProgress(3, 4, 100);    
            UpdateProgress(1, 2, 150);    
            UpdateProgress(999, 2, 50);   

            PrintHeader("Update - Publish a course");
            PublishCourse(5);
            PublishCourse(5);             
            PublishCourse(999);

            PrintHeader("Delete - Unenroll");
            Unenroll(3, 4);
            Unenroll(3, 4);               

            PrintHeader("Delete - Delete a category");
            DeleteCategoryUnsafe(1);      
            DeleteCategorySafe(1);        
            DeleteCategorySafe(6);       

            PrintHeader("Delete - Delete a course");
            DeleteCourse(1);

            PrintHeader("Bulk - Price raise 10%");
            BulkRaisePrice(CourseLevel.Beginner);

            PrintHeader("Bulk - Delete dropped enrollments");
            BulkDeleteDropped();

            Console.WriteLine();
            Console.WriteLine("Done.");
        }

        static void SeedStudentsAndEnrollments(AppDbContext context)
        {
if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student { FullName = "Youssef Adel", Email = "youssef.adel@mail.com", DateOfBirth = new DateTime(2002, 3, 14) },
                    new Student { FullName = "Nour Ibrahim", Email = "nour.ibrahim@mail.com", DateOfBirth = new DateTime(2001, 7, 2) },
                    new Student { FullName = "Mariam Samir", Email = "mariam.samir@mail.com", DateOfBirth = new DateTime(2003, 11, 25) },
                    new Student { FullName = "Karim Fathy", Email = "karim.fathy@mail.com", DateOfBirth = new DateTime(2000, 1, 30) },
                    new Student { FullName = "Hana Mostafa", Email = "hana.mostafa@mail.com", DateOfBirth = new DateTime(2002, 9, 9) }
                );
                context.SaveChanges();
            }

            if (!context.Enrollments.Any())
            {
                context.Enrollments.AddRange(
                    new Enrollment { StudentId = 1, CourseId = 1, EnrollmentDate = new DateTime(2024, 5, 1), ProgressPercentage = 100, Status = EnrollmentStatus.Completed, FinalGrade = 92.5m },
                    new Enrollment { StudentId = 1, CourseId = 2, EnrollmentDate = new DateTime(2024, 5, 10), ProgressPercentage = 40, Status = EnrollmentStatus.Active, FinalGrade = null },
                    new Enrollment { StudentId = 2, CourseId = 1, EnrollmentDate = new DateTime(2024, 5, 3), ProgressPercentage = 60, Status = EnrollmentStatus.Active, FinalGrade = null },
                    new Enrollment { StudentId = 2, CourseId = 3, EnrollmentDate = new DateTime(2024, 6, 1), ProgressPercentage = 15, Status = EnrollmentStatus.Dropped, FinalGrade = null },
                    new Enrollment { StudentId = 3, CourseId = 2, EnrollmentDate = new DateTime(2024, 5, 20), ProgressPercentage = 100, Status = EnrollmentStatus.Completed, FinalGrade = 85m },
                    new Enrollment { StudentId = 3, CourseId = 4, EnrollmentDate = new DateTime(2024, 6, 5), ProgressPercentage = 25, Status = EnrollmentStatus.Active, FinalGrade = null },
                    new Enrollment { StudentId = 4, CourseId = 3, EnrollmentDate = new DateTime(2024, 6, 8), ProgressPercentage = 10, Status = EnrollmentStatus.Dropped, FinalGrade = null },
                    new Enrollment { StudentId = 5, CourseId = 4, EnrollmentDate = new DateTime(2024, 6, 12), ProgressPercentage = 100, Status = EnrollmentStatus.Completed, FinalGrade = 78m }
                );
                context.SaveChanges();
            }
        }

     
        static void AddInstructor(string fullName, string email, string? bio, DateTime joinedDate)
        {
            using var context = new AppDbContext();

            var instructor = new Instructor
            {
                FullName = fullName,
                Email = email,
                Bio = bio,
                JoinedDate = joinedDate
            };

            context.Instructors.Add(instructor);
            context.SaveChanges();

            Console.WriteLine($"Instructor added. Generated Id = {instructor.Id}");
        }

        static void AddCourse(string title, string? description, decimal price, CourseLevel level, int instructorId, int categoryId)
        {
            using var context = new AppDbContext();

            if (context.Instructors.Find(instructorId) == null)
            {
                Console.WriteLine($"Cannot add the course: instructor {instructorId} does not exist.");
                return;
            }

            if (context.Categories.Find(categoryId) == null)
            {
                Console.WriteLine($"Cannot add the course: category {categoryId} does not exist.");
                return;
            }

            var course = new Course
            {
                Title = title,
                Description = description,
                Price = price,
                Level = level,
                IsPublished = false,
                PublishedDate = null,
                InstructorId = instructorId,
                CategoryId = categoryId
            };

            context.Courses.Add(course);
            context.SaveChanges();

            Console.WriteLine($"Course added. Generated Id = {course.Id}");
        }

        static void AddStudents()
        {
            using var context = new AppDbContext();

            var s1 = new Student { FullName = "Ali Tarek", Email = "ali.tarek@mail.com", DateOfBirth = new DateTime(2001, 4, 4) };
            var s2 = new Student { FullName = "Salma Reda", Email = "salma.reda@mail.com", DateOfBirth = new DateTime(2002, 8, 18) };
            var s3 = new Student { FullName = "Tamer Wael", Email = "tamer.wael@mail.com", DateOfBirth = new DateTime(2000, 12, 12) };

            context.Students.AddRange(s1, s2, s3);
            context.SaveChanges();

            Console.WriteLine($"Students added. Ids = {s1.Id}, {s2.Id}, {s3.Id}");
        }

        static void EnrollStudent(int studentId, int courseId)
        {
            using var context = new AppDbContext();

            var student = context.Students.Find(studentId);
            if (student == null)
            {
                Console.WriteLine($"Enrollment refused: student {studentId} does not exist.");
                return;
            }

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Enrollment refused: course {courseId} does not exist.");
                return;
            }

            if (!course.IsPublished)
            {
                Console.WriteLine($"Enrollment refused: course '{course.Title}' is not published.");
                return;
            }

            if (context.Enrollments.Find(studentId, courseId) != null)
            {
                Console.WriteLine($"Enrollment refused: '{student.FullName}' is already enrolled in '{course.Title}'.");
                return;
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                ProgressPercentage = 0,
                Status = EnrollmentStatus.Active
            };

            context.Enrollments.Add(enrollment);
            context.SaveChanges();

            Console.WriteLine($"'{student.FullName}' enrolled in '{course.Title}'.");
        }

        static void FindCourse(int id, string title)
        {
            using var context = new AppDbContext();

            var byId = context.Courses.Find(id);
            if (byId == null)
                Console.WriteLine($"Find by Id {id}: not found");
            else
                Console.WriteLine($"Find by Id {id}: {byId.Title} - {byId.Price}");

            var byTitle = context.Courses.FirstOrDefault(c => c.Title == title);
            if (byTitle == null)
                Console.WriteLine($"Find by title '{title}': not found");
            else
                Console.WriteLine($"Find by title '{title}': Id = {byTitle.Id}, {byTitle.Level}, {byTitle.Price}");
        }

        static void FilterCourses(decimal minPrice, decimal maxPrice)
        {
            using var context = new AppDbContext();

            var courses = context.Courses
                .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
                .OrderByDescending(c => c.Price)
                .Select(c => new { c.Title, c.Level, c.Price })
                .ToList();

            Console.WriteLine($"Courses priced between {minPrice} and {maxPrice}:");
            foreach (var c in courses)
                Console.WriteLine($"  {c.Title} | {c.Level} | {c.Price}");
        }

        static void PrintSubCategories(int categoryId)
        {
            using var context = new AppDbContext();

            var subCategories = context.Categories
                .Where(c => c.ParentCategoryId == categoryId)
                .ToList();

            Console.WriteLine($"Direct sub-categories of category {categoryId}:");
            if (subCategories.Count == 0)
                Console.WriteLine("  (none)");
            foreach (var c in subCategories)
                Console.WriteLine($"  {c.Id} - {c.Name}");
        }

        static void PrintTopLevelCategories()
        {
            using var context = new AppDbContext();

            var topLevel = context.Categories
                .Where(c => c.ParentCategoryId == null)
                .ToList();

            Console.WriteLine("Top-level categories:");
            foreach (var c in topLevel)
                Console.WriteLine($"  {c.Id} - {c.Name}");
        }

        static void PrintCourseRoster(int courseId)
        {
            using var context = new AppDbContext();

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Course {courseId} not found.");
                return;
            }

            // 1) student Ids from the enrollments
            var studentIds = context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId)
                .ToList();

            // 2) students with Contains
            var students = context.Students
                .Where(s => studentIds.Contains(s.Id))
                .ToList();

            Console.WriteLine($"Course '{course.Title}' has {studentIds.Count} enrolled student(s):");
            foreach (var s in students)
                Console.WriteLine($"  {s.FullName}");
        }

    
        static void ChangePrice(int courseId, decimal newPrice)
        {
            using var context = new AppDbContext();

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Course {courseId} not found.");
                return;
            }

            if (newPrice < 0)
            {
                Console.WriteLine("Invalid price: it cannot be negative.");
                return;
            }

            var oldPrice = course.Price;
            course.Price = newPrice;
            context.SaveChanges();

            Console.WriteLine($"Price of '{course.Title}' changed from {oldPrice} to {newPrice}.");
        }

        static void UpdateProgress(int studentId, int courseId, int newProgress)
        {
            using var context = new AppDbContext();

            var enrollment = context.Enrollments.Find(studentId, courseId);
            if (enrollment == null)
            {
                Console.WriteLine($"Enrollment (student {studentId}, course {courseId}) not found.");
                return;
            }

            if (newProgress < 0 || newProgress > 100)
            {
                Console.WriteLine($"Invalid progress {newProgress}: it must be between 0 and 100.");
                return;
            }

            enrollment.ProgressPercentage = newProgress;
            if (newProgress == 100)
                enrollment.Status = EnrollmentStatus.Completed;

            context.SaveChanges();

            Console.WriteLine($"Progress of enrollment (student {studentId}, course {courseId}) = {newProgress}%, status = {enrollment.Status}.");
        }

        static void PublishCourse(int courseId)
        {
            using var context = new AppDbContext();

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Course {courseId} not found.");
                return;
            }

            if (course.IsPublished)
            {
                Console.WriteLine($"Course '{course.Title}' is already published.");
                return;
            }

            course.IsPublished = true;
            course.PublishedDate = DateTime.Now;
            context.SaveChanges();

            Console.WriteLine($"Course '{course.Title}' published on {course.PublishedDate}.");
        }

        static void Unenroll(int studentId, int courseId)
        {
            using var context = new AppDbContext();

            var enrollment = context.Enrollments.Find(studentId, courseId);
            if (enrollment == null)
            {
                Console.WriteLine($"Enrollment (student {studentId}, course {courseId}) not found.");
                return;
            }

            context.Enrollments.Remove(enrollment);
            context.SaveChanges();

            Console.WriteLine($"Student {studentId} was unenrolled from course {courseId}.");
        }

        static void DeleteCategoryUnsafe(int categoryId)
        {
            using var context = new AppDbContext();

            var category = context.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine($"Category {categoryId} not found.");
                return;
            }

            try
            {
                context.Categories.Remove(category);
                context.SaveChanges();
                Console.WriteLine($"Category '{category.Name}' deleted.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("DbUpdateException: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner: " + ex.InnerException.Message);
            }
        }

        static void DeleteCategorySafe(int categoryId)
        {
            using var context = new AppDbContext();

            var category = context.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine($"Category {categoryId} not found.");
                return;
            }

            if (context.Categories.Any(c => c.ParentCategoryId == categoryId))
            {
                Console.WriteLine($"Category '{category.Name}' still has sub-categories, so it was not deleted.");
                return;
            }

            context.Categories.Remove(category);
            context.SaveChanges();

            Console.WriteLine($"Category '{category.Name}' deleted.");
        }

        static void DeleteCourse(int courseId)
        {
            using var context = new AppDbContext();

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Course {courseId} not found.");
                return;
            }

            int before = context.Enrollments.Count(e => e.CourseId == courseId);
            Console.WriteLine($"Enrollments of '{course.Title}' before delete: {before}");

            context.Courses.Remove(course);
            context.SaveChanges();

            int after = context.Enrollments.Count(e => e.CourseId == courseId);
            Console.WriteLine($"Enrollments of '{course.Title}' after delete: {after}");

 
        }

        
        static void BulkRaisePrice(CourseLevel level)
        {
            using var context = new AppDbContext();

            int affected = context.Courses
                .Where(c => c.IsPublished && c.Level == level)
                .ExecuteUpdate(s => s
                    .SetProperty(c => c.Price, c => c.Price * 1.1m));

            Console.WriteLine($"Price raised by 10% for published {level} courses. Affected rows: {affected}");
        }

        static void BulkDeleteDropped()
        {
            using var context = new AppDbContext();

            int deleted = context.Enrollments
                .Where(e => e.Status == EnrollmentStatus.Dropped)
                .ExecuteDelete();

            Console.WriteLine($"Dropped enrollments deleted. Affected rows: {deleted}");
        }

        static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine("=== " + title + " ===");
        }
    }
}
