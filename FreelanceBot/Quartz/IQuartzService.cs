using System.Threading.Tasks;

namespace FreelanceBot.Quartz
{
    public interface IQuartzService
    {
        Task ParseJob();
    }
}
