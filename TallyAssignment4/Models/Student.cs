using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallyAssignment4.Models
{
    public partial class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Class { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}
