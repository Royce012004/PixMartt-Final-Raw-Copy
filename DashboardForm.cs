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
using System.Data.SqlClient;

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

            ShowWelcomeName();
            LoadArtworks();
        }
        private void LoadArtworks()
        {
            flowLayoutPanel1.Controls.Clear();

            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string query = @"SELECT ArtworkID, UserID, PostedBy, Title, Category, Description, Price, ImagePath
                         FROM Artworks
                         WHERE (@Category = 'CATEGORY' OR Category = @Category)
                         AND (@Search = '' OR Title LIKE @SearchLike OR PostedBy LIKE @SearchLike OR Category LIKE @SearchLike)
                         ORDER BY ArtworkID DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    string searchText = txtSearch.Text.Trim();

                    if (searchText == "Search Bar")
                    {
                        searchText = "";
                    }

                    string selectedCategory = cmbCategory.Text.Trim();

                    if (selectedCategory == "" || selectedCategory == "CATEGORY")
                    {
                        selectedCategory = "CATEGORY";
                    }

                    cmd.Parameters.AddWithValue("@Category", selectedCategory);
                    cmd.Parameters.AddWithValue("@Search", searchText);
                    cmd.Parameters.AddWithValue("@SearchLike", "%" + searchText + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Artwork artwork = new Artwork
                            {
                                ArtworkID = Convert.ToInt32(reader["ArtworkID"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                PostedBy = reader["PostedBy"].ToString(),
                                Title = reader["Title"].ToString(),
                                Category = reader["Category"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                ImagePath = reader["ImagePath"].ToString()
                            };

                            Panel card = new Panel();
                            card.Width = 220;
                            card.Height = 320;
                            card.BackColor = Color.White;
                            card.Margin = new Padding(15);
                            card.BorderStyle = BorderStyle.None;

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

                            Label title = new Label();
                            title.Text = artwork.Title.ToUpper();
                            title.Left = 15;
                            title.Top = 200;
                            title.Width = 190;
                            title.Height = 25;
                            title.Font = new Font("Arial", 13, FontStyle.Bold);
                            title.ForeColor = Color.Black;

                            Label author = new Label();
                            author.Text = artwork.PostedBy.ToUpper();
                            author.Left = 15;
                            author.Top = 225;
                            author.Width = 190;
                            author.Height = 18;
                            author.Font = new Font("Arial", 7, FontStyle.Regular);
                            author.ForeColor = Color.Black;

                            Label category = new Label();
                            category.Text = artwork.Category.ToUpper();
                            category.Left = 15;
                            category.Top = 243;
                            category.Width = 190;
                            category.Height = 18;
                            category.Font = new Font("Arial", 7, FontStyle.Bold);
                            category.ForeColor = Color.Gray;

                            Label price = new Label();
                            price.Text = "₱" + artwork.Price.ToString("0.##");
                            price.Left = 15;
                            price.Top = 263;
                            price.Width = 65;
                            price.Height = 25;
                            price.TextAlign = ContentAlignment.MiddleCenter;
                            price.Font = new Font("Arial", 10, FontStyle.Bold);
                            price.ForeColor = Color.White;
                            price.BackColor = Color.Gray;

                            Button btnView = new Button();
                            btnView.Text = "VIEW";
                            btnView.Left = 95;
                            btnView.Top = 263;
                            btnView.Width = 95;
                            btnView.Height = 25;
                            btnView.BackColor = Color.Black;
                            btnView.ForeColor = Color.White;
                            btnView.FlatStyle = FlatStyle.Flat;
                            btnView.FlatAppearance.BorderSize = 0;
                            btnView.Font = new Font("Arial", 8, FontStyle.Bold);

                            int artworkID = artwork.ArtworkID;

                            btnView.Click += (s, e) =>
                            {
                                ArtworkDetailsForm details = new ArtworkDetailsForm(currentUserID, artworkID);
                                details.Show();
                                this.Hide();
                            };

                            card.Controls.Add(picture);
                            card.Controls.Add(title);
                            card.Controls.Add(author);
                            card.Controls.Add(category);
                            card.Controls.Add(price);
                            card.Controls.Add(btnView);

                            flowLayoutPanel1.Controls.Add(card);
                        }
                    }
                }
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
            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string query = "SELECT FullName FROM Users WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", currentUserID);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        lblWelcome.Text = "Welcome, " + result.ToString() + "!";
                    }
                    else
                    {
                        lblWelcome.Text = "Welcome, User!";
                    }
                }
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void BtnNotification_Click(object sender, EventArgs e)
        {
            Notification notifForm = new Notification(currentUserID);
            notifForm.Show();
            this.Hide();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "Search Bar";
            cmbCategory.Text = "CATEGORY";
            LoadArtworks();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
