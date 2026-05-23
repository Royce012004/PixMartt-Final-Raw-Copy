using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
                Panel panel = new Panel();
                panel.Width = 180;
                panel.Height = 240;
                panel.BorderStyle = BorderStyle.FixedSingle;

                PictureBox picture = new PictureBox();
                picture.Width = 150;
                picture.Height = 120;
                picture.Top = 10;
                picture.Left = 15;
                picture.ImageLocation = artwork.ImagePath;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;

                Label title = new Label();
                title.Text = artwork.Title;
                title.Top = 135;
                title.Left = 15;
                title.Width = 150;

                Label postedBy = new Label();
                postedBy.Text = "By: " + artwork.PostedBy;
                postedBy.Top = 160;
                postedBy.Left = 15;
                postedBy.Width = 150;

                Button btnDownloadImage = new Button();
                btnDownloadImage.Text = "Download";
                btnDownloadImage.Top = 190;
                btnDownloadImage.Left = 15;
                btnDownloadImage.Width = 150;
                btnDownloadImage.Height = 30;

                string imagePath = artwork.ImagePath;

                btnDownloadImage.Click += (s, e) =>
                {
                    DownloadImageToFileExplorer(imagePath);
                };

                panel.Controls.Add(picture);
                panel.Controls.Add(title);
                panel.Controls.Add(postedBy);
                panel.Controls.Add(btnDownloadImage);

                flowLayoutPanel1.Controls.Add(panel);
            }
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
    }
}