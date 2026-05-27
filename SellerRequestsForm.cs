using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

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

                if (artwork == null || buyer == null)
                    continue;

                Panel card = new Panel();
                card.Width = 760;
                card.Height = 190;
                card.BackColor = Color.White;
                card.Margin = new Padding(10);
                card.BorderStyle = BorderStyle.None;

                // Artwork image
                PictureBox picture = new PictureBox();
                picture.Width = 150;
                picture.Height = 140;
                picture.Top = 25;
                picture.Left = 25;
                picture.ImageLocation = artwork.ImagePath;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.BackColor = Color.FromArgb(220, 210, 200);
                picture.BorderStyle = BorderStyle.None;

                MakeRounded(picture, 18);

                // Artwork title
                Label title = new Label();
                title.Text = artwork.Title.ToUpper();
                title.Top = 28;
                title.Left = 200;
                title.Width = 500;
                title.Height = 30;
                title.Font = new Font("Arial", 15, FontStyle.Bold);
                title.ForeColor = Color.Black;

                // Buyer
                Label buyerLabel = new Label();
                buyerLabel.Text = "BUYER: " + buyer.FullName.ToUpper();
                buyerLabel.Top = 60;
                buyerLabel.Left = 200;
                buyerLabel.Width = 500;
                buyerLabel.Height = 20;
                buyerLabel.Font = new Font("Arial", 9, FontStyle.Bold);
                buyerLabel.ForeColor = Color.Black;

                // Price
                Label price = new Label();
                price.Text = "₱" + artwork.Price.ToString("0.00");
                price.Top = 88;
                price.Left = 200;
                price.Width = 85;
                price.Height = 28;
                price.TextAlign = ContentAlignment.MiddleCenter;
                price.Font = new Font("Arial", 11, FontStyle.Bold);
                price.ForeColor = Color.White;
                price.BackColor = Color.Gray;

                MakeRounded(price, 12);

                // Status
                Label status = new Label();
                status.Text = request.Status.ToUpper();
                status.Top = 92;
                status.Left = 300;
                status.Width = 180;
                status.Height = 22;
                status.Font = new Font("Arial", 9, FontStyle.Bold);
                status.ForeColor = Color.Gray;

                // Accept button
                Button btnAccept = new Button();
                btnAccept.Text = "ACCEPT";
                btnAccept.Top = 130;
                btnAccept.Left = 200;
                btnAccept.Width = 120;
                btnAccept.Height = 35;
                btnAccept.BackColor = Color.Black;
                btnAccept.ForeColor = Color.White;
                btnAccept.FlatStyle = FlatStyle.Flat;
                btnAccept.FlatAppearance.BorderSize = 0;
                btnAccept.Font = new Font("Arial", 9, FontStyle.Bold);

                // Reject button
                Button btnReject = new Button();
                btnReject.Text = "REJECT";
                btnReject.Top = 130;
                btnReject.Left = 335;
                btnReject.Width = 120;
                btnReject.Height = 35;
                btnReject.BackColor = Color.FromArgb(210, 210, 210);
                btnReject.ForeColor = Color.Black;
                btnReject.FlatStyle = FlatStyle.Flat;
                btnReject.FlatAppearance.BorderSize = 0;
                btnReject.Font = new Font("Arial", 9, FontStyle.Bold);

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

                card.Controls.Add(picture);
                card.Controls.Add(title);
                card.Controls.Add(buyerLabel);
                card.Controls.Add(price);
                card.Controls.Add(status);
                card.Controls.Add(btnAccept);
                card.Controls.Add(btnReject);

                flowLayoutPanelRequests.Controls.Add(card);
            }
        }

        private void MakeRounded(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);

            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

            path.CloseAllFigures();

            control.Region = new Region(path);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm(currentUserID);
            dashboard.Show();
            this.Hide();
        }
    }
}
