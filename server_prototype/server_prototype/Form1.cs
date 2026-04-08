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
            // Form_addAgents newForm = new Form_addAgents();
            // Проверяем, есть ли уже открытая форма Form_addAgents
            Form_addAgents existingForm = Application.OpenForms
                .OfType<Form_addAgents>()
                .FirstOrDefault();

            if (existingForm == null)
            {
                // Форма не найдена — создаём новую
                new Form_addAgents().Show();
            }
            else
            {
                // Форма уже есть — просто активируем
                existingForm.Activate();
            }
            // newForm.Show();
        }


        private void button_Ignored_Click(object sender, EventArgs e)
        {
            Form_Ignored existingForm = Application.OpenForms
                .OfType<Form_Ignored>()
                .FirstOrDefault();
            if (existingForm == null)
            {
                new Form_Ignored().Show();
            }
            else
            {
                existingForm.Activate();
            }
        }

        private void button_Authorised_Click(object sender, EventArgs e)
        {
            
            Form_addAuthorised existingForm = Application.OpenForms
                .OfType<Form_addAuthorised>()
                .FirstOrDefault();
            if (existingForm == null)
            {
                new Form_addAuthorised().Show();
            }
            else
            {
                existingForm.Activate();
            }
        }


        private void button_agentLog_Click(object sender, EventArgs e)
        {
           
            Form_agentLog existingForm = Application.OpenForms
                .OfType<Form_agentLog>()
                .FirstOrDefault();
            if (existingForm == null)
            {
                new Form_agentLog().Show();
            }
            else
            {
                existingForm.Activate();
            }
        }

        private void button_monitoringLog_Click(object sender, EventArgs e)
        {
         
            Form_agentLog existingForm = Application.OpenForms
                .OfType<Form_agentLog>()
                .FirstOrDefault();
            if (existingForm == null)
            {
                new Form_agentLog().Show();
            }
            else
            {
                existingForm.Activate();
            }
        }

        /*private void button_monitor_Click(object sender, EventArgs e)
        {
            
            Form_monitoring existingForm = Application.OpenForms
                .OfType<Form_monitoring>()
                .FirstOrDefault();
            if (existingForm == null)
            {
                new Form_monitoring().Show();
            }
            else
            {
                existingForm.Activate();
            }
        }*/

        private void button_monitor_Click(object sender, EventArgs e)
        {
            var existingForm = Application.OpenForms.OfType<Form_monitoring>().FirstOrDefault();
            if (existingForm == null)
            {
                new Form_monitoring().Show();
            }
            else
            {
                existingForm.Activate();
            }
        }
    }
}
