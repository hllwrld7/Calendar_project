using CalendarAPI.DataTypes;
using CalendarAPI.Interfaces;
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

        public List<DataTypes.Appointment> GetAppointments()
        {
            var query = "SELECT * FROM appointments;";
            var command = new SQLiteCommand(query, _dbConnection);
            var result = new List<DataTypes.Appointment>();
            using (var reader = command.ExecuteReader())
            {
                if (reader == null)
                    return result;

                while (reader.Read())
                {
                    result.Add(new DataTypes.Appointment(Convert.ToInt16(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), ParseBoolValue(reader[5]), reader[6].ToString(), ParseBoolValue(reader[7])));
                }
                reader.Close();
            };

            return result;
        }

        public void WriteAppointment(DataTypes.Appointment appointment)
        {
            var query = $"INSERT INTO appointments (id, title, description, startDate, endDate, isAllDay, location, isLocationOnline)" +
                $" values ({appointment.Id}, '{appointment.Title}', '{appointment.Description}', '{appointment.StartDate}', '{appointment.EndDate}', {appointment.IsAllDay}, '{appointment.Location}', {appointment.IsLocationOnline});";
            var command = new SQLiteCommand(query, _dbConnection);
            command.ExecuteNonQuery();
        }

        public void RemoveAppointment(DataTypes.Appointment appointment) 
        {
            var query = $"DELETE FROM appointments WHERE id='{appointment.Id}';";
            var command = new SQLiteCommand(query, _dbConnection);
            command.ExecuteNonQuery();
        }

        private void PrepareTables()
        {
            var query = "CREATE TABLE IF NOT EXISTS appointments (id NUMBER, title TEXT, description TEXT, startDate TEXT, endDate TEXT, isAllDay BOOL, location TEXT, isLocationOnline BOOL);" +
                    "CREATE TABLE IF NOT EXISTS contacts (id NUMBER, name TEXT, phoneNumber TEXT, email TEXT);";
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

        public List<Contact> GetContacts()
        {
            var query = "SELECT * FROM contacts;";
            var command = new SQLiteCommand(query, _dbConnection);
            var result = new List<Contact>();
            using (var reader = command.ExecuteReader())
            {
                if (reader == null)
                    return result;

                while (reader.Read())
                {
                    result.Add(new Contact(Convert.ToUInt16(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString()));
                }
                reader.Close();
            };
            return result;
        }

        public void WriteContact(Contact contact)
        {
            var query = $"INSERT INTO contacts (id, name, phoneNumber, email)" +
            $" values ({contact.Id}, '{contact.Name}', '{contact.PhoneNumber}', '{contact.Email}');";
            var command = new SQLiteCommand(query, _dbConnection);
            command.ExecuteNonQuery();
        }

        public void RemoveContact(Contact contact)
        {
            var query = $"DELETE FROM contacts WHERE id='{contact.Id}';";
            var command = new SQLiteCommand(query, _dbConnection);
            command.ExecuteNonQuery();
        }

        public void UpdateAppointment(Appointment appointment)
        {
            var query = $"UPDATE appointments SET title='{appointment.Title}', description='{appointment.Description}', " +
                $"startDate='{appointment.StartDate}', endDate='{appointment.EndDate}', isAllDay={appointment.IsAllDay}, " +
                $"location='{appointment.Location}', isLocationOnline={appointment.IsLocationOnline} " +
                $"WHERE id={appointment.Id};";
            var command = new SQLiteCommand(query, _dbConnection);
            command.ExecuteNonQuery();
        }

        public void UpdateContact(Contact contact)
        {
            var query = $"UPDATE contacts SET name='{contact.Name}', phoneNumber='{contact.PhoneNumber}'," +
                $"email='{contact.Email}' WHERE id={contact.Id};";
            var command = new SQLiteCommand(query, _dbConnection);
            command.ExecuteNonQuery();
        }
    }
}
