using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PixMartt
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtFullName.Text.Trim() == "" ||
                txtUserName.Text.Trim() == "" ||
                txtPassword.Text.Trim() == "" ||
                txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Please complete all fields.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Username already exists.");
                        return;
                    }
                }

                string insertQuery = @"INSERT INTO Users (FullName, Username, Password, Email)
                               VALUES (@FullName, @Username, @Password, @Email)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Account created successfully!");

            LoginForm login = new LoginForm();
            login.Show();
            this.Hide();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {

        }
    }
}
