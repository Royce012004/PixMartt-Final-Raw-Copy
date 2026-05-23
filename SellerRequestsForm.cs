using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixMartt
{
    public partial class SellerRequestsForm : Form
    {
        int currentUserID;
        public SellerRequestsForm(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
        }

        private void SellerRequestsForm_Load(object sender, EventArgs e)
        {
            LoadRequests();
        }
        private void LoadRequests()
        {
            flowLayoutPanelRequests.Controls.Clear();

            var requests = DataStore.PurchaseRequests
                .Where(r => r.SellerID == currentUserID && r.Status == "Pending")
                .ToList();

            if (requests.Count == 0)
            {
                Label noRequests = new Label();
                noRequests.Text = "No pending purchase requests.";
                noRequests.Width = 300;
                noRequests.Height = 30;
                flowLayoutPanelRequests.Controls.Add(noRequests);
                return;
            }

            foreach (PurchaseRequest request in requests)
            {
                var artwork = DataStore.Artworks.FirstOrDefault(a => a.ArtworkID == request.ArtworkID);
                var buyer = DataStore.Users.FirstOrDefault(u => u.UserID == request.BuyerID);

                Panel panel = new Panel();
                panel.Width = 500;
                panel.Height = 170;
                panel.BorderStyle = BorderStyle.FixedSingle;

                PictureBox picture = new PictureBox();
                picture.Width = 120;
                picture.Height = 120;
                picture.Top = 15;
                picture.Left = 15;
                picture.ImageLocation = artwork.ImagePath;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;

                Label details = new Label();
                details.Top = 15;
                details.Left = 150;
                details.Width = 320;
                details.Height = 80;
                details.Text =
                    "Artwork: " + artwork.Title +
                    "\nBuyer: " + buyer.FullName +
                    "\nPrice: ₱" + artwork.Price.ToString("0.00") +
                    "\nStatus: " + request.Status;

                Button btnAccept = new Button();
                btnAccept.Text = "Accept";
                btnAccept.Top = 105;
                btnAccept.Left = 150;
                btnAccept.Width = 90;

                Button btnReject = new Button();
                btnReject.Text = "Reject";
                btnReject.Top = 105;
                btnReject.Left = 250;
                btnReject.Width = 90;

                btnAccept.Click += (s, e) =>
                {
                    request.Status = "Accepted";

                    Download download = new Download
                    {
                        DownloadID = DataStore.NextDownloadID++,
                        UserID = request.BuyerID,
                        ArtworkID = request.ArtworkID
                    };

                    DataStore.Downloads.Add(download);

                    MessageBox.Show("Request accepted. Artwork added to buyer's gallery.");
                    LoadRequests();
                };

                btnReject.Click += (s, e) =>
                {
                    request.Status = "Rejected";
                    MessageBox.Show("Request rejected.");
                    LoadRequests();
                };

                panel.Controls.Add(picture);
                panel.Controls.Add(details);
                panel.Controls.Add(btnAccept);
                panel.Controls.Add(btnReject);

                flowLayoutPanelRequests.Controls.Add(panel);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm(currentUserID);
            dashboard.Show();
            this.Hide();
        }
    }
}
