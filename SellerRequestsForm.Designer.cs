namespace PixMartt
{
    partial class SellerRequestsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.flowLayoutPanelRequests = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(197, 22);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(100, 13);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Purchase Requests";
            // 
            // flowLayoutPanelRequests
            // 
            this.flowLayoutPanelRequests.AutoScroll = true;
            this.flowLayoutPanelRequests.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelRequests.Location = new System.Drawing.Point(13, 67);
            this.flowLayoutPanelRequests.Name = "flowLayoutPanelRequests";
            this.flowLayoutPanelRequests.Size = new System.Drawing.Size(502, 310);
            this.flowLayoutPanelRequests.TabIndex = 1;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(222, 402);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // SellerRequestsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 450);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.flowLayoutPanelRequests);
            this.Controls.Add(this.lblHeader);
            this.Name = "SellerRequestsForm";
            this.Text = "SellerRequestsForm";
            this.Load += new System.EventHandler(this.SellerRequestsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelRequests;
        private System.Windows.Forms.Button btnBack;
    }
}