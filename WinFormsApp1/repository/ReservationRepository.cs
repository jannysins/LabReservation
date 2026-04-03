using LabReservation;
using LabReservation.model;
using LabReservation.model; // Adjust to your actual namespace
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace LabReservation.repository
{
    public class ReservationRepository
    {
        // Calling the connection class you made earlier
        private ConnectionSql dbConnection = new ConnectionSql();



        public bool AddReservation(Reservation res)
        {
            try
            {
                using (MySqlConnection con = dbConnection.GetConnection())
                {
                    // Make sure the table name and column names match your actual MySQL database!
                    string query = "INSERT INTO reservations (lab_room, reservation_date, start_time, end_time, reserver_name) " +
                                   "VALUES (@room, @date, @start, @end, @name)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Using parameters prevents SQL Injection!
                        cmd.Parameters.AddWithValue("@room", res.LabRoom);
                        cmd.Parameters.AddWithValue("@date", res.Date.ToString("yyyy-MM-dd")); // Format for MySQL DATE column
                        cmd.Parameters.AddWithValue("@start", res.StartTime);
                        cmd.Parameters.AddWithValue("@end", res.EndTime);
                        cmd.Parameters.AddWithValue("@name", res.Name);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Returns true if saving was successful
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Database Error: " + ex.Message);
                return false;
            }
        }

        public bool IsTimeSlotTaken(Reservation res)
        {
            try
            {
                using (MySqlConnection con = dbConnection.GetConnection())
                {
                    // We use COUNT(*) to check if any row matches the same room, date, and start time
                    string query = "SELECT COUNT(*) FROM reservations " +
                                   "WHERE lab_room = @room " +
                                   "AND reservation_date = @date " +
                                   "AND start_time = @start";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@room", res.LabRoom);
                        cmd.Parameters.AddWithValue("@date", res.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@start", res.StartTime);

                        con.Open();
                        // ExecuteScalar returns the first column of the first row (which is our COUNT)
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        // If count is greater than 0, it means the slot is taken!
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Database Error: " + ex.Message);
                return true; // If there's an error, return true (taken) just to be safe and prevent saving
            }
        }

        public void DeletePastReservations()
        {
            try
            {
                using (MySqlConnection con = dbConnection.GetConnection())
                {
                    // The query now checks TWO things:
                    // 1. Is the date older than today? (Wipe it out immediately)
                    // 2. Is the date exactly today, BUT the end_time has already passed? (Wipe it out too)

                    string query = "DELETE FROM reservations " +
                                   "WHERE reservation_date < CURDATE() " +
                                   "OR (reservation_date = CURDATE() AND STR_TO_DATE(end_time, '%l:%i:%s %p') < CURTIME())";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery(); // Executes the deletion quietly
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Auto-Cleanup Error: " + ex.Message);
            }
        }

        public bool HasUserAlreadyBooked(string name)
        {
            try
            {
                using (MySqlConnection con = dbConnection.GetConnection())
                {
                    // LOWER() ensures that 'John', 'john', and 'JOHN' are treated as exact matches
                    string query = "SELECT COUNT(*) FROM reservations WHERE LOWER(reserver_name) = LOWER(@name)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@name", name);

                        con.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        // If count is greater than 0, it means this name is already in the database!
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Database Error: " + ex.Message);
                return true; // Return true to block the saving just in case of an error
            }
        }
        // 1. Fetches the existing reservation based on the name
        public Reservation GetReservationByName(string name)
        {
            using (MySqlConnection con = dbConnection.GetConnection())
            {
                string query = "SELECT * FROM reservations WHERE LOWER(reserver_name) = LOWER(@name)";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    con.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // If a record is found
                        {
                            return new Reservation
                            {
                                LabRoom = reader["lab_room"].ToString(),
                                Date = Convert.ToDateTime(reader["reservation_date"]),
                                StartTime = reader["start_time"].ToString(),
                                EndTime = reader["end_time"].ToString(),
                                Name = reader["reserver_name"].ToString()
                            };
                        }
                    }
                }
            }
            return null; // Return null if the name doesn't exist
        }

        // 2. Updates the existing reservation
        public bool UpdateReservation(string originalName, Reservation updatedRes)
        {
            try
            {
                using (MySqlConnection con = dbConnection.GetConnection())
                {
                    string query = "UPDATE reservations SET lab_room = @room, reservation_date = @date, " +
                                   "start_time = @start, end_time = @end, reserver_name = @newName " +
                                   "WHERE LOWER(reserver_name) = LOWER(@origName)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@room", updatedRes.LabRoom);
                        cmd.Parameters.AddWithValue("@date", updatedRes.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@start", updatedRes.StartTime);
                        cmd.Parameters.AddWithValue("@end", updatedRes.EndTime);
                        cmd.Parameters.AddWithValue("@newName", updatedRes.Name);
                        cmd.Parameters.AddWithValue("@origName", originalName); // To find the right row!

                        con.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Database Error: " + ex.Message);
                return false;
            }
        }

        public bool DeleteReservationByName(string name)
        {
            try
            {
                using (MySqlConnection con = dbConnection.GetConnection())
                {
                    // LOWER() ensures it deletes correctly whether they type "John" or "JOHN"
                    string query = "DELETE FROM reservations WHERE LOWER(reserver_name) = LOWER(@name)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@name", name);

                        con.Open();
                        int rows = cmd.ExecuteNonQuery();

                        // Returns true if at least 1 row was successfully deleted
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Database Error: " + ex.Message);
                return false;
            }
        }

        public DataTable GetAllReservations()
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = dbConnection.GetConnection())
                {
                    // Simple query to get everything
                    string query = "SELECT * FROM reservations";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        con.Open();

                        // A DataAdapter automatically reads the data and fills a DataTable
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Database Error: " + ex.Message);
            }
            return dt;
        }
    }
}