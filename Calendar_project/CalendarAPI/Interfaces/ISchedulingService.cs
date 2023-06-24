namespace CalendarAPI.Interfaces
{
    public interface ISchedulingService
    {
        public void ScheduleNotification(string appointmentTitle, int appointmentId, DateTime notificationDateTime);
        public void UnscheduleNotification(int appointmentId);
    }
}
