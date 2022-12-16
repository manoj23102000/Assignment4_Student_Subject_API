using Microsoft.EntityFrameworkCore;
using TallyAssignment4.Models;

namespace TallyAssignment4.Data
{
    public class StudentDBContext:DbContext
    {
        public StudentDBContext(DbContextOptions<StudentDBContext> options):base(options)
        {
        }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
