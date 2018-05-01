namespace VSMonoSDB
{
	partial class SettingsForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.numDebugPort = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.tbMonoPath = new System.Windows.Forms.TextBox();
			this.btnMonoBrowse = new System.Windows.Forms.Button();
			this.buttonsPanel = new System.Windows.Forms.Panel();
			this.btnOk = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numDebugPort)).BeginInit();
			this.buttonsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(55, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Port:";
			// 
			// numDebugPort
			// 
			this.numDebugPort.Location = new System.Drawing.Point(95, 16);
			this.numDebugPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.numDebugPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numDebugPort.Name = "numDebugPort";
			this.numDebugPort.Size = new System.Drawing.Size(143, 22);
			this.numDebugPort.TabIndex = 1;
			this.numDebugPort.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(19, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(74, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Mono path:";
			// 
			// tbMonoPath
			// 
			this.tbMonoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMonoPath.BackColor = System.Drawing.Color.White;
			this.tbMonoPath.Location = new System.Drawing.Point(95, 44);
			this.tbMonoPath.Name = "tbMonoPath";
			this.tbMonoPath.ReadOnly = true;
			this.tbMonoPath.Size = new System.Drawing.Size(210, 22);
			this.tbMonoPath.TabIndex = 3;
			// 
			// btnMonoBrowse
			// 
			this.btnMonoBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMonoBrowse.Location = new System.Drawing.Point(311, 44);
			this.btnMonoBrowse.Name = "btnMonoBrowse";
			this.btnMonoBrowse.Size = new System.Drawing.Size(75, 22);
			this.btnMonoBrowse.TabIndex = 4;
			this.btnMonoBrowse.Text = "Browse";
			this.btnMonoBrowse.UseVisualStyleBackColor = true;
			this.btnMonoBrowse.Click += new System.EventHandler(this.btnMonoBrowse_Click);
			// 
			// buttonsPanel
			// 
			this.buttonsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonsPanel.BackColor = System.Drawing.SystemColors.Control;
			this.buttonsPanel.Controls.Add(this.btnOk);
			this.buttonsPanel.Location = new System.Drawing.Point(0, 87);
			this.buttonsPanel.Name = "buttonsPanel";
			this.buttonsPanel.Size = new System.Drawing.Size(414, 45);
			this.buttonsPanel.TabIndex = 5;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(306, 8);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(86, 27);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// SettingsForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(409, 129);
			this.Controls.Add(this.buttonsPanel);
			this.Controls.Add(this.btnMonoBrowse);
			this.Controls.Add(this.tbMonoPath);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.numDebugPort);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "SettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Debugger settings";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.numDebugPort)).EndInit();
			this.buttonsPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numDebugPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbMonoPath;
		private System.Windows.Forms.Button btnMonoBrowse;
		private System.Windows.Forms.Panel buttonsPanel;
		private System.Windows.Forms.Button btnOk;
	}
}