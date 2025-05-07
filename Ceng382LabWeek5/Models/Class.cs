using System.ComponentModel.DataAnnotations;

namespace Ceng382LabWeek5.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Class name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Student count is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Student count must be greater than 0.")]
        public int PersonCount { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
