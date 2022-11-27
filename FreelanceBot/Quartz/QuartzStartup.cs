using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using System;
using FreelanceBot.Quartz.Jobs;

namespace FreelanceBot.Quartz
{
    public class QuartzStartup
    {
        private IScheduler _scheduler; // После запуска и до завершения выключения ссылается на объект планировщика
        private readonly IServiceProvider _container;

        public QuartzStartup(IServiceProvider container)
        {
            _container = container;
        }
        public static async void Start(IServiceProvider container, IConfiguration configuration)
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = new JobFactory(container);
            await scheduler.Start();

            // Работа по повторной отправке аутентификационных данных
            var job = JobBuilder.Create<ParseJob>()
                .WithIdentity("ParseJob", "group1")
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("ParseJobTrigger", "group1")
                .StartAt(DateTimeOffset.Now.AddSeconds(5))
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(20)
                    .RepeatForever())
                .Build();
            await scheduler.ScheduleJob(job, trigger);


        }
        public void Stop()
        {
            if (_scheduler == null)
            {
                return;
            }

            // give running jobs 30 sec (for example) to stop gracefully
            if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
            {
                _scheduler = null;
            }
            else
            {
                // jobs didn't exit in timely fashion - log a warning...
            }
        }
    }
}
