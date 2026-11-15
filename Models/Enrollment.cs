using C48_G03_EF03.Enums;

namespace C48_G03_EF03.Models
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public int ProgressPercentage { get; set; }
        public EnrollmentStatus Status { get; set; }
        public decimal? FinalGrade { get; set; }
    }
}
