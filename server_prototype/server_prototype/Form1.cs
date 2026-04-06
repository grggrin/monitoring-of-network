using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server_prototype
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_Agent_Click(object sender, EventArgs e)
        {
            Form_addAgents newForm = new Form_addAgents();
            newForm.Show();
        }

        private void button_Ignored_Click(object sender, EventArgs e)
        {
            Form_Ignored newForm = new Form_Ignored();
            newForm.Show();
        }

        private void button_Authorised_Click(object sender, EventArgs e)
        {
            Form_addAuthorised newForm = new Form_addAuthorised();
            newForm.Show();
        }


        private void button_agentLog_Click(object sender, EventArgs e)
        {
            Form_agentLog newForm = new Form_agentLog();
            newForm.Show();
        }

        private void button_monitoringLog_Click(object sender, EventArgs e)
        {
            Form_agentLog newForm = new Form_agentLog();
            newForm.Show();
        }

        private void button_monitor_Click(object sender, EventArgs e)
        {
            Form_monitoring newForm = new Form_monitoring();
            newForm.Show();
        }
    }
}
