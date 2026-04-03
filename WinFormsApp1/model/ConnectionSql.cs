using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace LabReservation
{
    public class ConnectionSql
    {
        // Centralized connection string
        private string connectionString = "server=localhost;uid=root;pwd=Noredlac2025;database=appsdev";

        /// <summary>
        /// Returns a new MySqlConnection object using the centralized connection string.
        /// </summary>
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Optional utility method to test if the database connects successfully.
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection failed: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }


}