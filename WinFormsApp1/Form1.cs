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
        // This will track if we are currently updating someone
        private string currentUpdatingName = "";


        public Form1()
        {
            InitializeComponent();


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
            // This runs silently in the background every time you run the app!
            reservationController.AutoCleanUp();

            // (If you have other code here to load data into your DataGrid, keep it below this)
            // NEW FEATURE: Lock the calendar to TODAY only!
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
                // 1. Gather data from the UI and put it in the Model
                Reservation newReservation = new Reservation
                {
                    // CHANGE THESE variable names to match your actual Form control names!
                    LabRoom = labsRoom.Text,
                    Date = date.Value,
                    StartTime = startTime.Text,
                    EndTime = endTime.Text,
                    Name = nameTxtBox.Text
                };

                // 1. Create the object reference (build it using "new")
                ReservationController controller = new ReservationController();

                // 2. Use that specific object to call the method! 
                bool isSaved = controller.SaveReservation(newReservation);

                // 3. Show feedback to the user
                if (isSaved)
                {
                    MessageBox.Show("Reservation saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    // Optional: You can create a method here to clear the textboxes after saving
                }
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            // PHASE 1: SEARCH MODE
            if (currentUpdatingName == "")
            {
                // 1. Ask the user for the name using our custom pop-up
                string searchName = ShowInputDialog("Enter the Name of the reservation you want to update:");

                if (string.IsNullOrWhiteSpace(searchName)) return; // User canceled or typed nothing

                // 2. Search the database
                Reservation foundRes = reservationController.GetReservationForUpdate(searchName);

                if (foundRes != null)
                {
                    // 3. Populate the textboxes with the found data
                    labsRoom.Text = foundRes.LabRoom;
                    date.Value = foundRes.Date;
                    startTime.Text = foundRes.StartTime;
                    endTime.Text = foundRes.EndTime;
                    nameTxtBox.Text = foundRes.Name;

                    // 4. Set the tracker and change button visual
                    currentUpdatingName = foundRes.Name;
                    updateButton.Text = "Save Changes"; // The button transforms!
                    updateButton.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    MessageBox.Show("No reservation found for that name.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            // PHASE 2: SAVE MODE
            else
            {
                // 1. Gather the edited data from the UI
                Reservation updatedReservation = new Reservation
                {
                    LabRoom = labsRoom.Text,
                    Date = date.Value,
                    StartTime = startTime.Text,
                    EndTime = endTime.Text,
                    Name = nameTxtBox.Text
                };

                // 2. Send to controller to update
                bool isUpdated = reservationController.UpdateReservation(currentUpdatingName, updatedReservation);

                if (isUpdated)
                {
                    MessageBox.Show("Reservation updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 3. Reset everything back to normal
                    currentUpdatingName = "";
                    updateButton.Text = "Update Reservation";
                    updateButton.BackColor = SystemColors.Control; // Reset color

                    ClearFields();

                    // Optional: Clear the textboxes here so it's clean for the next user
                    labsRoom.SelectedIndex = -1;
                    nameTxtBox.Clear();
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            // 1. Ask the user for the name using our custom pop-up
            string searchName = ShowInputDialog("Enter the Name of the reservation you want to delete:");

            // If they click cancel or leave it blank, do nothing
            if (string.IsNullOrWhiteSpace(searchName)) return;

            // 2. Check if the reservation actually exists first (reusing your Update search feature!)
            Reservation foundRes = reservationController.GetReservationForUpdate(searchName);

            if (foundRes != null)
            {
                // 3. Ask for confirmation before deleting (Standard programming practice!)
                DialogResult dialogResult = MessageBox.Show(
                    $"Are you sure you want to permanently delete the reservation for {foundRes.Name} in {foundRes.LabRoom}?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dialogResult == DialogResult.Yes)
                {
                    // 4. Send to controller to delete
                    bool isDeleted = reservationController.DeleteReservation(searchName);

                    if (isDeleted)
                    {
                        MessageBox.Show("Reservation deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 5. Instantly clear the screen!
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
            // 1. We detach the event first to prevent an infinite loop of updating
            startTime.ValueChanged -= startTime_ValueChanged_1;

            DateTime time = startTime.Value;
            int minute = time.Minute;

            // 2. Check if the minutes are not strictly 00 or 30
            if (minute != 0 && minute != 30)
            {
                // Round to the nearest 30 minutes
                if (minute < 15) minute = 0;
                else if (minute < 45) minute = 30;
                else
                {
                    minute = 0;
                    time = time.AddHours(1); // Roll over to the next hour
                }

                // Apply the rounded time back to the Start Time box
                startTime.Value = new DateTime(time.Year, time.Month, time.Day, time.Hour, minute, 0);
            }

            // 3. MAGIC: Automatically set the End Time to exactly 1 hour later!
            endTime.Value = startTime.Value.AddHours(1);

            // 4. Re-attach the event so it works the next time they click
            startTime.ValueChanged += startTime_ValueChanged_1;
        }

        private void endTime_ValueChanged(object sender, EventArgs e)
        {
            // 1. Detach event
            endTime.ValueChanged -= endTime_ValueChanged;

            DateTime time = endTime.Value;
            int minute = time.Minute;

            // 2. Force 30-minute increments
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

            // 3. Re-attach event
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
            // 1. Reset Lab Room
            labsRoom.SelectedIndex = -1;

            // 2. Set Times to 00:00:00 instead of completely blank
            startTime.Text = "00:00:00";
            endTime.Text = "00:00:00";

            // 3. The "Empty Date" Trick: Change the format to a custom blank space
            date.Format = DateTimePickerFormat.Custom;
            date.CustomFormat = " "; // Notice the space inside the quotes!

            // 4. Clear Name
            nameTxtBox.Clear();
        }

        private void date_CloseUp(object sender, EventArgs e)
        {
            // This fires every time the calendar closes, even if they pick the exact same date!
            date.Format = DateTimePickerFormat.Long;
        }

        private void showReservations_Click(object sender, EventArgs e)
        {
            // 1. Get the data and plug it into the DataGridView
            System.Data.DataTable data = reservationController.GetAllReservations();
            dataGridView1.DataSource = data;

            // 2. Hide the ID column (optional, but usually recommended)
            if (dataGridView1.Columns.Contains("id"))
            {
                dataGridView1.Columns["id"].Visible = false;
            }

            // 3. CHANGE THE COLUMN HEADERS HERE
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

            // Hide the blank gray row header on the far left
            dataGridView1.RowHeadersVisible = false;

            // Hide the ID column
            dataGridView1.Columns["id"].Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
    }
}

