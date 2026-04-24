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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAgents)).BeginInit();
            this.SuspendLayout();
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_IP.Location = new System.Drawing.Point(12, 6);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(72, 17);
            this.label_IP.TabIndex = 0;
            this.label_IP.Text = "IP агента:";
            // 
            // textBox_ipAgent
            // 
            this.textBox_ipAgent.Location = new System.Drawing.Point(81, 6);
            this.textBox_ipAgent.Name = "textBox_ipAgent";
            this.textBox_ipAgent.Size = new System.Drawing.Size(95, 20);
            this.textBox_ipAgent.TabIndex = 1;
            // 
            // button_addAgent
            // 
            this.button_addAgent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_addAgent.Location = new System.Drawing.Point(13, 30);
            this.button_addAgent.Name = "button_addAgent";
            this.button_addAgent.Size = new System.Drawing.Size(164, 23);
            this.button_addAgent.TabIndex = 2;
            this.button_addAgent.Text = "Добавить агента";
            this.button_addAgent.UseVisualStyleBackColor = true;
            this.button_addAgent.Click += new System.EventHandler(this.button_addAgent_Click);
            // 
            // button_deleteAgents
            // 
            this.button_deleteAgents.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_deleteAgents.Location = new System.Drawing.Point(183, 30);
            this.button_deleteAgents.Name = "button_deleteAgents";
            this.button_deleteAgents.Size = new System.Drawing.Size(164, 23);
            this.button_deleteAgents.TabIndex = 4;
            this.button_deleteAgents.Text = "Удалить агента";
            this.button_deleteAgents.UseVisualStyleBackColor = true;
            this.button_deleteAgents.Click += new System.EventHandler(this.button_deleteAgents_Click);
            // 
            // dataGridViewAgents
            // 
            this.dataGridViewAgents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAgents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAgents.Location = new System.Drawing.Point(9, 75);
            this.dataGridViewAgents.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridViewAgents.MultiSelect = false;
            this.dataGridViewAgents.Name = "dataGridViewAgents";
            this.dataGridViewAgents.ReadOnly = true;
            this.dataGridViewAgents.RowHeadersWidth = 51;
            this.dataGridViewAgents.RowTemplate.Height = 24;
            this.dataGridViewAgents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAgents.Size = new System.Drawing.Size(338, 181);
            this.dataGridViewAgents.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Добавленные агенты";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(180, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Имя:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(215, 7);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(95, 20);
            this.textBoxName.TabIndex = 8;
            // 
            // Form_addAgents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 266);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewAgents);
            this.Controls.Add(this.button_deleteAgents);
            this.Controls.Add(this.button_addAgent);
            this.Controls.Add(this.textBox_ipAgent);
            this.Controls.Add(this.label_IP);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxName;
    }
}