using System.ComponentModel.DataAnnotations;

namespace FreelanceBot.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Username { get; set; }

        [Required]
        public long ChatId { get; set; }
        public int MaxResumes { get; set; } = 5;
        public int MaxJobs { get; set; } = 5;
        public int MaxEvents { get; set; } = 5; 

        public Role Role { get; set; }
        
        public Stage Stage { get; set; }

    }
}
