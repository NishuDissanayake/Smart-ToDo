using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Client.Models
{
    public class ToDoItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsDone { get; set; } = false;
    }
}
