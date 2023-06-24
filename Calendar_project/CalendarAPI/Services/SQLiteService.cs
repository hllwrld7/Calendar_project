using CalendarAPI.DataTypes;
using CalendarAPI.Interfaces;
using Common;
using System.Data.SQLite;

namespace CalendarAPI.Services
{
    public class SQLiteService: ISQLiteService, IDisposable
    {
        private static SQLiteConnection _dbConnection = new("Data Source=CalendarDb.sqlite;");

        public SQLiteService() 
        {
            if (_dbConnection == null)
                SQLiteConnection.CreateFile("CalendarDb.sqlite");
            _dbConnection?.Open();
            PrepareTables();
        }

        public List<Appointment> GetAppointments()
        {
            var query = "SELECT * FROM appointments;";
            var command = new SQLiteCommand(query, _dbConnection);
            var result = new List<Appointment>();
            using (var reader = command.ExecuteReader())
            {
                if (reader == null)
                    return result;

                while (reader.Read())
                {
                    result.Add(new Appointment(Convert.ToInt16(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), ParseBoolValue(reader[5]), reader[6].ToString(), ParseBoolValue(reader[7])));
                }
                reader.Close();
            };

            return result;
        }

        public void WriteAppointment(Appointment appointment)
        {
            var query = $"INSERT INTO appointments (id, title, description, startDate, endDate, isAllDay, location, isLocationOnline)" +
                $" values ({appointment.Id}, '{appointment.Title}', '{appointment.Description}', '{appointment.StartDate}', '{appointment.EndDate}', {appointment.IsAllDay}, '{appointment.Location}', {appointment.IsLocationOnline});";
            var command = new SQLiteCommand(query, _dbConnection);
            command.ExecuteNonQuery();
        }

        public void RemoveAppointment(Appointment appointment) 
        {
            var query = $"DELETE FROM appointments WHERE id='{appointment.Id}';";
            var command = new SQLiteCommand(query, _dbConnection);
            command.ExecuteNonQuery();
        }

        private void PrepareTables()
        {
            var query = "CREATE TABLE IF NOT EXISTS appointments (id NUMBER, title TEXT, description TEXT, startDate TEXT, endDate TEXT, isAllDay BOOL, location TEXT, isLocationOnline BOOL);" +
                    "CREATE TABLE IF NOT EXISTS contacts (name TEXT, email TEXT, phoneNumber TEXT);";
            SQLiteCommand command = new SQLiteCommand(query, _dbConnection);
            command.ExecuteNonQuery();
        }

        private bool ParseBoolValue(object value)
        {
            return value.ToString() == "True";
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}
