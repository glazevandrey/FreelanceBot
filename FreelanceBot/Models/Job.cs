using System.ComponentModel.DataAnnotations;

namespace FreelanceBot.Models
{
    public class Job
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }
        public string Contact{ get; set; }


        public string Title { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }


        public int PayMin { get; set; }
        public int PayMax { get; set; }
        public int Pay { get; set; }

        public string Place { get; set; }
        public string TypeJob { get; set; }

        public bool IsDone { get; set; }
    }
}
