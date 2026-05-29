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
    public partial class UploadArtworkForm : Form
    {
        int currentUserID;
        string imagePath = "";
        public UploadArtworkForm(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Digital Art");
            cmbCategory.Items.Add("Photography");
            cmbCategory.Items.Add("Abstract");
            cmbCategory.SelectedIndex = 0;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() == "" ||
        cmbCategory.Text.Trim() == "" ||
        txtDescription.Text.Trim() == "" ||
        txtprice.Text.Trim() == "" ||
        imagePath == "")
            {
                MessageBox.Show("Please complete all fields and select an image.");
                return;
            }

            decimal price;

            if (!decimal.TryParse(txtprice.Text.Trim(), out price))
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string getUserQuery = "SELECT FullName FROM Users WHERE UserID = @UserID";
                string postedBy = "";

                using (SqlCommand getUserCmd = new SqlCommand(getUserQuery, conn))
                {
                    getUserCmd.Parameters.AddWithValue("@UserID", currentUserID);

                    object result = getUserCmd.ExecuteScalar();

                    if (result != null)
                    {
                        postedBy = result.ToString();
                    }
                }

                string insertQuery = @"INSERT INTO Artworks 
                               (UserID, PostedBy, Title, Category, Description, Price, ImagePath)
                               VALUES 
                               (@UserID, @PostedBy, @Title, @Category, @Description, @Price, @ImagePath)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", currentUserID);
                    cmd.Parameters.AddWithValue("@PostedBy", postedBy);
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Category", cmbCategory.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Artwork posted successfully!");

            DashboardForm dashboard = new DashboardForm(currentUserID);
            dashboard.Show();
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm(currentUserID);
            dashboard.Show();
            this.Hide();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files|*.jpg;*.jpeg;*.png";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                imagePath = openFile.FileName;
                pictureBoxArtwork.ImageLocation = imagePath;
                pictureBoxArtwork.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void pictureBoxArtwork_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
