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

            flowLayoutPanelRequests.AutoScroll = true;
            flowLayoutPanelRequests.WrapContents = true;
            flowLayoutPanelRequests.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanelRequests.Padding = new Padding(10);
            flowLayoutPanelRequests.BackColor = Color.White;

            int requestCount = 0;

            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string query = @"SELECT 
                            PR.RequestID,
                            PR.BuyerID,
                            PR.SellerID,
                            PR.ArtworkID,
                            PR.Status,
                            A.Title,
                            A.Price,
                            A.ImagePath,
                            U.FullName AS BuyerName
                         FROM PurchaseRequests PR
                         INNER JOIN Artworks A ON PR.ArtworkID = A.ArtworkID
                         INNER JOIN Users U ON PR.BuyerID = U.UserID
                         WHERE PR.SellerID = @SellerID
                         AND PR.Status = 'Pending'
                         ORDER BY PR.RequestID DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SellerID", currentUserID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            requestCount++;

                            int requestID = Convert.ToInt32(reader["RequestID"]);
                            int buyerID = Convert.ToInt32(reader["BuyerID"]);
                            int artworkID = Convert.ToInt32(reader["ArtworkID"]);

                            string artworkTitle = reader["Title"].ToString();
                            string buyerName = reader["BuyerName"].ToString();
                            string requestStatus = reader["Status"].ToString();
                            decimal artworkPrice = Convert.ToDecimal(reader["Price"]);
                            string artworkImagePath = reader["ImagePath"].ToString();

                            Panel card = new Panel();
                            card.Width = 760;
                            card.Height = 190;
                            card.BackColor = Color.White;
                            card.Margin = new Padding(10);
                            card.BorderStyle = BorderStyle.FixedSingle;

                            PictureBox picture = new PictureBox();
                            picture.Width = 150;
                            picture.Height = 140;
                            picture.Top = 25;
                            picture.Left = 25;
                            picture.ImageLocation = artworkImagePath;
                            picture.SizeMode = PictureBoxSizeMode.StretchImage;
                            picture.BackColor = Color.FromArgb(220, 210, 200);
                            picture.BorderStyle = BorderStyle.None;
                            MakeRounded(picture, 18);

                            Label title = new Label();
                            title.Text = artworkTitle.ToUpper();
                            title.Top = 28;
                            title.Left = 200;
                            title.Width = 500;
                            title.Height = 30;
                            title.Font = new Font("Arial", 15, FontStyle.Bold);
                            title.ForeColor = Color.Black;

                            Label buyerLabel = new Label();
                            buyerLabel.Text = "BUYER: " + buyerName.ToUpper();
                            buyerLabel.Top = 60;
                            buyerLabel.Left = 200;
                            buyerLabel.Width = 500;
                            buyerLabel.Height = 20;
                            buyerLabel.Font = new Font("Arial", 9, FontStyle.Bold);
                            buyerLabel.ForeColor = Color.Black;

                            Label price = new Label();
                            price.Text = "₱" + artworkPrice.ToString("0.00");
                            price.Top = 88;
                            price.Left = 200;
                            price.Width = 85;
                            price.Height = 28;
                            price.TextAlign = ContentAlignment.MiddleCenter;
                            price.Font = new Font("Arial", 11, FontStyle.Bold);
                            price.ForeColor = Color.White;
                            price.BackColor = Color.Gray;
                            MakeRounded(price, 12);

                            Label status = new Label();
                            status.Text = requestStatus.ToUpper();
                            status.Top = 92;
                            status.Left = 300;
                            status.Width = 180;
                            status.Height = 22;
                            status.Font = new Font("Arial", 9, FontStyle.Bold);
                            status.ForeColor = Color.Gray;

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
                                AcceptRequest(requestID, buyerID, currentUserID, artworkID, artworkPrice);
                            };

                            btnReject.Click += (s, e) =>
                            {
                                RejectRequest(requestID);
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
                }
            }

            if (requestCount == 0)
            {
                Label noRequests = new Label();
                noRequests.Text = "No pending purchase requests.";
                noRequests.Width = 300;
                noRequests.Height = 30;
                noRequests.Font = new Font("Segoe UI", 11, FontStyle.Regular);
                noRequests.ForeColor = Color.Gray;

                flowLayoutPanelRequests.Controls.Add(noRequests);
            }
        }

        private void AcceptRequest(int requestID, int buyerID, int sellerID, int artworkID, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string updateQuery = @"UPDATE PurchaseRequests 
                               SET Status = 'Accepted' 
                               WHERE RequestID = @RequestID";

                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@RequestID", requestID);
                    updateCmd.ExecuteNonQuery();
                }

                string transactionQuery = @"IF NOT EXISTS 
                                    (SELECT 1 FROM Transactions WHERE RequestID = @RequestID)
                                    BEGIN
                                        INSERT INTO Transactions 
                                        (RequestID, BuyerID, SellerID, ArtworkID, Amount)
                                        VALUES 
                                        (@RequestID, @BuyerID, @SellerID, @ArtworkID, @Amount)
                                    END";

                using (SqlCommand transactionCmd = new SqlCommand(transactionQuery, conn))
                {
                    transactionCmd.Parameters.AddWithValue("@RequestID", requestID);
                    transactionCmd.Parameters.AddWithValue("@BuyerID", buyerID);
                    transactionCmd.Parameters.AddWithValue("@SellerID", sellerID);
                    transactionCmd.Parameters.AddWithValue("@ArtworkID", artworkID);
                    transactionCmd.Parameters.AddWithValue("@Amount", amount);

                    transactionCmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Request accepted.\nTransaction has been recorded.");
            LoadRequests();
        }

        private void RejectRequest(int requestID)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
            {
                conn.Open();

                string updateQuery = "UPDATE PurchaseRequests SET Status = 'Rejected' WHERE RequestID = @RequestID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@RequestID", requestID);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Request rejected.");
            LoadRequests();
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
