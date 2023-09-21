using System.ComponentModel.DataAnnotations;

namespace dotnetlearningclass.Entities
{
    public class Students
    {
        [Key]
        public int? StudentId { get; set; }
        public string? StudentName { get; set; }
        public int? StudentClassId { get; set; }
    }
}
