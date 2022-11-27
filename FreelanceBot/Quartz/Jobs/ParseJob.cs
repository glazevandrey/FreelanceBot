using Quartz;
using System.Threading.Tasks;

namespace FreelanceBot.Quartz.Jobs
{
    public class ParseJob : IJob
    {
        private readonly IQuartzService _quartzService;

        public ParseJob(IQuartzService quartzService)
        {
            _quartzService = quartzService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _quartzService.ParseJob();
        }
    }
}
