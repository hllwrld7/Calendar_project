using CalendarAPI.DataTypes;
using CalendarAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI
{
    public class SqliteContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        private readonly string _dbPath = "calendar.db";

        public SqliteContext()
        {
            Database.EnsureCreated();
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            _dbPath = Path.Join(path);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite($"Data Source={_dbPath}");
    }
}
