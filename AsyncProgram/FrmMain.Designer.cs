namespace AsyncProgram
{
    partial class FrmMain
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
            this.btnAPM = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // btnAPM
            // 
            this.btnAPM.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAPM.BackColor = System.Drawing.Color.Transparent;
            this.btnAPM.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.btnAPM.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnAPM.Location = new System.Drawing.Point(7, 171);
            this.btnAPM.Name = "btnAPM";
            this.btnAPM.Size = new System.Drawing.Size(209, 78);
            this.btnAPM.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnAPM.TabIndex = 184;
            this.btnAPM.Text = "异步编程模型(Asynchronous Programming Model APM) ";
            this.btnAPM.Click += new System.EventHandler(this.btnAPM_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.BackColor = System.Drawing.Color.Transparent;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonX1.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonX1.Location = new System.Drawing.Point(7, 58);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(209, 78);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.buttonX1.TabIndex = 184;
            this.buttonX1.Text = "启动一个线程";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click_1);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(210)))), ((int)(((byte)(235)))));
            this.ClientSize = new System.Drawing.Size(850, 492);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.btnAPM);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnAPM;
        private DevComponents.DotNetBar.ButtonX buttonX1;
    }
}

