using LabReservation;
using LabReservation.model;
using LabReservation.model;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace LabReservation.repository
{
    public class ReservationRepository
    {
        private ConnectionSql dbConnection = new ConnectionSql();



        public bool AddReservation(Reservation res)
        {
            try
            {
                using (MySqlConnection con = dbConnection.GetConnection())
                {
                    string query = "INSERT INTO reservations (lab_room, reservation_date, start_time, end_time, reserver_name) " +
                                   "VALUES (@room, @date, @start, @end, @name)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@room", res.LabRoom);
                        cmd.Parameters.AddWithValue("@date", res.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@start", res.StartTime);
                        cmd.Parameters.AddWithValue("@end", res.EndTime);
                        cmd.Parameters.AddWithValue("@name", res.Name);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; 
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
                    string query = "SELECT COUNT(*) FROM reservations " +
                                   "WHERE lab_room = @room " +
                                   "AND reservation_date = @date " +
                                   "AND STR_TO_DATE(@start, '%h:%i %p') < STR_TO_DATE(end_time, '%h:%i %p') " +
                                   "AND STR_TO_DATE(@end, '%h:%i %p') > STR_TO_DATE(start_time, '%h:%i %p')";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@room", res.LabRoom);
                        cmd.Parameters.AddWithValue("@date", res.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@start", res.StartTime);
                        cmd.Parameters.AddWithValue("@end", res.EndTime);

                        con.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Database Error: " + ex.Message);
                return true; 
            }
        }

        public void DeletePastReservations()
        {
            try
            {
                using (MySqlConnection con = dbConnection.GetConnection())
                {
                    string query = "DELETE FROM reservations " +
                                   "WHERE reservation_date < CURDATE() " +
                                   "OR (reservation_date = CURDATE() AND STR_TO_DATE(end_time, '%h:%i %p') < CURTIME())";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery(); 
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
                    string query = "SELECT COUNT(*) FROM reservations WHERE LOWER(reserver_name) = LOWER(@name)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@name", name);

                        con.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Database Error: " + ex.Message);
                return true;
            }
        }
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
                        if (reader.Read()) 
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
            return null;
        }

        // Updates the existing reservation
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
                        cmd.Parameters.AddWithValue("@origName", originalName);

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
                    string query = "DELETE FROM reservations WHERE LOWER(reserver_name) = LOWER(@name)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@name", name);

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

        public DataTable GetAllReservations()
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = dbConnection.GetConnection())
                {
                    string query = "SELECT * FROM reservations";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        con.Open();

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