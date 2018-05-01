namespace VSMonoSDB
{
	partial class ConnectForm
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
			this.buttonsPanel = new System.Windows.Forms.Panel();
			this.btnOk = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tbIP = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.numPort = new System.Windows.Forms.NumericUpDown();
			this.button1 = new System.Windows.Forms.Button();
			this.buttonsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonsPanel
			// 
			this.buttonsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonsPanel.BackColor = System.Drawing.SystemColors.Control;
			this.buttonsPanel.Controls.Add(this.button1);
			this.buttonsPanel.Controls.Add(this.btnOk);
			this.buttonsPanel.Location = new System.Drawing.Point(-1, 83);
			this.buttonsPanel.Name = "buttonsPanel";
			this.buttonsPanel.Size = new System.Drawing.Size(439, 45);
			this.buttonsPanel.TabIndex = 6;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(339, 10);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(86, 27);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "Connect";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(19, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Target:";
			// 
			// tbIP
			// 
			this.tbIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbIP.Location = new System.Drawing.Point(74, 30);
			this.tbIP.Name = "tbIP";
			this.tbIP.Size = new System.Drawing.Size(257, 22);
			this.tbIP.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(336, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(11, 16);
			this.label2.TabIndex = 9;
			this.label2.Text = ":";
			// 
			// numPort
			// 
			this.numPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numPort.Location = new System.Drawing.Point(348, 30);
			this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numPort.Name = "numPort";
			this.numPort.Size = new System.Drawing.Size(68, 22);
			this.numPort.TabIndex = 10;
			this.numPort.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(246, 10);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(86, 27);
			this.button1.TabIndex = 1;
			this.button1.Text = "Cancel";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// ConnectForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(435, 127);
			this.Controls.Add(this.numPort);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbIP);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonsPanel);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "ConnectForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ConnectForm";
			this.Load += new System.EventHandler(this.ConnectForm_Load);
			this.buttonsPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel buttonsPanel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbIP;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numPort;
		private System.Windows.Forms.Button button1;
	}
}