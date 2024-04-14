using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dll_client
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "Справа расположен пример создаваемого графа.\n" +
                "Плитками с буквами характеризуются вершины графа, черные линии - это ребра графа.\n" +
                "Ребра графа необходимо рассматривать справа налево:\n" +
                "Первое ребро: C-B, второе: C-D и так далее";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
