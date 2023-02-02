using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace UpdateHelper
{
    public partial class Form1 : Form
    {
        private string _descriptionFileName;

        private decimal _currentVersionDate;

        private string _workDir;

        public Form1(string workDir)
        {
            InitializeComponent();
            _workDir = workDir;
        }

        public string ForceFlag { get; set; }
        public string DirectUpdate { get; set; }
        public string ReleaseVersion { get; set; }
        public string VersionDesc { get; set; }

        private void Init()
        {
            var info = this.GetServer();

            var version = info.ReleaseVersion.Split('.');
            this.numericUpDown1.Value = Convert.ToInt32(version[0]);
            this.numericUpDown2.Value = Convert.ToInt32(version[1]);
            this.numericUpDown3.Value = Convert.ToInt32(version[2]);
            this.numericUpDown4.Value = Convert.ToInt32(version[3]);

            _currentVersionDate = this.numericUpDown1.Value * 1000 + this.numericUpDown2.Value * 100 + this.numericUpDown3.Value * 10 + this.numericUpDown4.Value;

            var uri = new Uri(info.VersionDesc);
            _descriptionFileName = System.IO.Path.Combine(_workDir, Uri.UnescapeDataString(uri.Segments.Last()));

            this.textBox1.Text = System.IO.File.ReadAllText(_descriptionFileName, System.Text.Encoding.Default);
            this.checkBox1.Checked = info.ForceFlag != "0";
            this.checkBox2.Checked = info.DirectUpdate != "0";
        }

        /// <summary>
        /// 获取更新的服务器端的数据信息
        /// </summary>
        /// <param name="url">自动更新的URL信息</param>
        /// <returns></returns>
        private RemoteInfo GetServer()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(System.IO.Path.Combine(_workDir, "Server.xml"));
            var root = xdoc.DocumentElement;
            var listNodes = root.SelectNodes("/ServerUpdate/item");

            var node = listNodes[0];
            RemoteInfo remote = new RemoteInfo();
            foreach (XmlNode pItem in node.ChildNodes)
            {
                var property = remote.GetType().GetProperty(pItem.Name);
                if (property == null)
                    continue;
                property.SetValue(remote, pItem.InnerText, null);
            }

            return remote;
        }



        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var version = $"{this.numericUpDown1.Value}.{this.numericUpDown2.Value}.{this.numericUpDown3.Value}.{this.numericUpDown4.Value}";

            var versionDate = this.numericUpDown1.Value * 1000 + this.numericUpDown2.Value * 100 + this.numericUpDown3.Value * 10 + this.numericUpDown4.Value;

            if (versionDate <= this._currentVersionDate)
            {
                MessageBox.Show("版本号不能小于等于上一个版本号");
                return;
            }

            this.ReleaseVersion = version;
            this.ForceFlag = this.checkBox1.Checked ? "1" : "0";
            this.DirectUpdate = this.checkBox2.Checked ? "1" : "0";
            this.VersionDesc = this.textBox1.Text;

            this.DialogResult = DialogResult.OK;
        }
    }
}
