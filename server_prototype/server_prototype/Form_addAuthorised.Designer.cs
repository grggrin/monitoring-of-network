namespace server_prototype
{
    partial class Form_addAuthorised
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
            this.label_ip = new System.Windows.Forms.Label();
            this.textBox_ipAuthorised = new System.Windows.Forms.TextBox();
            this.button_addAuthorised = new System.Windows.Forms.Button();
            this.button_deleteAuthorised = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewAuthorised = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAuthorised)).BeginInit();
            this.SuspendLayout();
            // 
            // label_ip
            // 
            this.label_ip.AutoSize = true;
            this.label_ip.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_ip.Location = new System.Drawing.Point(17, 16);
            this.label_ip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_ip.Name = "label_ip";
            this.label_ip.Size = new System.Drawing.Size(130, 20);
            this.label_ip.TabIndex = 0;
            this.label_ip.Text = "IP устройства:";
            // 
            // textBox_ipAuthorised
            // 
            this.textBox_ipAuthorised.Location = new System.Drawing.Point(144, 16);
            this.textBox_ipAuthorised.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_ipAuthorised.Name = "textBox_ipAuthorised";
            this.textBox_ipAuthorised.Size = new System.Drawing.Size(83, 22);
            this.textBox_ipAuthorised.TabIndex = 1;
            // 
            // button_addAuthorised
            // 
            this.button_addAuthorised.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_addAuthorised.Location = new System.Drawing.Point(17, 48);
            this.button_addAuthorised.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_addAuthorised.Name = "button_addAuthorised";
            this.button_addAuthorised.Size = new System.Drawing.Size(211, 28);
            this.button_addAuthorised.TabIndex = 2;
            this.button_addAuthorised.Text = "Добавить устройство";
            this.button_addAuthorised.UseVisualStyleBackColor = true;
            this.button_addAuthorised.Click += new System.EventHandler(this.button_addAuthorised_Click);
            // 
            // button_deleteAuthorised
            // 
            this.button_deleteAuthorised.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_deleteAuthorised.Location = new System.Drawing.Point(236, 48);
            this.button_deleteAuthorised.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_deleteAuthorised.Name = "button_deleteAuthorised";
            this.button_deleteAuthorised.Size = new System.Drawing.Size(200, 28);
            this.button_deleteAuthorised.TabIndex = 3;
            this.button_deleteAuthorised.Text = "Удалить устройство";
            this.button_deleteAuthorised.UseVisualStyleBackColor = true;
            this.button_deleteAuthorised.Click += new System.EventHandler(this.button_deleteAuthorised_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(17, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(324, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Список авторизированных устройств";
            // 
            // dataGridViewAuthorised
            // 
            this.dataGridViewAuthorised.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAuthorised.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAuthorised.Location = new System.Drawing.Point(17, 107);
            this.dataGridViewAuthorised.MultiSelect = false;
            this.dataGridViewAuthorised.Name = "dataGridViewAuthorised";
            this.dataGridViewAuthorised.ReadOnly = true;
            this.dataGridViewAuthorised.RowHeadersWidth = 51;
            this.dataGridViewAuthorised.RowTemplate.Height = 24;
            this.dataGridViewAuthorised.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAuthorised.Size = new System.Drawing.Size(419, 150);
            this.dataGridViewAuthorised.TabIndex = 5;
            // 
            // Form_addAuthorised
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 554);
            this.Controls.Add(this.dataGridViewAuthorised);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_deleteAuthorised);
            this.Controls.Add(this.button_addAuthorised);
            this.Controls.Add(this.textBox_ipAuthorised);
            this.Controls.Add(this.label_ip);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form_addAuthorised";
            this.Text = "Редактирование списка авторизированных устройств";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAuthorised)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_ip;
        private System.Windows.Forms.TextBox textBox_ipAuthorised;
        private System.Windows.Forms.Button button_addAuthorised;
        private System.Windows.Forms.Button button_deleteAuthorised;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewAuthorised;
    }
}