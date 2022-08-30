using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace UpdateHelper
{
    public partial class FormSelectFile : Form
    {
        private string _selectedFileName;

        private string _workDir = "";
        private string _backupDir = "";

        public FormSelectFile(string[] args)
        {
            InitializeComponent();

            var workdir = System.Configuration.ConfigurationManager.AppSettings["workDir"];
            if (string.IsNullOrEmpty(workdir))
                _workDir = AppDomain.CurrentDomain.BaseDirectory;
            else
                _workDir = workdir;

            var backupDir = System.Configuration.ConfigurationManager.AppSettings["backupDir"];
            if (string.IsNullOrEmpty(backupDir))
                _backupDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backup");
            else
            {
                if (backupDir == workdir)
                    _backupDir = Path.Combine(backupDir, "backup");
                else
                    _backupDir = backupDir;
            }

            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                    this.GetServerInfo(args);
            }
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                var fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
                this.GetServerInfo(fileNames);
            }
        }

        private void GetServerInfo(string[] fileNames)
        {
            if (fileNames.Length > 0)
            {
                _selectedFileName = fileNames[0];
                var ext = System.IO.Path.GetExtension(_selectedFileName);
                if (ext.ToLower() != ".zip")
                {
                    MessageBox.Show("请传入zip压缩包");
                    return;
                }

                Form1 form = new Form1(_workDir);
                this.Hide();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.MoveOldFile();
                    this.CreateNewFile(form.ReleaseVersion, form.VersionDesc, form.ForceFlag);
                }
                this.Show();
            }
        }

        private void MoveOldFile()
        {
            if (!Directory.Exists(_backupDir))
                Directory.CreateDirectory(_backupDir);

            var dateFolder = Path.Combine(_backupDir, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(dateFolder))
                Directory.CreateDirectory(dateFolder);

            var timeFolder = Path.Combine(dateFolder, DateTime.Now.ToString("HHmmss"));
            if (!Directory.Exists(timeFolder))
                Directory.CreateDirectory(timeFolder);

            var fileNames = Directory.GetFiles(_workDir);
            foreach (var fileName in fileNames)
            {
                var tmp = Path.GetFileName(fileName);
                if (tmp == "最新版本.zip" || tmp == "最新版本更新说明.txt")
                {
                    try
                    {
                        File.Move(fileName, Path.Combine(timeFolder, tmp));
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void CreateNewFile(string version, string desc, string forceFlag)
        {
            File.Move(_selectedFileName, Path.Combine(_workDir, "最新版本.zip"));
            File.WriteAllText(Path.Combine(_workDir, "最新版本更新说明.txt"), desc, Encoding.Default);

            var xml = this.SetServer(version, desc, forceFlag);

            File.WriteAllText(Path.Combine(_workDir, "Server.xml"), xml);
        }

        private string SetServer(string version, string desc, string forceFlag)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(System.IO.Path.Combine(_workDir, "Server.xml"));
            var root = xdoc.DocumentElement;
            var listNodes = root.SelectNodes("/ServerUpdate/item");

            var node = listNodes[0];

            node.SelectSingleNode("ReleaseVersion").InnerText = version;
            node.SelectSingleNode("ForceFlag").InnerText = forceFlag;

            return xdoc.OuterXml;
        }

        private void label1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }


    }
}
