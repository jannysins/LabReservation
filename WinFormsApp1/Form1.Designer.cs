namespace LabReservation
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            saveButton = new Button();
            labsRoom = new ComboBox();
            date = new DateTimePicker();
            groupBox1 = new GroupBox();
            deleteButton = new Button();
            updateButton = new Button();
            textBox6 = new TextBox();
            textBox5 = new TextBox();
            textBox4 = new TextBox();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            nameTxtBox = new TextBox();
            endTime = new DateTimePicker();
            startTime = new DateTimePicker();
            showReservations = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            dataGridView1 = new DataGridView();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // saveButton
            // 
            saveButton.AccessibleName = "";
            saveButton.Location = new Point(79, 167);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(136, 28);
            saveButton.TabIndex = 0;
            saveButton.Text = "Save Reservation";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click_1;
            // 
            // labsRoom
            // 
            labsRoom.FormattingEnabled = true;
            labsRoom.Items.AddRange(new object[] { "Lab 1", "Lab 2", "Lab 3" });
            labsRoom.Location = new Point(79, 22);
            labsRoom.Name = "labsRoom";
            labsRoom.Size = new Size(217, 23);
            labsRoom.TabIndex = 2;
            labsRoom.SelectedIndexChanged += labsRoom_SelectedIndexChanged_1;
            // 
            // date
            // 
            date.Location = new Point(79, 51);
            date.Name = "date";
            date.Size = new Size(217, 23);
            date.TabIndex = 3;
            date.CloseUp += date_CloseUp;
            date.ValueChanged += date_ValueChanged_1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(deleteButton);
            groupBox1.Controls.Add(updateButton);
            groupBox1.Controls.Add(textBox6);
            groupBox1.Controls.Add(saveButton);
            groupBox1.Controls.Add(textBox5);
            groupBox1.Controls.Add(textBox4);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(nameTxtBox);
            groupBox1.Controls.Add(endTime);
            groupBox1.Controls.Add(startTime);
            groupBox1.Controls.Add(date);
            groupBox1.Controls.Add(labsRoom);
            groupBox1.Location = new Point(26, 25);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(344, 290);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(79, 235);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(136, 28);
            deleteButton.TabIndex = 13;
            deleteButton.Text = "Delete Reservation";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // updateButton
            // 
            updateButton.Location = new Point(79, 201);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(136, 28);
            updateButton.TabIndex = 12;
            updateButton.Text = "Update Reservation";
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Click += updateButton_Click;
            // 
            // textBox6
            // 
            textBox6.BorderStyle = BorderStyle.None;
            textBox6.Location = new Point(6, 138);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(67, 16);
            textBox6.TabIndex = 11;
            textBox6.Text = "Name";
            // 
            // textBox5
            // 
            textBox5.BorderStyle = BorderStyle.None;
            textBox5.Location = new Point(6, 109);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(67, 16);
            textBox5.TabIndex = 10;
            textBox5.Text = "End Time";
            // 
            // textBox4
            // 
            textBox4.BorderStyle = BorderStyle.None;
            textBox4.Location = new Point(6, 80);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(67, 16);
            textBox4.TabIndex = 9;
            textBox4.Text = "Start Time";
            // 
            // textBox3
            // 
            textBox3.BorderStyle = BorderStyle.None;
            textBox3.Location = new Point(6, 51);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(67, 16);
            textBox3.TabIndex = 8;
            textBox3.Text = "Date";
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Location = new Point(6, 22);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(67, 16);
            textBox2.TabIndex = 7;
            textBox2.Text = "Lab Rooms";
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // nameTxtBox
            // 
            nameTxtBox.Location = new Point(79, 138);
            nameTxtBox.Name = "nameTxtBox";
            nameTxtBox.Size = new Size(217, 23);
            nameTxtBox.TabIndex = 6;
            nameTxtBox.TextChanged += nameTxtBox_TextChanged_1;
            // 
            // endTime
            // 
            endTime.CustomFormat = "hh:mm tt";
            endTime.Format = DateTimePickerFormat.Custom;
            endTime.Location = new Point(79, 109);
            endTime.Name = "endTime";
            endTime.ShowUpDown = true;
            endTime.Size = new Size(217, 23);
            endTime.TabIndex = 5;
            endTime.Value = new DateTime(2026, 4, 18, 12, 0, 0, 0);
            endTime.ValueChanged += endTime_ValueChanged;
            // 
            // startTime
            // 
            startTime.CustomFormat = "hh:mm tt";
            startTime.Format = DateTimePickerFormat.Custom;
            startTime.Location = new Point(79, 80);
            startTime.Name = "startTime";
            startTime.ShowUpDown = true;
            startTime.Size = new Size(217, 23);
            startTime.TabIndex = 4;
            startTime.Value = new DateTime(2026, 4, 3, 12, 0, 0, 0);
            startTime.ValueChanged += startTime_ValueChanged_1;
            // 
            // showReservations
            // 
            showReservations.Location = new Point(387, 260);
            showReservations.Name = "showReservations";
            showReservations.Size = new Size(114, 53);
            showReservations.TabIndex = 5;
            showReservations.Text = "Show All Reservations";
            showReservations.UseVisualStyleBackColor = true;
            showReservations.Click += showReservations_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Location = new Point(387, 30);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(0, 0);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(387, 37);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size(510, 217);
            dataGridView1.TabIndex = 8;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(935, 554);
            Controls.Add(dataGridView1);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(showReservations);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button saveButton;
        private ComboBox labsRoom;
        private DateTimePicker date;
        private GroupBox groupBox1;
        private DateTimePicker startTime;
        private TextBox nameTxtBox;
        private DateTimePicker endTime;
        private TextBox textBox2;
        private TextBox textBox3;
        private Button deleteButton;
        private Button updateButton;
        private TextBox textBox6;
        private TextBox textBox5;
        private TextBox textBox4;
        private Button showReservations;
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dataGridView1;
    }
}
