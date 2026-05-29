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
    public partial class ProfileForm : Form
    {
        int currentUserID;
        public ProfileForm(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
        }
        private void ProfileForm_Load(object sender, EventArgs e)
        {
            LoadUserDetails();
            SetEditMode(false);
        }
        private void LoadUserDetails()
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string query = "SELECT FullName, Username, Password, Email FROM Users WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", currentUserID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblFullName.Text = "Full Name: " + reader["FullName"].ToString();
                            txtUsername.Text = reader["Username"].ToString();
                            txtPassword.Text = reader["Password"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                        }
                    }
                }
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm(currentUserID);
            dashboard.Show();
            this.Hide();
        }

      
        
        private void SetEditMode(bool isEditing)
        {
            txtUsername.Enabled = isEditing;
            txtPassword.Enabled = isEditing;
            txtEmail.Enabled = isEditing;

            btnSave.Visible = isEditing;
            btnEdit.Visible = !isEditing;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            SetEditMode(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" ||
                txtPassword.Text.Trim() == "" ||
                txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Please complete all fields.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string checkQuery = @"SELECT COUNT(*) FROM Users 
                              WHERE Username = @Username 
                              AND UserID != @UserID";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    checkCmd.Parameters.AddWithValue("@UserID", currentUserID);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Username is already taken.");
                        return;
                    }
                }

                string updateQuery = @"UPDATE Users
                               SET Username = @Username,
                                   Password = @Password,
                                   Email = @Email
                               WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserID", currentUserID);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Profile updated successfully!");
            LoadUserDetails();
            SetEditMode(false);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
