namespace Common
{
    public interface IAppointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsAllDay { get; set; }
        public string Location { get; set; }
        public bool IsLocationOnline { get; set; }
    }
}