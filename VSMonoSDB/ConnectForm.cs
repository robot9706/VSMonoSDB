using System;
using System.Net;
using System.Windows.Forms;

namespace VSMonoSDB
{
	public partial class ConnectForm : Form
	{
		private MainPackage _package;

		public IPEndPoint DebugTarget
		{
			get;
			private set;
		}

		public ConnectForm(MainPackage package)
		{
			InitializeComponent();

			_package = package;
		}

		private void ConnectForm_Load(object sender, EventArgs e)
		{
			Settings settings = _package.Settings;

			tbIP.Text = settings.LastRemoteIP;
			numPort.Value = settings.LastRemotePort;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			IPAddress ip;
			if (!IPAddress.TryParse(tbIP.Text, out ip))
			{
				MessageBox.Show(this, "Invalid IP address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			DebugTarget = new IPEndPoint(ip, (int)numPort.Value);

			Settings set = _package.Settings;
			set.LastRemoteIP = tbIP.Text;
			set.LastRemotePort = DebugTarget.Port;

			DialogResult = DialogResult.OK;
		}
	}
}
