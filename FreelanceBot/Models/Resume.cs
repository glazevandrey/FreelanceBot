using System.ComponentModel.DataAnnotations;

namespace FreelanceBot.Models
{
    public class Resume
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }


        public int PayMin { get; set; }
        public int PayMax { get; set; }
        public int Pay { get; set; }

        public string Place { get; set; }
        public bool HaveFile { get; set; }
        public string FileName { get; set; }
        public bool IsDone { get; set; }
    }
}
