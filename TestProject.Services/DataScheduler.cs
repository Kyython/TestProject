using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject.Services
{
    public class DataScheduler : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DataScheduler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleResponseAdd();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task ScheduleResponseAdd()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = new JobFactory(_serviceProvider);
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<JobService>()
                                            .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                             .WithIdentity("addResponseDataTrigger", "group1")
                                             .StartNow()
                                             .WithSimpleSchedule(x => x
                                             .WithIntervalInSeconds(5)
                                             .RepeatForever())
                                             .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
