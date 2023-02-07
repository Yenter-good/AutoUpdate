using Ionic.Zip;
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
    public partial class FormSelectFile : Form
    {

        public string ZipFileName { get; private set; }

        public FormSelectFile()
        {
            InitializeComponent();
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                var fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
                this.GetZipFileName(fileNames);
            }
        }

        public void GetZipFileName(string[] fileNames)
        {
            var zipPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp.zip");

            if (fileNames.Length == 1 && Path.GetExtension(fileNames[0]).ToLower() == ".zip")
            {
                ZipFileName = fileNames[0];
                this.DialogResult = DialogResult.OK;
                return;
            }


            using (var zip = new ZipFile(Encoding.UTF8))
            {
                foreach (var fileName in fileNames)
                {
                    var file = new FileInfo(fileName);
                    if ((file.Attributes & FileAttributes.Directory) != 0)
                        zip.AddDirectory(fileName, file.Name);
                    else
                        zip.AddFile(fileName, "");
                }
                zip.Save(zipPath);
            }

            ZipFileName = zipPath;
            this.DialogResult = DialogResult.OK;
        }

        private void label1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {

        }
    }
}
