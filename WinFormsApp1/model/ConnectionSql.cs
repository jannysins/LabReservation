using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace LabReservation
{
    public class ConnectionSql
    {
        private string connectionString = "server=localhost;uid=root;pwd=Noredlac2025;database=appsdev";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

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