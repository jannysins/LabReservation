using LabReservation.controller;
using LabReservation.model;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Linq;

namespace LabReservation
{
    public partial class Form1 : Form
    {
        private controller.ReservationController reservationController = new controller.ReservationController();
        
        private string currentUpdatingName = "";


        public Form1()
        {
            InitializeComponent();
            // zxccasc

        }

        private string ShowInputDialog(string text)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Search Reservation",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text, Width = 350 };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
            Button confirmation = new Button() { Text = "Search", Left = 260, Top = 80, Width = 100, DialogResult = DialogResult.OK };

            prompt.Controls.Add(textBox); prompt.Controls.Add(confirmation); prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            reservationController.AutoCleanUp();

            date.MinDate = DateTime.Today;
            date.MaxDate = DateTime.Today;
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void nameTxtBox_TextChanged(object sender, EventArgs e)
        {

        }


        private void saveButton_Click_1(object sender, EventArgs e)
        {
            {
                Reservation newReservation = new Reservation
                {
                    LabRoom = labsRoom.Text,
                    Date = date.Value,
                    StartTime = startTime.Text,
                    EndTime = endTime.Text,
                    Name = nameTxtBox.Text
                };

                ReservationController controller = new ReservationController();

                bool isSaved = controller.SaveReservation(newReservation);

                if (isSaved)
                {
                    MessageBox.Show("Reservation saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (currentUpdatingName == "")
            {
                string searchName = ShowInputDialog("Enter the Name of the reservation you want to update:");

                if (string.IsNullOrWhiteSpace(searchName)) return; 

                //Search the database
                Reservation foundRes = reservationController.GetReservationForUpdate(searchName);

                if (foundRes != null)
                {
                    labsRoom.Text = foundRes.LabRoom;
                    date.Value = foundRes.Date;
                    startTime.Text = foundRes.StartTime;
                    endTime.Text = foundRes.EndTime;
                    nameTxtBox.Text = foundRes.Name;

                    currentUpdatingName = foundRes.Name;
                    updateButton.Text = "Save Changes"; 
                    updateButton.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    MessageBox.Show("No reservation found for that name.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                Reservation updatedReservation = new Reservation
                {
                    LabRoom = labsRoom.Text,
                    Date = date.Value,
                    StartTime = startTime.Text,
                    EndTime = endTime.Text,
                    Name = nameTxtBox.Text
                };

                // Send to controller to update
                bool isUpdated = reservationController.UpdateReservation(currentUpdatingName, updatedReservation);

                if (isUpdated)
                {
                    MessageBox.Show("Reservation updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    currentUpdatingName = "";
                    updateButton.Text = "Update Reservation";
                    updateButton.BackColor = SystemColors.Control;

                    ClearFields();

                    labsRoom.SelectedIndex = -1;
                    nameTxtBox.Clear();
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            string searchName = ShowInputDialog("Enter the Name of the reservation you want to delete:");

            if (string.IsNullOrWhiteSpace(searchName)) return;

            // Check if the reservation actually exists first
            Reservation foundRes = reservationController.GetReservationForUpdate(searchName);

            if (foundRes != null)
            {
                DialogResult dialogResult = MessageBox.Show(
                    $"Are you sure you want to permanently delete the reservation for {foundRes.Name} in {foundRes.LabRoom}?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dialogResult == DialogResult.Yes)
                {
                    bool isDeleted = reservationController.DeleteReservation(searchName);

                    if (isDeleted)
                    {
                        MessageBox.Show("Reservation deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ClearFields();
                    }
                }
            }
            else
            {
                MessageBox.Show("No reservation found for that name.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void labsRoom_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }


        private void startTime_ValueChanged_1(object sender, EventArgs e)
        {
            startTime.ValueChanged -= startTime_ValueChanged_1;

            DateTime time = startTime.Value;
            int minute = time.Minute;

            if (minute != 0 && minute != 30)
            {
                if (minute < 15) minute = 0;
                else if (minute < 45) minute = 30;
                else
                {
                    minute = 0;
                    time = time.AddHours(1);
                }

                startTime.Value = new DateTime(time.Year, time.Month, time.Day, time.Hour, minute, 0);
            }

            endTime.Value = startTime.Value.AddHours(1);

            startTime.ValueChanged += startTime_ValueChanged_1;
        }

        private void endTime_ValueChanged(object sender, EventArgs e)
        {
            endTime.ValueChanged -= endTime_ValueChanged;

            DateTime time = endTime.Value;
            int minute = time.Minute;

            if (minute != 0 && minute != 30)
            {
                if (minute < 15) minute = 0;
                else if (minute < 45) minute = 30;
                else
                {
                    minute = 0;
                    time = time.AddHours(1);
                }

                endTime.Value = new DateTime(time.Year, time.Month, time.Day, time.Hour, minute, 0);
            }

            endTime.ValueChanged += endTime_ValueChanged;
        }

        private void nameTxtBox_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void date_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void ClearFields()
        {
            labsRoom.SelectedIndex = -1;

            startTime.Text = "00:00:00";
            endTime.Text = "00:00:00";

            date.Format = DateTimePickerFormat.Custom;
            date.CustomFormat = " "; 

            nameTxtBox.Clear();
        }

        private void date_CloseUp(object sender, EventArgs e)
        {
            date.Format = DateTimePickerFormat.Long;
        }

        private void showReservations_Click(object sender, EventArgs e)
        {
            // Get the data and plug it into the DataGridView
            System.Data.DataTable data = reservationController.GetAllReservations();
            dataGridView1.DataSource = data;

            if (dataGridView1.Columns.Contains("id"))
            {
                dataGridView1.Columns["id"].Visible = false;
            }

            if (dataGridView1.Columns.Contains("lab_room"))
                dataGridView1.Columns["lab_room"].HeaderText = "Lab Room";

            if (dataGridView1.Columns.Contains("reservation_date"))
                dataGridView1.Columns["reservation_date"].HeaderText = "Date";

            if (dataGridView1.Columns.Contains("start_time"))
                dataGridView1.Columns["start_time"].HeaderText = "Start Time";

            if (dataGridView1.Columns.Contains("end_time"))
                dataGridView1.Columns["end_time"].HeaderText = "End Time";

            if (dataGridView1.Columns.Contains("reserver_name"))
                dataGridView1.Columns["reserver_name"].HeaderText = "Name";

            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns["id"].Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
    }
}

