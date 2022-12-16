using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallyAssignment4.Models
{
    public partial class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubId { get; set; }
        public string SubName { get; set; }
        public int MaxMarks { get; set; }
        public int MarksObtained { get; set; }
        public int StudentStudId { get; set; }
    }
}
