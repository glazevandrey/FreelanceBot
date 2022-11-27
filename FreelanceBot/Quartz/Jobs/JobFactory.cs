﻿using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace FreelanceBot.Quartz.Jobs
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;


        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJob job;
            try
            {
                // смотрим какой триггер сработал
                job = bundle.JobDetail.Key.Name switch
                {
                    nameof(ParseJob) =>
                        (IJob)_serviceProvider.GetService<ParseJob>(),
                    _ => throw new System.Exception($"Service {bundle.JobDetail.Key.Name} not found")
                };
            }
            catch (Exception e)
            {
                throw e;
            }

            return job;
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
