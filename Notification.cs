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

            flowLayoutPanelNotifications.Controls.Clear();

            flowLayoutPanelNotifications.AutoScroll = true;
            flowLayoutPanelNotifications.WrapContents = true;
            flowLayoutPanelNotifications.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanelNotifications.Padding = new Padding(20);
            flowLayoutPanelNotifications.BackColor = Color.White;

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

                Panel card = new Panel();
                card.Width = 220;
                card.Height = 320;
                card.BackColor = Color.White;
                card.Margin = new Padding(15);
                card.BorderStyle = BorderStyle.None;

                // Artwork image
                PictureBox picture = new PictureBox();
                picture.Width = 190;
                picture.Height = 180;
                picture.Left = 15;
                picture.Top = 10;
                picture.ImageLocation = artwork.ImagePath;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.BackColor = Color.FromArgb(220, 210, 200);
                picture.BorderStyle = BorderStyle.None;

                MakeRounded(picture, 18);

                // Title
                Label title = new Label();
                title.Text = artwork.Title.ToUpper();
                title.Left = 15;
                title.Top = 200;
                title.Width = 190;
                title.Height = 25;
                title.Font = new Font("Arial", 13, FontStyle.Bold);
                title.ForeColor = Color.Black;

                // Seller
                Label sellerName = new Label();
                sellerName.Text = (seller != null ? seller.FullName.ToUpper() : "UNKNOWN SELLER");
                sellerName.Left = 15;
                sellerName.Top = 225;
                sellerName.Width = 190;
                sellerName.Height = 18;
                sellerName.Font = new Font("Arial", 7, FontStyle.Regular);
                sellerName.ForeColor = Color.Black;

                // Price
                Label price = new Label();
                price.Text = "₱" + artwork.Price.ToString("0.##");
                price.Left = 15;
                price.Top = 248;
                price.Width = 65;
                price.Height = 25;
                price.TextAlign = ContentAlignment.MiddleCenter;
                price.Font = new Font("Arial", 10, FontStyle.Bold);
                price.ForeColor = Color.White;
                price.BackColor = Color.Gray;

                MakeRounded(price, 12);

                // Status
                Label status = new Label();
                status.Left = 95;
                status.Top = 248;
                status.Width = 110;
                status.Height = 25;
                status.TextAlign = ContentAlignment.MiddleCenter;
                status.Font = new Font("Arial", 8, FontStyle.Bold);
                status.ForeColor = Color.White;
                status.Text = request.Status.ToUpper();

                if (request.Status == "Accepted")
                {
                    status.BackColor = Color.SeaGreen;
                }
                else
                {
                    status.BackColor = Color.Firebrick;
                }

                MakeRounded(status, 12);

                // Note
                Label note = new Label();
                note.Left = 15;
                note.Top = 285;
                note.Width = 190;
                note.Height = 35;
                note.Font = new Font("Arial", 7, FontStyle.Italic);
                note.ForeColor = Color.FromArgb(160, 160, 160);

                if (request.Status == "Accepted")
                {
                    note.Text = "See your Purchase Gallery to download this artwork.";
                }
                else if (request.Status == "Pending")
                {
                    note.Text = "Please wait for the seller to accept your request.";
                }
                else
                {
                    note.Text = "This purchase request was rejected.";
                }

                card.Controls.Add(picture);
                card.Controls.Add(title);
                card.Controls.Add(sellerName);
                card.Controls.Add(price);
                card.Controls.Add(status);
                card.Controls.Add(note);

                flowLayoutPanelNotifications.Controls.Add(card);
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
