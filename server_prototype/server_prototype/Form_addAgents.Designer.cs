namespace server_prototype
{
    partial class Form_addAgents
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
            this.label_IP = new System.Windows.Forms.Label();
            this.textBox_ipAgent = new System.Windows.Forms.TextBox();
            this.button_addAgent = new System.Windows.Forms.Button();
            this.button_deleteAgents = new System.Windows.Forms.Button();
            this.dataGridViewAgents = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAgents)).BeginInit();
            this.SuspendLayout();
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_IP.Location = new System.Drawing.Point(16, 7);
            this.label_IP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(92, 20);
            this.label_IP.TabIndex = 0;
            this.label_IP.Text = "IP агента:";
            // 
            // textBox_ipAgent
            // 
            this.textBox_ipAgent.Location = new System.Drawing.Point(108, 7);
            this.textBox_ipAgent.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_ipAgent.Name = "textBox_ipAgent";
            this.textBox_ipAgent.Size = new System.Drawing.Size(125, 22);
            this.textBox_ipAgent.TabIndex = 1;
            // 
            // button_addAgent
            // 
            this.button_addAgent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_addAgent.Location = new System.Drawing.Point(17, 37);
            this.button_addAgent.Margin = new System.Windows.Forms.Padding(4);
            this.button_addAgent.Name = "button_addAgent";
            this.button_addAgent.Size = new System.Drawing.Size(219, 28);
            this.button_addAgent.TabIndex = 2;
            this.button_addAgent.Text = "Добавить агента";
            this.button_addAgent.UseVisualStyleBackColor = true;
            this.button_addAgent.Click += new System.EventHandler(this.button_addAgent_Click);
            // 
            // button_deleteAgents
            // 
            this.button_deleteAgents.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_deleteAgents.Location = new System.Drawing.Point(244, 37);
            this.button_deleteAgents.Margin = new System.Windows.Forms.Padding(4);
            this.button_deleteAgents.Name = "button_deleteAgents";
            this.button_deleteAgents.Size = new System.Drawing.Size(219, 28);
            this.button_deleteAgents.TabIndex = 4;
            this.button_deleteAgents.Text = "Удалить агента";
            this.button_deleteAgents.UseVisualStyleBackColor = true;
            this.button_deleteAgents.Click += new System.EventHandler(this.button_deleteAgents_Click);
            // 
            // dataGridViewAgents
            // 
            this.dataGridViewAgents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAgents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAgents.Location = new System.Drawing.Point(12, 92);
            this.dataGridViewAgents.MultiSelect = false;
            this.dataGridViewAgents.Name = "dataGridViewAgents";
            this.dataGridViewAgents.ReadOnly = true;
            this.dataGridViewAgents.RowHeadersWidth = 51;
            this.dataGridViewAgents.RowTemplate.Height = 24;
            this.dataGridViewAgents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAgents.Size = new System.Drawing.Size(451, 223);
            this.dataGridViewAgents.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(16, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Добавленные агенты";
            // 
            // Form_addAgents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 327);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewAgents);
            this.Controls.Add(this.button_deleteAgents);
            this.Controls.Add(this.button_addAgent);
            this.Controls.Add(this.textBox_ipAgent);
            this.Controls.Add(this.label_IP);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form_addAgents";
            this.Text = "Добавление агента";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAgents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_IP;
        private System.Windows.Forms.TextBox textBox_ipAgent;
        private System.Windows.Forms.Button button_addAgent;
        private System.Windows.Forms.Button button_deleteAgents;
        private System.Windows.Forms.DataGridView dataGridViewAgents;
        private System.Windows.Forms.Label label1;
    }
}