using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PixMartt
{
    public partial class ArtworkDetailsForm : Form
    {
        int currentUserID;
        int selectedArtworkID;
        public ArtworkDetailsForm(int userID, int artworkID)
        {
            InitializeComponent();
            currentUserID = userID;
            selectedArtworkID = artworkID;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            BuyRequestForm requestForm = new BuyRequestForm(currentUserID, selectedArtworkID);
            requestForm.Show();
            this.Hide();
        }

        private void ArtworkDetailsForm_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Artworks WHERE ArtworkID = @ArtworkID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ArtworkID", selectedArtworkID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int artworkUserID = Convert.ToInt32(reader["UserID"]);

                            lblTitle.Text = reader["Title"].ToString();
                            lblPostedBy.Text = "Posted By: " + reader["PostedBy"].ToString();
                            lblCategory.Text = "Category: " + reader["Category"].ToString();
                            lblPrice.Text = "Price: ₱" + Convert.ToDecimal(reader["Price"]).ToString("0.00");
                            lblDescription.Text = reader["Description"].ToString();

                            pictureBoxArtwork.ImageLocation = reader["ImagePath"].ToString();
                            pictureBoxArtwork.SizeMode = PictureBoxSizeMode.StretchImage;

                            if (artworkUserID == currentUserID)
                            {
                                btnDownload.Enabled = false;
                                btnDownload.Text = "Your Artwork";
                                btnDownload.ForeColor = Color.White;
                            }
                            else
                            {
                                btnDownload.Enabled = true;
                                btnDownload.Text = "Request To Buy";
                            }
                        }
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm(currentUserID);
            dashboard.Show();
            this.Hide();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}

