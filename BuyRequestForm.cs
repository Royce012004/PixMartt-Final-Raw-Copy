using System;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PixMartt
{
    public partial class BuyRequestForm : Form
    {
        int currentUserID;
        int selectedArtworkID;

        public BuyRequestForm(int userID, int artworkID)
        {
            InitializeComponent();
            currentUserID = userID;
            selectedArtworkID = artworkID;
        }

        private void BuyRequestForm_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string query = "SELECT Title, PostedBy, Price, ImagePath FROM Artworks WHERE ArtworkID = @ArtworkID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ArtworkID", selectedArtworkID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblTitle.Text = "Title: " + reader["Title"].ToString();
                            lblSeller.Text = "Seller: " + reader["PostedBy"].ToString();
                            lblPrice.Text = "Price: ₱" + Convert.ToDecimal(reader["Price"]).ToString("0.00");

                            pictureBoxArtwork.ImageLocation = reader["ImagePath"].ToString();
                            pictureBoxArtwork.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    }
                }
            }
        }

        private void btnSendRequest_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                int sellerID = 0;

                string artworkQuery = "SELECT UserID FROM Artworks WHERE ArtworkID = @ArtworkID";

                using (SqlCommand artworkCmd = new SqlCommand(artworkQuery, conn))
                {
                    artworkCmd.Parameters.AddWithValue("@ArtworkID", selectedArtworkID);

                    object result = artworkCmd.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show("Artwork not found.");
                        return;
                    }

                    sellerID = Convert.ToInt32(result);
                }

                string checkQuery = @"SELECT COUNT(*) FROM PurchaseRequests
                              WHERE BuyerID = @BuyerID
                              AND ArtworkID = @ArtworkID
                              AND Status = 'Pending'";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@BuyerID", currentUserID);
                    checkCmd.Parameters.AddWithValue("@ArtworkID", selectedArtworkID);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("You already sent a request for this artwork.");
                        return;
                    }
                }

                string insertQuery = @"INSERT INTO PurchaseRequests
                               (BuyerID, SellerID, ArtworkID, Status)
                               VALUES
                               (@BuyerID, @SellerID, @ArtworkID, 'Pending')";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@BuyerID", currentUserID);
                    cmd.Parameters.AddWithValue("@SellerID", sellerID);
                    cmd.Parameters.AddWithValue("@ArtworkID", selectedArtworkID);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Request sent to seller!");

            DashboardForm dashboard = new DashboardForm(currentUserID);
            dashboard.Show();
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ArtworkDetailsForm details = new ArtworkDetailsForm(currentUserID, selectedArtworkID);
            details.Show();
            this.Hide();
        }

        private void lblPrice_Click(object sender, EventArgs e)
        {

        }
    }
}