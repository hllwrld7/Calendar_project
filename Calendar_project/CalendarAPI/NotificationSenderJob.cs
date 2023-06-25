using Microsoft.Toolkit.Uwp.Notifications;
using Quartz;

namespace CalendarAPI
{
    public class NotificationSenderJob : IJob
    {
        public NotificationSenderJob() { }
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            var appTitle = dataMap["Title"];
            var appLocation = dataMap["Location"];
            var appStart = dataMap["StartTime"];
            new ToastContentBuilder()
            .AddText(appTitle.ToString())
            .AddText(appLocation.ToString())
            .AddText(appStart.ToString())
            .Show();
            return Task.CompletedTask;
        }
    }
}
