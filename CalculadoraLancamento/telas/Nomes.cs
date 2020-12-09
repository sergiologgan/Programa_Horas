using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculadoraLancamento.telas
{
    public partial class Nomes : Form
    {
        public string receb;
        public Nomes(List<string> ls)
        {
            InitializeComponent();
            for (int i = 0; i < ls.Count; i++)
            {
                this.listBox1.Items.Add(ls[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(this.listBox1.SelectedIndex >= 0))
            {
                MessageBox.Show("Selecione pelo menos um nome");
                return;
            }
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.receb = this.listBox1.Items[this.listBox1.SelectedIndex].ToString();
        }
    }
}
