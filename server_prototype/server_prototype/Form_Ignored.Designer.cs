namespace server_prototype
{
    partial class Form_Ignored
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
            this.button_addIgnored = new System.Windows.Forms.Button();
            this.button_deleteIgnored = new System.Windows.Forms.Button();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.dataGridViewIgnored = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIgnored)).BeginInit();
            this.SuspendLayout();
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_IP.Location = new System.Drawing.Point(15, 11);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(102, 17);
            this.label_IP.TabIndex = 0;
            this.label_IP.Text = "IP устройства:";
            // 
            // button_addIgnored
            // 
            this.button_addIgnored.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_addIgnored.Location = new System.Drawing.Point(18, 43);
            this.button_addIgnored.Name = "button_addIgnored";
            this.button_addIgnored.Size = new System.Drawing.Size(160, 23);
            this.button_addIgnored.TabIndex = 1;
            this.button_addIgnored.Text = "Добавить устройство";
            this.button_addIgnored.UseVisualStyleBackColor = true;
            this.button_addIgnored.Click += new System.EventHandler(this.button_addIgnored_Click);
            // 
            // button_deleteIgnored
            // 
            this.button_deleteIgnored.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_deleteIgnored.Location = new System.Drawing.Point(184, 43);
            this.button_deleteIgnored.Name = "button_deleteIgnored";
            this.button_deleteIgnored.Size = new System.Drawing.Size(150, 23);
            this.button_deleteIgnored.TabIndex = 2;
            this.button_deleteIgnored.Text = "Удалить устройство";
            this.button_deleteIgnored.UseVisualStyleBackColor = true;
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(110, 11);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(111, 20);
            this.textBox_IP.TabIndex = 4;
            // 
            // dataGridViewIgnored
            // 
            this.dataGridViewIgnored.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewIgnored.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewIgnored.Location = new System.Drawing.Point(18, 88);
            this.dataGridViewIgnored.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridViewIgnored.MultiSelect = false;
            this.dataGridViewIgnored.Name = "dataGridViewIgnored";
            this.dataGridViewIgnored.ReadOnly = true;
            this.dataGridViewIgnored.RowHeadersWidth = 51;
            this.dataGridViewIgnored.RowTemplate.Height = 24;
            this.dataGridViewIgnored.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewIgnored.Size = new System.Drawing.Size(316, 123);
            this.dataGridViewIgnored.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(15, 69);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Список игнорируемых устройств";
            // 
            // Form_Ignored
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewIgnored);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.button_deleteIgnored);
            this.Controls.Add(this.button_addIgnored);
            this.Controls.Add(this.label_IP);
            this.Name = "Form_Ignored";
            this.Text = "Редактирование списка игнорируемых устройств";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIgnored)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_IP;
        private System.Windows.Forms.Button button_addIgnored;
        private System.Windows.Forms.Button button_deleteIgnored;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.DataGridView dataGridViewIgnored;
        private System.Windows.Forms.Label label1;
    }
}