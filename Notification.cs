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
    public partial class Notification : Form
    {
        int currentUserID;
        public Notification(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
        }

        private void Notification_Load(object sender, EventArgs e)
        {
            LoadNotifications();
        }
        private void LoadNotifications()
        {
            flowLayoutPanelNotifications.Controls.Clear();

            var notifications = DataStore.PurchaseRequests
                .Where(r => r.BuyerID == currentUserID &&
                       (r.Status == "Accepted" || r.Status == "Rejected"))
                .ToList();

            if (notifications.Count == 0)
            {
                Label noNotif = new Label();
                noNotif.Text = "No notifications yet.";
                noNotif.Font = new Font("Segoe UI", 12, FontStyle.Regular);
                noNotif.ForeColor = Color.Gray;
                noNotif.Width = 300;
                noNotif.Height = 40;

                flowLayoutPanelNotifications.Controls.Add(noNotif);
                return;
            }

            foreach (PurchaseRequest request in notifications)
            {
                Artwork artwork = DataStore.Artworks
                    .FirstOrDefault(a => a.ArtworkID == request.ArtworkID);

                User seller = DataStore.Users
                    .FirstOrDefault(u => u.UserID == request.SellerID);

                if (artwork == null)
                {
                    continue;
                }

                Panel panel = new Panel();
                panel.Width = 210;
                panel.Height = 280;
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.BackColor = Color.White;
                panel.Margin = new Padding(10);

                PictureBox picture = new PictureBox();
                picture.Width = 160;
                picture.Height = 120;
                picture.Top = 10;
                picture.Left = 25;
                picture.ImageLocation = artwork.ImagePath;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;

                Label title = new Label();
                title.Text = artwork.Title;
                title.Top = 140;
                title.Left = 15;
                title.Width = 180;
                title.Height = 25;
                title.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                title.ForeColor = Color.Black;

                Label sellerName = new Label();
                sellerName.Text = "Seller: " + (seller != null ? seller.FullName : "Unknown");
                sellerName.Top = 165;
                sellerName.Left = 15;
                sellerName.Width = 180;
                sellerName.Height = 25;
                sellerName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                sellerName.ForeColor = Color.DimGray;

                Label price = new Label();
                price.Text = "Price: ₱" + artwork.Price.ToString("0.00");
                price.Top = 190;
                price.Left = 15;
                price.Width = 180;
                price.Height = 25;
                price.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                price.ForeColor = Color.DimGray;

                Label status = new Label();
                status.Text = "Status: " + request.Status;
                status.Top = 215;
                status.Left = 15;
                status.Width = 180;
                status.Height = 25;
                status.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                if (request.Status == "Accepted")
                {
                    status.ForeColor = Color.SeaGreen;
                }
                else
                {
                    status.ForeColor = Color.Firebrick;
                }

                panel.Controls.Add(picture);
                panel.Controls.Add(title);
                panel.Controls.Add(sellerName);
                panel.Controls.Add(price);
                panel.Controls.Add(status);

                flowLayoutPanelNotifications.Controls.Add(panel);
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
