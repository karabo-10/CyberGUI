namespace PROG_Part2_C_
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            rtbChat = new RichTextBox();
            txtInput = new TextBox();
            btnSend = new Button();
            btnClear = new Button();
            lblTitle = new Label();
            lblSubtitle = new Label();
            pnlTop = new Panel();
            pnlBottom = new Panel();
            pnlTop.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // rtbChat
            // 
            rtbChat.BackColor = Color.FromArgb(18, 18, 32);
            rtbChat.BorderStyle = BorderStyle.None;
            rtbChat.Dock = DockStyle.Fill;
            rtbChat.Font = new Font("Consolas", 10F);
            rtbChat.ForeColor = Color.FromArgb(200, 220, 255);
            rtbChat.Location = new Point(0, 70);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbChat.Size = new Size(942, 523);
            rtbChat.TabIndex = 0;
            rtbChat.Text = "";
            // 
            // txtInput
            // 
            txtInput.BackColor = Color.FromArgb(28, 28, 48);
            txtInput.BorderStyle = BorderStyle.FixedSingle;
            txtInput.Dock = DockStyle.Fill;
            txtInput.Font = new Font("Consolas", 11F);
            txtInput.ForeColor = Color.White;
            txtInput.Location = new Point(10, 8);
            txtInput.Name = "txtInput";
            txtInput.PlaceholderText = "Type your message here and press Enter or Send...";
            txtInput.Size = new Size(722, 29);
            txtInput.TabIndex = 0;
            txtInput.Text = "ar";
            txtInput.KeyDown += txtInput_KeyDown;
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.FromArgb(0, 150, 110);
            btnSend.Cursor = Cursors.Hand;
            btnSend.Dock = DockStyle.Right;
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Consolas", 10F, FontStyle.Bold);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(732, 8);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(120, 44);
            btnSend.TabIndex = 1;
            btnSend.Text = "Send  ➤";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(55, 20, 20);
            btnClear.Cursor = Cursors.Hand;
            btnClear.Dock = DockStyle.Right;
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Consolas", 10F);
            btnClear.ForeColor = Color.FromArgb(255, 90, 90);
            btnClear.Location = new Point(852, 8);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(80, 44);
            btnClear.TabIndex = 2;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Consolas", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 210, 160);
            lblTitle.Location = new Point(16, 8);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(197, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🛡  CyberBot";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Consolas", 9F);
            lblSubtitle.ForeColor = Color.FromArgb(140, 140, 180);
            lblSubtitle.Location = new Point(20, 40);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(272, 18);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "CyberSecurity Awareness Assistant";
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(8, 8, 18);
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(lblSubtitle);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(16, 6, 0, 0);
            pnlTop.Size = new Size(942, 70);
            pnlTop.TabIndex = 2;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.FromArgb(8, 8, 18);
            pnlBottom.Controls.Add(txtInput);
            pnlBottom.Controls.Add(btnSend);
            pnlBottom.Controls.Add(btnClear);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 593);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Padding = new Padding(10, 8, 10, 8);
            pnlBottom.Size = new Size(942, 60);
            pnlBottom.TabIndex = 1;
            // 
            // Form1
            // 
            BackColor = Color.FromArgb(15, 15, 25);
            ClientSize = new Size(942, 653);
            Controls.Add(rtbChat);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Font = new Font("Consolas", 10F);
            MinimumSize = new Size(700, 500);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CyberBot — CyberSecurity Awareness Bot";
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
        }

        // ── Controls ──────────────────────────────────────────────────────
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.TextBox     txtInput;
        private System.Windows.Forms.Button      btnSend;
        private System.Windows.Forms.Button      btnClear;
        private System.Windows.Forms.Label       lblTitle;
        private System.Windows.Forms.Label       lblSubtitle;
        private System.Windows.Forms.Panel       pnlTop;
        private System.Windows.Forms.Panel       pnlBottom;
    }
}
