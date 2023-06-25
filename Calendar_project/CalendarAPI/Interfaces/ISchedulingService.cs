using CalendarAPI.DataTypes;

namespace CalendarAPI.Interfaces
{
    public interface ISchedulingService
    {
        public void ScheduleNotification(Appointment appointment, DateTime notificationDateTime);
        public void UnscheduleNotification(int appointmentId);
    }
}
