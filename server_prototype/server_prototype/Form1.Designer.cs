namespace server_prototype
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_monitor = new System.Windows.Forms.Button();
            this.button_Authorised = new System.Windows.Forms.Button();
            this.button_Agent = new System.Windows.Forms.Button();
            this.button_Ignored = new System.Windows.Forms.Button();
            this.button_monitoringLog = new System.Windows.Forms.Button();
            this.button_agentLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_monitor
            // 
            this.button_monitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_monitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_monitor.Location = new System.Drawing.Point(11, 150);
            this.button_monitor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_monitor.Name = "button_monitor";
            this.button_monitor.Size = new System.Drawing.Size(253, 28);
            this.button_monitor.TabIndex = 1;
            this.button_monitor.Text = "Запустить мониторинг";
            this.button_monitor.UseVisualStyleBackColor = true;
            this.button_monitor.Click += new System.EventHandler(this.button_monitor_Click);
            // 
            // button_Authorised
            // 
            this.button_Authorised.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Authorised.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Authorised.Location = new System.Drawing.Point(9, 11);
            this.button_Authorised.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Authorised.Name = "button_Authorised";
            this.button_Authorised.Size = new System.Drawing.Size(255, 49);
            this.button_Authorised.TabIndex = 2;
            this.button_Authorised.Text = "Редактирование списка авторизированных устройств";
            this.button_Authorised.UseVisualStyleBackColor = true;
            this.button_Authorised.Click += new System.EventHandler(this.button_Authorised_Click);
            // 
            // button_Agent
            // 
            this.button_Agent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Agent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Agent.Location = new System.Drawing.Point(11, 114);
            this.button_Agent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Agent.Name = "button_Agent";
            this.button_Agent.Size = new System.Drawing.Size(253, 32);
            this.button_Agent.TabIndex = 3;
            this.button_Agent.Text = "Редактирование списка агентов";
            this.button_Agent.UseVisualStyleBackColor = true;
            this.button_Agent.Click += new System.EventHandler(this.button_Agent_Click);
            // 
            // button_Ignored
            // 
            this.button_Ignored.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Ignored.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Ignored.Location = new System.Drawing.Point(9, 64);
            this.button_Ignored.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Ignored.Name = "button_Ignored";
            this.button_Ignored.Size = new System.Drawing.Size(255, 46);
            this.button_Ignored.TabIndex = 4;
            this.button_Ignored.Text = "Редактирование списка игнорируемых устройств";
            this.button_Ignored.UseVisualStyleBackColor = true;
            this.button_Ignored.Click += new System.EventHandler(this.button_Ignored_Click);
            // 
            // button_monitoringLog
            // 
            this.button_monitoringLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_monitoringLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_monitoringLog.Location = new System.Drawing.Point(9, 212);
            this.button_monitoringLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_monitoringLog.Name = "button_monitoringLog";
            this.button_monitoringLog.Size = new System.Drawing.Size(253, 28);
            this.button_monitoringLog.TabIndex = 5;
            this.button_monitoringLog.Text = "Лог мониторинга";
            this.button_monitoringLog.UseVisualStyleBackColor = true;
            this.button_monitoringLog.Click += new System.EventHandler(this.button_monitoringLog_Click);
            // 
            // button_agentLog
            // 
            this.button_agentLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_agentLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_agentLog.Location = new System.Drawing.Point(10, 181);
            this.button_agentLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_agentLog.Name = "button_agentLog";
            this.button_agentLog.Size = new System.Drawing.Size(253, 28);
            this.button_agentLog.TabIndex = 6;
            this.button_agentLog.Text = "Логи с агентов";
            this.button_agentLog.UseVisualStyleBackColor = true;
            this.button_agentLog.Click += new System.EventHandler(this.button_agentLog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 252);
            this.Controls.Add(this.button_agentLog);
            this.Controls.Add(this.button_monitoringLog);
            this.Controls.Add(this.button_Ignored);
            this.Controls.Add(this.button_Agent);
            this.Controls.Add(this.button_Authorised);
            this.Controls.Add(this.button_monitor);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_monitor;
        private System.Windows.Forms.Button button_Authorised;
        private System.Windows.Forms.Button button_Agent;
        private System.Windows.Forms.Button button_Ignored;
        private System.Windows.Forms.Button button_monitoringLog;
        private System.Windows.Forms.Button button_agentLog;
    }
}

