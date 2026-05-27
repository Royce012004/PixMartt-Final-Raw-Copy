using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace PixMartt
{
    public partial class DownloadedGalleryForm : Form
    {
        int currentUserID;

        public DownloadedGalleryForm(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm(currentUserID);
            dashboard.Show();
            this.Hide();
        }

        private void DownloadedGalleryForm_Load(object sender, EventArgs e)
        {
            LoadDownloadedArtworks();
        }

        private void LoadDownloadedArtworks()
        {
            flowLayoutPanel1.Controls.Clear();

            var downloadedArtworks =
                from download in DataStore.Downloads
                join artwork in DataStore.Artworks
                on download.ArtworkID equals artwork.ArtworkID
                where download.UserID == currentUserID
                select artwork;

            foreach (Artwork artwork in downloadedArtworks)
            {
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

                // Posted by
                Label postedBy = new Label();
                postedBy.Text = artwork.PostedBy.ToUpper();
                postedBy.Left = 15;
                postedBy.Top = 225;
                postedBy.Width = 190;
                postedBy.Height = 18;
                postedBy.Font = new Font("Arial", 7, FontStyle.Regular);
                postedBy.ForeColor = Color.Black;

                // Category
                Label category = new Label();
                category.Text = artwork.Category.ToUpper();
                category.Left = 15;
                category.Top = 243;
                category.Width = 190;
                category.Height = 18;
                category.Font = new Font("Arial", 7, FontStyle.Bold);
                category.ForeColor = Color.Gray;

                // Download button
                Button btnDownloadImage = new Button();
                btnDownloadImage.Text = "DOWNLOAD";
                btnDownloadImage.Left = 15;
                btnDownloadImage.Top = 265;
                btnDownloadImage.Width = 95;
                btnDownloadImage.Height = 25;
                btnDownloadImage.BackColor = Color.Black;
                btnDownloadImage.ForeColor = Color.White;
                btnDownloadImage.FlatStyle = FlatStyle.Flat;
                btnDownloadImage.FlatAppearance.BorderSize = 0;
                btnDownloadImage.Font = new Font("Arial", 8, FontStyle.Bold);

                string imagePath = artwork.ImagePath;

                btnDownloadImage.Click += (s, e) =>
                {
                    DownloadImageToFileExplorer(imagePath);
                };

                card.Controls.Add(picture);
                card.Controls.Add(title);
                card.Controls.Add(postedBy);
                card.Controls.Add(category);
                card.Controls.Add(btnDownloadImage);

                flowLayoutPanel1.Controls.Add(card);
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

        private void DownloadImageToFileExplorer(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                MessageBox.Show("Image file not found.");
                return;
            }

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Save Artwork Image";

            string extension = Path.GetExtension(imagePath);
            string fileName = Path.GetFileNameWithoutExtension(imagePath);

            if (extension.ToLower() == ".png")
            {
                saveFile.Filter = "PNG Image|*.png";
            }
            else
            {
                saveFile.Filter = "JPEG Image|*.jpg";
            }

            saveFile.FileName = fileName + extension;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                File.Copy(imagePath, saveFile.FileName, true);
                MessageBox.Show("Image downloaded successfully!");
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}