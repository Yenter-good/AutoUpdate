using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

namespace UpdateHelper
{
    public partial class FormMain : Form
    {
        private string _zipFileName;
        private FormSelectFile _formSelectFile;
        private BuildVersion _buildVersion;

        private string _workDir;

        private RemoteInfo _currentVersion;
        private List<RemoteInfo> _remoteInfos;
        private RemoteInfo _latestVersion;

        public FormMain(string[] args)
        {
            InitializeComponent();
            _formSelectFile = new FormSelectFile();
            _buildVersion = new BuildVersion();
            if (args != null && args.Any())
            {
                _formSelectFile.GetZipFileName(args);
                _zipFileName = _formSelectFile.ZipFileName;
            }
        }

        private void Init()
        {
            var workdir = System.Configuration.ConfigurationManager.AppSettings["workDir"];
            if (string.IsNullOrEmpty(workdir))
                _workDir = AppDomain.CurrentDomain.BaseDirectory;
            else
                _workDir = workdir;

            var infos = this.GetServer();
            _latestVersion = infos.FirstOrDefault();
            foreach (var info in infos)
            {
                var listviewItem = new ListViewItem($"{info.ReleaseVersion}");
                listviewItem.Tag = info;
                this.lvVersion.Items.Add(listviewItem);
            }

        }

        /// <summary>
        /// 获取更新的服务器端的数据信息
        /// </summary>
        /// <param name="url">自动更新的URL信息</param>
        /// <returns></returns>
        private List<RemoteInfo> GetServer()
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<RemoteInfo>));
            try
            {
                using (var stream = new FileStream(Path.Combine(_workDir, "Server.json"), FileMode.Open))
                    _remoteInfos = jsonSerializer.ReadObject(stream) as List<RemoteInfo>;
            }
            catch
            {
                _remoteInfos = null;
            }

            if (_remoteInfos == null)
                _remoteInfos = new List<RemoteInfo>();

            return _remoteInfos.OrderByDescending(p => p.AbsoluteValue).ToList();
        }



        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Init();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.btnOK.Text == "增加新版本")
            {
                if (_formSelectFile.ShowDialog() == DialogResult.OK)
                {
                    _zipFileName = _formSelectFile.ZipFileName;
                    this.btnOK.Text = "确定";

                    string[] version = new string[4];
                    if (_latestVersion != null)
                        version = _latestVersion.ReleaseVersion.Split('.');
                    else
                        version[0] = "1";

                    this.numericUpDown1.Value = Convert.ToInt32(version[0]);
                    this.numericUpDown2.Value = Convert.ToInt32(version[1]);
                    this.numericUpDown3.Value = Convert.ToInt32(version[2]);
                    this.numericUpDown4.Value = Convert.ToInt32(version[3]);

                    this.numericUpDown1.Enabled = true;
                    this.numericUpDown2.Enabled = true;
                    this.numericUpDown3.Enabled = true;
                    this.numericUpDown4.Enabled = true;

                    this.tbxVersionDescription.Enabled = true;

                    this.cbxForceUpdate.Enabled = true;
                    this.cbxDirectUpdate.Enabled = true;
                }
            }
            else
                NewVersion();
        }

        private void NewVersion()
        {
            var version = $"{this.numericUpDown1.Value}.{this.numericUpDown2.Value}.{this.numericUpDown3.Value}.{this.numericUpDown4.Value}";

            var forceFlag = this.cbxForceUpdate.Checked ? "1" : "0";
            var directUpdate = this.cbxDirectUpdate.Checked ? "1" : "0";

            var remoteInfo = new RemoteInfo()
            {
                ReleaseVersion = version,
                DirectUpdate = directUpdate,
                ForceFlag = forceFlag
            };

            if (_remoteInfos.Any(p => p.ReleaseVersion == version))
            {
                MessageBox.Show("版本号不能和历史版本重复");
                return;
            }

            remoteInfo = _buildVersion.Handler(_zipFileName, this.tbxVersionDescription.Text, remoteInfo);
            _remoteInfos.Add(remoteInfo);

            var listviewItem = new ListViewItem($"{version}");
            listviewItem.Tag = remoteInfo;
            this.lvVersion.Items.Insert(0, listviewItem);

            _currentVersion = remoteInfo;
            _latestVersion = remoteInfo;
            ShowVersion();
        }

        private void lvVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItems = this.lvVersion.SelectedItems;
            if (selectedItems.Count == 0)
                return;

            _currentVersion = selectedItems[0].Tag as RemoteInfo;

            ShowVersion();
        }

        private void ShowVersion()
        {
            var version = _currentVersion.ReleaseVersion.Split('.');
            this.numericUpDown1.Value = Convert.ToInt32(version[0]);
            this.numericUpDown2.Value = Convert.ToInt32(version[1]);
            this.numericUpDown3.Value = Convert.ToInt32(version[2]);
            this.numericUpDown4.Value = Convert.ToInt32(version[3]);

            var uri = new Uri(_currentVersion.VersionDesc);
            var descriptionFileName = Path.Combine(_workDir, _currentVersion.ReleaseVersion, Uri.UnescapeDataString(uri.Segments.Last()));

            if (File.Exists(descriptionFileName))
                this.tbxVersionDescription.Text = File.ReadAllText(descriptionFileName, System.Text.Encoding.Default);

            this.numericUpDown1.Enabled = false;
            this.numericUpDown2.Enabled = false;
            this.numericUpDown3.Enabled = false;
            this.numericUpDown4.Enabled = false;

            this.tbxVersionDescription.Enabled = false;

            this.cbxForceUpdate.Enabled = false;
            this.cbxDirectUpdate.Enabled = false;
            this.btnOK.Text = "增加新版本";
        }
    }
}
