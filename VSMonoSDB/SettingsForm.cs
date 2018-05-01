using System;
using System.IO;
using System.Windows.Forms;

namespace VSMonoSDB
{
	public partial class SettingsForm : Form
	{
		private Settings _settings;

		public SettingsForm(MainPackage package)
		{
			InitializeComponent();

			_settings = package.Settings;
		}

		private void SettingsForm_Load(object sender, EventArgs e)
		{
			numDebugPort.Value = _settings.DebugPort;
			tbMonoPath.Text = _settings.MonoPath;
		}

		private void btnMonoBrowse_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog fbd = new FolderBrowserDialog())
			{
				if (!string.IsNullOrEmpty(tbMonoPath.Text) && Directory.Exists(tbMonoPath.Text))
				{
					fbd.SelectedPath = tbMonoPath.Text;
				}

				if (fbd.ShowDialog() == DialogResult.OK)
				{
					string binFolder = Path.Combine(fbd.SelectedPath, "bin");
					string etcFolder = Path.Combine(fbd.SelectedPath, "etc");

					if (!Directory.Exists(binFolder))
					{
						MessageBox.Show(this, "\"bin\" folder not found! \"" + binFolder + "\"", "Invalid Mono folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					if (!Directory.Exists(etcFolder))
					{
						MessageBox.Show(this, "\"etc\" folder not found! \"" + etcFolder + "\"", "Invalid Mono folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					tbMonoPath.Text = fbd.SelectedPath;
					_settings.MonoPath = fbd.SelectedPath;
				}
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			_settings.DebugPort = (int)numDebugPort.Value;

			DialogResult = DialogResult.OK;
		}
	}
}
