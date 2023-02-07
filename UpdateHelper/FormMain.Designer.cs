namespace UpdateHelper
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxVersionDescription = new System.Windows.Forms.TextBox();
            this.cbxForceUpdate = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbxDirectUpdate = new System.Windows.Forms.CheckBox();
            this.lvVersion = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "版本号:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Enabled = false;
            this.numericUpDown1.Location = new System.Drawing.Point(243, 28);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(35, 23);
            this.numericUpDown1.TabIndex = 1;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Enabled = false;
            this.numericUpDown2.Location = new System.Drawing.Point(284, 28);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(35, 23);
            this.numericUpDown2.TabIndex = 1;
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Enabled = false;
            this.numericUpDown3.Location = new System.Drawing.Point(325, 28);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(35, 23);
            this.numericUpDown3.TabIndex = 1;
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Enabled = false;
            this.numericUpDown4.Location = new System.Drawing.Point(366, 28);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(35, 23);
            this.numericUpDown4.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(276, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = ".";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(317, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = ".";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(358, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = ".";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(167, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 14);
            this.label4.TabIndex = 0;
            this.label4.Text = "更新文本:";
            // 
            // tbxVersionDescription
            // 
            this.tbxVersionDescription.Enabled = false;
            this.tbxVersionDescription.Location = new System.Drawing.Point(243, 59);
            this.tbxVersionDescription.Multiline = true;
            this.tbxVersionDescription.Name = "tbxVersionDescription";
            this.tbxVersionDescription.Size = new System.Drawing.Size(326, 139);
            this.tbxVersionDescription.TabIndex = 6;
            // 
            // cbxForceUpdate
            // 
            this.cbxForceUpdate.AutoSize = true;
            this.cbxForceUpdate.Checked = true;
            this.cbxForceUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxForceUpdate.Enabled = false;
            this.cbxForceUpdate.Location = new System.Drawing.Point(242, 204);
            this.cbxForceUpdate.Name = "cbxForceUpdate";
            this.cbxForceUpdate.Size = new System.Drawing.Size(82, 18);
            this.cbxForceUpdate.TabIndex = 7;
            this.cbxForceUpdate.Text = "强制更新";
            this.cbxForceUpdate.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(478, 233);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(91, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "增加新版本";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbxDirectUpdate
            // 
            this.cbxDirectUpdate.AutoSize = true;
            this.cbxDirectUpdate.Checked = true;
            this.cbxDirectUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxDirectUpdate.Enabled = false;
            this.cbxDirectUpdate.Location = new System.Drawing.Point(343, 204);
            this.cbxDirectUpdate.Name = "cbxDirectUpdate";
            this.cbxDirectUpdate.Size = new System.Drawing.Size(82, 18);
            this.cbxDirectUpdate.TabIndex = 7;
            this.cbxDirectUpdate.Text = "直接更新";
            this.cbxDirectUpdate.UseVisualStyleBackColor = true;
            // 
            // lvVersion
            // 
            this.lvVersion.Dock = System.Windows.Forms.DockStyle.Left;
            this.lvVersion.FullRowSelect = true;
            this.lvVersion.HideSelection = false;
            this.lvVersion.Location = new System.Drawing.Point(0, 0);
            this.lvVersion.MultiSelect = false;
            this.lvVersion.Name = "lvVersion";
            this.lvVersion.Size = new System.Drawing.Size(134, 284);
            this.lvVersion.TabIndex = 9;
            this.lvVersion.UseCompatibleStateImageBehavior = false;
            this.lvVersion.View = System.Windows.Forms.View.List;
            this.lvVersion.SelectedIndexChanged += new System.EventHandler(this.lvVersion_SelectedIndexChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 284);
            this.Controls.Add(this.lvVersion);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbxDirectUpdate);
            this.Controls.Add(this.cbxForceUpdate);
            this.Controls.Add(this.tbxVersionDescription);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "版本管理";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxVersionDescription;
        private System.Windows.Forms.CheckBox cbxForceUpdate;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox cbxDirectUpdate;
        private System.Windows.Forms.ListView lvVersion;
    }
}

