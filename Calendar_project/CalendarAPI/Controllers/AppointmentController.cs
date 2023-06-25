using CalendarAPI.DataTypes;
using CalendarAPI.Interfaces;
using CalendarAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalendarAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentManagementService _appointmentManagementService;

        public AppointmentController(IAppointmentManagementService appointmentManagementService)
        {
            _appointmentManagementService = appointmentManagementService;
        }

        [HttpGet(Name = "GetAppointments")]
        public IEnumerable<Appointment> Get()
        {
            return _appointmentManagementService.Appointments;
        }

        [HttpPost(Name = "PostAppointments")]
        public string Post(string operation, [FromBody] Appointment appointment)
        {
            var response = String.Empty;
            if(operation == "add")
                _appointmentManagementService.AddAppointment(appointment, out response);

            if (operation == "edit")
                _appointmentManagementService.EditAppointment(appointment, out response);

            if (operation == "delete")
                _appointmentManagementService.DeleteAppointment(appointment, out response);

            return response;
        }
    }
}