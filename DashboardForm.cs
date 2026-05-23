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
    public partial class DashboardForm : Form
    {
        int currentUserID;
        public DashboardForm(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
            ShowWelcomeName();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            ProfileForm profile = new ProfileForm(currentUserID);
            profile.Show();
            this.Hide();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("CATEGORY");
            cmbCategory.Items.Add("Digital Art");
            cmbCategory.Items.Add("Photography");
            cmbCategory.Items.Add("Abstract");
            cmbCategory.SelectedIndex = 0;

            LoadArtworks();
        }
        private void LoadArtworks()
        {
            flowLayoutPanel1.Controls.Clear();

            var artworks = DataStore.Artworks.AsEnumerable();

            if (cmbCategory.Text != "CATEGORY")
            {
                string selectedCategory = cmbCategory.Text;

                artworks = artworks.Where(a =>
                    a.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)
                );
            }

            foreach (Artwork artwork in artworks)
            {
                Panel panel = new Panel();
                panel.Width = 210;
                panel.Height = 260;
                panel.BorderStyle = BorderStyle.FixedSingle;

                PictureBox picture = new PictureBox();
                picture.Width = 160;
                picture.Height = 120;
                picture.Top = 10;
                picture.Left = 15;
                picture.ImageLocation = artwork.ImagePath;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;

                Label title = new Label();
                title.Text = artwork.Title;
                title.Top = 140;
                title.Left = 15;
                title.Width = 180;
                title.Height = 20;

                Label postedBy = new Label();
                postedBy.Text = "By: " + artwork.PostedBy;
                postedBy.Top = 165;
                postedBy.Left = 15;
                postedBy.Width = 180;
                postedBy.Height = 20;

                Label category = new Label();
                category.Text = "Category: " + artwork.Category;
                category.Top = 190;
                category.Left = 15;
                category.Width = 180;
                category.Height = 20;

                Label price = new Label();
                price.Text = "Price: ₱" + artwork.Price.ToString("0.00");
                price.Top = 210;
                price.Left = 15;
                price.Width = 180;
                price.Height = 20;

                Button btnView = new Button();
                btnView.Text = "View";
                btnView.Top = 230;
                btnView.Left = 15;
                btnView.Width = 100;

                int artworkID = artwork.ArtworkID;

                btnView.Click += (s, e) =>
                {
                    ArtworkDetailsForm details = new ArtworkDetailsForm(currentUserID, artworkID);
                    details.Show();
                    this.Hide();
                };

                panel.Controls.Add(picture);
                panel.Controls.Add(title);
                panel.Controls.Add(postedBy);
                panel.Controls.Add(category);
                panel.Controls.Add(price);
                panel.Controls.Add(btnView);

                flowLayoutPanel1.Controls.Add(panel);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            UploadArtworkForm upload = new UploadArtworkForm(currentUserID);
            upload.Show();
            this.Hide();
        }

        private void btnGallery_Click(object sender, EventArgs e)
        {
            DownloadedGalleryForm gallery = new DownloadedGalleryForm(currentUserID);
            gallery.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Hide(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadArtworks();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadArtworks();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadArtworks();
        }

        private void btnRequests_Click(object sender, EventArgs e)
        {
            SellerRequestsForm requests = new SellerRequestsForm(currentUserID);
            requests.Show();
            this.Hide();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadArtworks();
        }
        private void ShowWelcomeName()
        {
            var user = DataStore.Users.FirstOrDefault(u => u.UserID == currentUserID);

            if (user != null)
            {
                lblWelcome.Text = "Welcome, " + user.FullName + "!";
            }
            else
            {
                lblWelcome.Text = "Welcome, User!";
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
