namespace server_prototype
{
    partial class FormLogViewer
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabHardware = new System.Windows.Forms.TabPage();
            this.tabSoftware = new System.Windows.Forms.TabPage();
            this.tabProcesses = new System.Windows.Forms.TabPage();
            this.tabWarnings = new System.Windows.Forms.TabPage();
            this.txtSoftware = new System.Windows.Forms.TextBox();
            this.txtHardware = new System.Windows.Forms.TextBox();
            this.txtProcesses = new System.Windows.Forms.TextBox();
            this.txtWarnings = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabHardware.SuspendLayout();
            this.tabSoftware.SuspendLayout();
            this.tabProcesses.SuspendLayout();
            this.tabWarnings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabSoftware);
            this.tabControl1.Controls.Add(this.tabHardware);
            this.tabControl1.Controls.Add(this.tabProcesses);
            this.tabControl1.Controls.Add(this.tabWarnings);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(744, 402);
            this.tabControl1.TabIndex = 1;
            // 
            // tabHardware
            // 
            this.tabHardware.Controls.Add(this.txtHardware);
            this.tabHardware.Location = new System.Drawing.Point(4, 22);
            this.tabHardware.Name = "tabHardware";
            this.tabHardware.Padding = new System.Windows.Forms.Padding(3);
            this.tabHardware.Size = new System.Drawing.Size(736, 376);
            this.tabHardware.TabIndex = 0;
            this.tabHardware.Text = "Аппаратное обеспечение";
            this.tabHardware.UseVisualStyleBackColor = true;
            // 
            // tabSoftware
            // 
            this.tabSoftware.Controls.Add(this.txtSoftware);
            this.tabSoftware.Location = new System.Drawing.Point(4, 22);
            this.tabSoftware.Name = "tabSoftware";
            this.tabSoftware.Padding = new System.Windows.Forms.Padding(3);
            this.tabSoftware.Size = new System.Drawing.Size(736, 376);
            this.tabSoftware.TabIndex = 1;
            this.tabSoftware.Text = "Установленное ПО";
            this.tabSoftware.UseVisualStyleBackColor = true;
            // 
            // tabProcesses
            // 
            this.tabProcesses.Controls.Add(this.txtProcesses);
            this.tabProcesses.Location = new System.Drawing.Point(4, 22);
            this.tabProcesses.Name = "tabProcesses";
            this.tabProcesses.Size = new System.Drawing.Size(736, 376);
            this.tabProcesses.TabIndex = 2;
            this.tabProcesses.Text = "Процессы";
            this.tabProcesses.UseVisualStyleBackColor = true;
            // 
            // tabWarnings
            // 
            this.tabWarnings.Controls.Add(this.txtWarnings);
            this.tabWarnings.Location = new System.Drawing.Point(4, 22);
            this.tabWarnings.Name = "tabWarnings";
            this.tabWarnings.Size = new System.Drawing.Size(736, 376);
            this.tabWarnings.TabIndex = 3;
            this.tabWarnings.Text = "Предупреждение";
            this.tabWarnings.UseVisualStyleBackColor = true;
            // 
            // txtSoftware
            // 
            this.txtSoftware.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSoftware.Location = new System.Drawing.Point(3, 3);
            this.txtSoftware.Multiline = true;
            this.txtSoftware.Name = "txtSoftware";
            this.txtSoftware.ReadOnly = true;
            this.txtSoftware.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSoftware.Size = new System.Drawing.Size(730, 370);
            this.txtSoftware.TabIndex = 0;
            // 
            // txtHardware
            // 
            this.txtHardware.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHardware.Location = new System.Drawing.Point(3, 3);
            this.txtHardware.Multiline = true;
            this.txtHardware.Name = "txtHardware";
            this.txtHardware.ReadOnly = true;
            this.txtHardware.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHardware.Size = new System.Drawing.Size(730, 370);
            this.txtHardware.TabIndex = 1;
            // 
            // txtProcesses
            // 
            this.txtProcesses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProcesses.Location = new System.Drawing.Point(0, 0);
            this.txtProcesses.Multiline = true;
            this.txtProcesses.Name = "txtProcesses";
            this.txtProcesses.ReadOnly = true;
            this.txtProcesses.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProcesses.Size = new System.Drawing.Size(736, 376);
            this.txtProcesses.TabIndex = 2;
            // 
            // txtWarnings
            // 
            this.txtWarnings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWarnings.Location = new System.Drawing.Point(0, 0);
            this.txtWarnings.Multiline = true;
            this.txtWarnings.Name = "txtWarnings";
            this.txtWarnings.ReadOnly = true;
            this.txtWarnings.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWarnings.Size = new System.Drawing.Size(736, 376);
            this.txtWarnings.TabIndex = 3;
            // 
            // FormLogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 422);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormLogViewer";
            this.Text = "Полный лог";
            this.tabControl1.ResumeLayout(false);
            this.tabHardware.ResumeLayout(false);
            this.tabHardware.PerformLayout();
            this.tabSoftware.ResumeLayout(false);
            this.tabSoftware.PerformLayout();
            this.tabProcesses.ResumeLayout(false);
            this.tabProcesses.PerformLayout();
            this.tabWarnings.ResumeLayout(false);
            this.tabWarnings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabHardware;
        private System.Windows.Forms.TabPage tabSoftware;
        private System.Windows.Forms.TabPage tabProcesses;
        private System.Windows.Forms.TabPage tabWarnings;
        private System.Windows.Forms.TextBox txtSoftware;
        private System.Windows.Forms.TextBox txtHardware;
        private System.Windows.Forms.TextBox txtProcesses;
        private System.Windows.Forms.TextBox txtWarnings;
    }
}