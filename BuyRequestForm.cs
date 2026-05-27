using System;
using System.Linq;
using System.Windows.Forms;

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
            var artwork = DataStore.Artworks.FirstOrDefault(a => a.ArtworkID == selectedArtworkID);

            if (artwork != null)
            {
                lblTitle.Text = "Title: " + artwork.Title;
                lblSeller.Text = "Seller: " + artwork.PostedBy;
                lblPrice.Text = "Price: ₱" + artwork.Price.ToString("0.00");

                pictureBoxArtwork.ImageLocation = artwork.ImagePath;
                pictureBoxArtwork.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void btnSendRequest_Click(object sender, EventArgs e)
        {
            var artwork = DataStore.Artworks.FirstOrDefault(a => a.ArtworkID == selectedArtworkID);

            if (artwork == null)
            {
                MessageBox.Show("Artwork not found.");
                return;
            }

            var existingRequest = DataStore.PurchaseRequests.FirstOrDefault(r =>
                r.BuyerID == currentUserID &&
                r.ArtworkID == selectedArtworkID &&
                r.Status == "Pending"
            );

            if (existingRequest != null)
            {
                MessageBox.Show("You already sent a request for this artwork.");
                return;
            }

            PurchaseRequest request = new PurchaseRequest
            {
                RequestID = DataStore.NextRequestID++,
                BuyerID = currentUserID,
                SellerID = artwork.UserID,
                ArtworkID = selectedArtworkID,
                Status = "Pending"
            };

            DataStore.PurchaseRequests.Add(request);

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