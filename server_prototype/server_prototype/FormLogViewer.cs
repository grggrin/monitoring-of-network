using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace server_prototype
{
    public partial class FormLogViewer : Form
    {
        public FormLogViewer(string text)
        {
            InitializeComponent();

            txtHardware.Text = GetHardware(text);
            txtSoftware.Text = GetSoftware(text);
            txtProcesses.Text = "Информация о процессах отсутствует";
            txtWarnings.Text = GetWarnings(text);
        }
        private string GetHardware(string text)
        {
            return ExtractSection(
                text,
                "HARDWARE:",
                "DISK USAGE:");
        }
        private string GetSoftware(string text)
        {
            return ExtractSection(
                text,
                "INSTALLED PROGRAMS:",
                "Total programs:");
        }
        private string GetWarnings(string logText)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string line in logText.Split('\n'))
            {
                if (line.Contains("WARNING"))
                    sb.AppendLine(line);
            }

            return sb.Length == 0
                ? "Предупреждения отсутствуют"
                : sb.ToString();
        }

        private string ExtractSection(string text,string startMarker,string endMarker)
        {
            int start = text.IndexOf(startMarker);

            if (start < 0)
                return "Нет данных";

            start += startMarker.Length;

            int end = text.IndexOf(endMarker, start);

            if (end < 0)
                end = text.Length;

            return text.Substring(start, end - start).Trim();
        }

    }
}
