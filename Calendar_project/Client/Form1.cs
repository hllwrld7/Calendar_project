using System.Text.RegularExpressions;

namespace Client
{
    public partial class Form1 : Form
    {
        private AppointmentManagementService _appoinmentManagementService;
        private ContactManagementService _contactManagementService;
        private SettingsService _settingsService;
        private bool _panelIsInAddMode = true;
        private Dictionary<string, Appointment> _currentAppointments = new Dictionary<string, Appointment>();
        private Appointment _currentAppointment = null;
        private Contact _currentContact = null;
        private DateTime _currentDate;
        private bool _contactPanelIsInAddMode = true;
        private List<string> _currentContactNames = new();

        public Form1()
        {
            InitializeComponent();
            _appoinmentManagementService = new AppointmentManagementService();
            _contactManagementService = new ContactManagementService();
            _settingsService = new SettingsService();
            SetCurrentDate(DateTime.Now);
            PopulateButtons(DateTime.Today);
            PopulateComboBox();
            PopulateListView();
            appInfoPanel.Visible = false;
            addAppPanel.Visible = false;
            statusLabel.Visible = false;
            statusLabel.Visible = false;
            lbContactStatus.Visible = false;
            contactInfoPanel.Visible = false;
            editContactPanel.Visible = false;
            lvContacts.View = View.List;
            ClearAddAppointmentPanel();
        }

        private void PopulateListView()
        {
            var contacts = _contactManagementService.GetContacts().Result;
            _currentContactNames.Clear();
            _currentContactNames.AddRange(contacts.Select(x => x.Name));
            AddItemsToListView();
        }

        private void AddItemsToListView()
        {
            lvContacts.Items.Clear();
            var listViewItems = new List<ListViewItem>();
            foreach (var contact in _currentContactNames)
                listViewItems.Add(new ListViewItem(contact));

            lvContacts.Items.AddRange(listViewItems.ToArray());
        }

        private void PopulateComboBox()
        {
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 60; j += 30)
                {
                    cbStartDate.Items.Add(string.Format("{0:0#}", i) + ":" + string.Format("{0:0#}", j));
                    cbEndDate.Items.Add(string.Format("{0:0#}", i) + ":" + string.Format("{0:0#}", j));
                }
            }
            cbStartDate.SelectedItem = "16:30";
            cbEndDate.SelectedItem = "17:00";
        }

        private void PopulateButtons(DateTime date)
        {
            ResetButtons();
            var appointmentsForTheDay = _appoinmentManagementService.GetAppointmentsForTheDay(date);
            appointmentsForTheDay.Sort((x, y) => DateTime.Compare(x.StartDate, y.StartDate));
            for (int i = 1; i < appointmentsForTheDay.Count + 1; i++)
            {
                _currentAppointments[i.ToString()] = appointmentsForTheDay[i - 1];
                SetupButton(i, appointmentsForTheDay[i - 1]);
            }
            SetCurrentAppointment("1");
        }

        private void ResetButtons()
        {
            app1.Visible = false;
            app2.Visible = false;
            app3.Visible = false;
            app4.Visible = false;
        }

        private void SetupButton(int index, Appointment appointment)
        {
            switch (index)
            {
                case 1:
                    {
                        app1.Visible = true;
                        app1.Text = $"1) {SetTimeText(appointment)}   {appointment.Title}";
                        break;
                    }
                case 2:
                    {
                        app2.Visible = true;
                        app2.Text = $"2) {SetTimeText(appointment)}   {appointment.Title}";
                        break;
                    }
                case 3:
                    {
                        app3.Visible = true;
                        app3.Text = $"3) {SetTimeText(appointment)}   {appointment.Title}";
                        break;
                    }
                case 4:
                    {
                        app4.Visible = true;
                        app4.Text = $"4) {SetTimeText(appointment)}   {appointment.Title}";
                        break;
                    }
            }
        }

        private string SetTimeText(Appointment appointment)
        {
            if (appointment.IsAllDay)
                return $"{appointment.StartDate.Day}.{appointment.StartDate.Month}.{appointment.StartDate.Year}";
            else
                return $"{appointment.StartDate} - {appointment.EndDate}";
        }

        private void addAppointment_Click(object sender, EventArgs e)
        {
            appInfoPanel.Visible = false;
            addAppPanel.Visible = true;
            _panelIsInAddMode = true;
            ClearAddAppointmentPanel();
        }

        private void ClearAddAppointmentPanel()
        {
            tbTitle.Text = "Title";
            tbDescription.Text = "Description";
            lbDate.Text = $"{_currentDate.Day}.{_currentDate.Month}.{_currentDate.Year}";
            tbAllDayEdit.Checked = false;
            tbLocation.Text = "Location";
            tbLocation.Enabled = true;
            cbIsOnline.Checked = false;
            statusLabel.Text = "";
            statusLabel.Visible = false;
        }

        private void statusLabel_Click(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            SetCurrentDate(e.Start);
            appDate.Text = _currentDate.ToString();
            appInfoPanel.Visible = false;
            PopulateButtons(e.Start);
        }

        private void SetCurrentDate(DateTime date)
        {
            _currentDate = date;
            var shortDate = $"{date.Day}.{date.Month}.{date.Year}";
            lbDate.Text = shortDate;
            lbCurrentDate.Text = shortDate;
        }

        private void appointment_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                var bt = sender as Button;
                SetCurrentAppointment(bt.Text);
                PopulateCurrentAppointmentPanel();
                appInfoPanel.Visible = true;
                addAppPanel.Visible = false;
            }
        }

        private void SetCurrentAppointment(string text)
        {
            if (!_currentAppointments.Any())
                return;
            ColorCurrentButton(Color.Transparent);
            _currentAppointment = _currentAppointments[text.First().ToString()];
            ColorCurrentButton(Color.LightGray);
        }

        private void ColorCurrentButton(Color color)
        {
            var index = -1;
            foreach (var v in _currentAppointments)
                if (v.Value == _currentAppointment)
                    index = Convert.ToInt32(v.Key);

            switch (index)
            {
                case 1:
                    {
                        app1.BackColor = color;
                        break;
                    }
                case 2:
                    {
                        app2.BackColor = color;
                        break;
                    }
                case 3:
                    {
                        app3.BackColor = color;
                        break;
                    }
                case 4:
                    {
                        app4.BackColor = color;
                        break;
                    }
                default: break;
            }
        }

        private void PopulateCurrentAppointmentPanel()
        {
            appTitle.Text = _currentAppointment.Title;
            appDescriptionBox.Text = _currentAppointment.Description;
            appDate.Text = SetTimeText(_currentAppointment);
            tbAllDayInfo.Checked = _currentAppointment.IsAllDay;
            appLocation.Text = _currentAppointment.IsLocationOnline ? "Online" : _currentAppointment.Location;
        }

        private void editAppointment_Click(object sender, EventArgs e)
        {
            appInfoPanel.Visible = false;
            addAppPanel.Visible = true;
            _panelIsInAddMode = false;
            PopulateAddAppointmentPanel();
        }

        private void PopulateAddAppointmentPanel()
        {
            tbTitle.Text = _currentAppointment.Title;
            tbDescription.Text = _currentAppointment.Description;
            lbDate.Text = SetTimeText(_currentAppointment);
            tbAllDayEdit.Checked = _currentAppointment.IsAllDay;
            SetCbValues();
            SetEditLocation();
        }

        private void SetCbValues()
        {
            if (_currentAppointment.IsAllDay)
            {
                cbStartDate.Enabled = false;
                cbEndDate.Enabled = false;
            }
            else
            {
                cbStartDate.Enabled = true;
                cbEndDate.Enabled = true;
                cbStartDate.SelectedItem = $"{_currentAppointment.StartDate.Hour}:{_currentAppointment.StartDate.Minute}";
                cbEndDate.SelectedItem = $"{_currentAppointment.EndDate.Hour}:{_currentAppointment.EndDate.Minute}";
            }
        }

        private void SetEditLocation()
        {
            if (_currentAppointment.IsLocationOnline)
            {
                cbIsOnline.Checked = true;
                tbLocation.Enabled = false;
            }
            else
            {
                cbIsOnline.Checked = false;
                tbLocation.Enabled = true;
                tbLocation.Text = _currentAppointment.Location;
            }
        }

        private async void deleteAppointment_Click(object sender, EventArgs e)
        {
            var result = await _appoinmentManagementService.DeleteAppointment(_currentAppointment);
            statusLabel.Text = result;
            statusLabel.Visible = true;
            PopulateButtons(_currentDate);
            appInfoPanel.Visible = false;
        }

        private void discardAppointment_Click(object sender, EventArgs e)
        {
            ClearAddAppointmentPanel();
        }

        private async void confirm_Click(object sender, EventArgs e)
        {
            var isAllDay = tbAllDayEdit.Checked;
            var startDate = GetDate(isAllDay, true);
            var endDate = GetDate(isAllDay, false);

            if (!isAllDay && !DateIsValid(startDate, endDate))
            {
                statusLabel.Text = "Start date has to be earlier than end date";
                statusLabel.Visible = true;
                return;
            }
            var isOnline = cbIsOnline.Checked;
            var app = new Appointment(-1, tbTitle.Text, tbDescription.Text,
                startDate, endDate, isAllDay,
                isOnline ? "Online" : tbLocation.Text, isOnline);
            if (_panelIsInAddMode)
            {
                app.Id = _appoinmentManagementService.GetNewId();
                statusLabel.Text = await _appoinmentManagementService.AddAppointment(app);
                statusLabel.Visible = true;
            }
            else
            {
                app.Id = _currentAppointment.Id;
                statusLabel.Text = await _appoinmentManagementService.EditAppointment(app);
                statusLabel.Visible = true;
            }
            PopulateButtons(_currentDate);
        }

        private bool DateIsValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate;
        }

        private DateTime GetDate(bool isAllDay, bool isStartDate)
        {
            var result = new DateTime(2023, 1, 1);
            if (isAllDay)
            {
                if (!isStartDate)
                    return result;
                return new DateTime(_currentDate.Year, _currentDate.Month, _currentDate.Day);
            }
            else
            {
                if (isStartDate)
                    return new DateTime(_currentDate.Year, _currentDate.Month, _currentDate.Day,
                        GetTimeFromString(cbStartDate.SelectedItem.ToString(), true),
                        GetTimeFromString(cbStartDate.SelectedItem.ToString(), false), 0);
                else
                    return new DateTime(_currentDate.Year, _currentDate.Month, _currentDate.Day,
            GetTimeFromString(cbEndDate.SelectedItem.ToString(), true),
            GetTimeFromString(cbEndDate.SelectedItem.ToString(), false), 0);
            }
        }

        private int GetTimeFromString(string timeString, bool getHour)
        {
            var result = getHour ? timeString.Substring(0, 2) : timeString.Substring(3, 2);
            return Convert.ToInt16(result);
        }

        private void tbAllDayEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (tbAllDayEdit.Checked)
            {
                cbStartDate.Enabled = false;
                cbEndDate.Enabled = false;
            }
            else
            {
                cbStartDate.Enabled = true;
                cbEndDate.Enabled = true;
            }
        }

        private void btTestNotification_Click(object sender, EventArgs e)
        {
            _settingsService.ScheduleNotification();
        }

        private void btFireNotification_Click(object sender, EventArgs e)
        {
            _settingsService.FireNotification();
        }

        private void tbSearchBox_TextChanged(object sender, EventArgs e)
        {
            Regex regex = new Regex(tbSearchBox.Text);
            _currentContactNames = _currentContactNames.OrderBy(x => regex.IsMatch(x)).Reverse().ToList();
            AddItemsToListView();
        }

        private void lvContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ListView)
            {
                var lv = sender as ListView;
                if (lv.SelectedItems.Count == 0)
                    return;
                _currentContact = _contactManagementService.GetContactByName(lv.SelectedItems[0].Text);
                SetContactInfoPanel(_currentContact.Name);
            }
        }

        private void SetContactInfoPanel(string contactName)
        {
            contactInfoPanel.Visible = true;
            editContactPanel.Visible = false;
            var contactInfo = _contactManagementService.GetContactByName(contactName);
            lbName.Text = contactInfo.Name;
            lbPhone.Text = contactInfo.PhoneNumber;
            lbEmail.Text = contactInfo.Email;
        }

        private async void btDeleteContact_Click(object sender, EventArgs e)
        {
            if (lvContacts.SelectedItems.Count == 0) return;

            lbContactStatus.Visible = true;
            lbContactStatus.Text = await _contactManagementService.DeleteContact(_currentContact);
            PopulateListView();
            contactInfoPanel.Visible = false;
        }

        private void btEditContact_Click(object sender, EventArgs e)
        {
            if (_currentContact == null) return;
            _contactPanelIsInAddMode = false;
            SetContactEditPanel(_currentContact.Name);
        }

        private void SetContactEditPanel(string contactName = "")
        {
            contactInfoPanel.Visible = false;
            editContactPanel.Visible = true;
            var contactInfo = _contactManagementService.GetContactByName(contactName);
            tbName.Text = contactInfo.Name;
            tbPhone.Text = contactInfo.PhoneNumber;
            tbEmail.Text = contactInfo.Email;
        }

        private void btDiscardContact_Click(object sender, EventArgs e)
        {
            ResetContactEditPanel();
        }

        private void ResetContactEditPanel()
        {
            lbContactStatus.Visible = false;
            tbName.Text = "Name";
            tbPhone.Text = "Phone";
            tbEmail.Text = "Email";
        }

        private async void btConfirmContact_Click(object sender, EventArgs e)
        {
            lbContactStatus.Visible = true;
            var contact = new Contact(tbName.Text, tbPhone.Text, tbEmail.Text);
            if (_contactPanelIsInAddMode)
            {
                contact.Id = _contactManagementService.GetNewId();
                lbContactStatus.Text = await _contactManagementService.AddContact(
                    contact);
            }
            else
            {
                contact.Id = _currentContact.Id;
                lbContactStatus.Text = await _contactManagementService.EditContact(contact);
            }

            PopulateListView();
        }

        private void btAddContact_Click(object sender, EventArgs e)
        {
            _contactPanelIsInAddMode = true;
            SetContactEditPanel();
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            lbContactStatus.Visible = true;
            lbContactStatus.Text = _contactManagementService.ExportContacts();
        }

        private async void btImport_Click(object sender, EventArgs e)
        {
            lbContactStatus.Visible = true;
            lbContactStatus.Text = await _contactManagementService.ImportContacts();
            PopulateListView();
        }

        private void cbIsOnline_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsOnline.Checked)
                tbLocation.Enabled = false;
            else
                tbLocation.Enabled = true;
        }
    }
}