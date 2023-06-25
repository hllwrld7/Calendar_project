using CalendarAPI.DataTypes;
using CalendarAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Toolkit.Uwp.Notifications;

namespace CalendarAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ISchedulingService _schedulingService;
        public SettingsController(ISchedulingService schedulingService)
        {
            _schedulingService = schedulingService;
        }

        [HttpPost(Name = "SendNotification")]
        public void Post(string operation)
        {
            if(operation == "schedule")
                _schedulingService.ScheduleNotification(new Appointment(), DateTime.UtcNow.AddMinutes(1));
            if (operation == "fire")
            {
                new ToastContentBuilder()
                .AddText("HELLO")
                .Show();
            }
        }
    }
}