using System.ComponentModel.DataAnnotations;

namespace FreelanceBot.Models
{
    public class Event
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HaveFile { get; set; }
        public string FileName { get; set; }
        public string StartDate { get; set; }
        public bool IsDone { get; set; }
    }
}
