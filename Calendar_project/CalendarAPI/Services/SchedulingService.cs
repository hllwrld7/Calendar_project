using CalendarAPI.Interfaces;
using Quartz.Impl;
using Quartz;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Quartz.Spi;

namespace CalendarAPI.Services
{
    public class SchedulingService : ISchedulingService, IDisposable
    {
        private IScheduler _scheduler;

        public SchedulingService()
        {
            SetScheduler();
            _scheduler.Start();
        }

        private async Task SetScheduler()
        {
            StdSchedulerFactory factory = new();
            _scheduler = await factory.GetScheduler();
        }

        public void ScheduleNotification(string appointmentTitle, int appointmentId, DateTime notificationDateTime)
        {
            IDictionary<string, object> jobData = new Dictionary<string, object> { { "AppointmentTitle", appointmentTitle } };
            IJobDetail job = JobBuilder.Create<NotificationSenderJob>()
            .WithIdentity(appointmentId.ToString()).UsingJobData(new Quartz.JobDataMap(jobData))
            .Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity(appointmentId.ToString())
            .StartAt((DateTimeOffset)notificationDateTime)
            .UsingJobData(new Quartz.JobDataMap(jobData))
            .Build();

            _scheduler
                .ScheduleJob(job, trigger);
        }

        public void UnscheduleNotification(int appointmentId)
        {
            var triggerKey = new TriggerKey(appointmentId.ToString());
            if (_scheduler.CheckExists(triggerKey).Result)
                _scheduler.UnscheduleJob(triggerKey);
        }

        public void Dispose()
        {
            _scheduler.Shutdown();
        }
    }
}
