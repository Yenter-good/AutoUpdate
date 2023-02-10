using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MAutoUpdate
{
    public partial class UpdateForm : Form
    {
        public delegate void UpdateUI(int step);//声明一个更新主线程的委托
        public UpdateUI UpdateUIDelegate;

        public bool ShowVersionDesc { get; set; }

        private UpdateWork work;
        public UpdateForm(UpdateWork _work)
        {
            work = _work;
            InitializeComponent();

            UpdateUIDelegate = new UpdateUI((obj) =>
            {
                this.updateBar.Value = obj;
            });
            work.OnUpdateProgess += new UpdateWork.UpdateProgess((obj) =>
            {
                this.Invoke(UpdateUIDelegate, (int)obj);
            });

        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            if (ShowVersionDesc)
            {
                work.OnUpdateVersionChanged += new UpdateWork.UpdateVersionChanged((data) =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        if (data == null)
                            this.hideCursorRichTextBox1.Text = "";
                        else
                            this.hideCursorRichTextBox1.Text = data;
                    });
                });
            }
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                try
                {
                    work.OnUpdateStateChanged += new UpdateWork.UpdateStateChanged((data) =>
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.label1.Text = data;
                        });
                    });
                    work.UpdateCompleted += Work_UpdateCompleted;
                    work.Do();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.Message);
                }
            });
        }

        private void Work_UpdateCompleted(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.DialogResult = DialogResult.OK;
            });
        }
    }
}
