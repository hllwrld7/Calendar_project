using CalendarAPI.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;
using Quartz;
using System.Data.Entity.Validation;

namespace CalendarAPI
{
    public class NotificationSenderJob : IJob
    {
        public NotificationSenderJob() { }
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            var appTitle = dataMap["AppointmentTitle"];
            new ToastContentBuilder()
            .AddText(appTitle.ToString())
            .Show();
            return Task.CompletedTask;
        }
    }
}
