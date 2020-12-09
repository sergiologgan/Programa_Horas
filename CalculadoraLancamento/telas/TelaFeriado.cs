using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CalculadoraLancamento.telas
{
    public partial class TelaFeriado : Form
    {
        public TelaFeriado()
        {
            InitializeComponent();
            try
            {
                this.numericUpDown1.Value = DateTime.Now.Year;                
                string[] lines = File.ReadAllLines("feriados.txt");
                string[] info = lines[0].Split(' ');
                if (info.Length == 2)
                {
                    this.comboBox1.Text = info[0];
                    this.numericUpDown1.Value = int.Parse(info[1]);
                }
                if (lines.Length >= 2)
                {
                    this.checkedListBox1.Items.Clear();
                    for (int i = 1; i < lines.Length; i++)
                    {
                        DateTime d = new DateTime((int)this.numericUpDown1.Value, ObterMes(this.comboBox1.Text), int.Parse(lines[i].Trim()));
                        this.checkedListBox1.Items.Add(string.Format("{0:D2} - {1}", lines[i], ObterSemana(d)), true);
                    }
                }
            }
            catch { }
        }

        private void AtualizarData()
        {
            try
            {
                int index = this.comboBox1.SelectedIndex;
                string item = this.comboBox1.Items[index].ToString();
                int mes = index + 1;
                if (this.numericUpDown1.Value <= 0)
                {
                    MessageBox.Show("Selecione um ano");
                    return;
                }
                this.checkedListBox1.Items.Clear();
                for (int i = 0; i < DateTime.DaysInMonth((int)this.numericUpDown1.Value, mes); i++)
                {
                    DateTime d = new DateTime((int)this.numericUpDown1.Value, this.comboBox1.SelectedIndex + 1, i + 1);
                    this.checkedListBox1.Items.Add(string.Format("{0:D2} - {1}", (i + 1).ToString(), ObterSemana(d)));
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.numericUpDown1.Value <= 0)
            {
                MessageBox.Show("Selecione um ano");
                return;
            }
            List<string> dias = new List<string>() { $"{this.comboBox1.Items[this.comboBox1.SelectedIndex]} {this.numericUpDown1.Value}" };            
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                if (this.checkedListBox1.GetItemChecked(i))
                {
                    dias.Add(this.checkedListBox1.Items[i].ToString().Split('-')[0]);
                }
            }
            File.WriteAllLines("feriados.txt", dias.ToArray());
            this.Close();
        }

        private string ObterSemana(DateTime data)
        {
            switch (data.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "Domingo";
                case DayOfWeek.Monday:
                    return "Segunda";
                case DayOfWeek.Tuesday:
                    return "Terça";
                case DayOfWeek.Wednesday:
                    return "Quarta";
                case DayOfWeek.Thursday:
                    return "Quinta";
                case DayOfWeek.Friday:
                    return "Sexta";
                case DayOfWeek.Saturday:
                    return "Sábado";
                default:
                    return null;
            }
        }

        private int ObterMes(string mes)
        {
            switch (mes)
            {
                case "Janeiro":
                    return 1;
                case "Fevereiro":
                    return 2;
                case "Março":
                    return 3;
                case "Abril":
                    return 4;
                case "Maio":
                    return 5;
                case "Junho":
                    return 6;
                case "Julho":
                    return 7;
                case "Agosto":
                    return 8;
                case "Setembro":
                    return 9;
                case "Outubro":
                    return 10;
                case "Novembro":
                    return 11;
                case "Dezembro":
                    return 12;
                default:
                    return -1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.button2.Text == "Marcar tudo")
            {
                this.button2.Text = "Desmarcar tudo";
                for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                {
                    this.checkedListBox1.SetItemChecked(i, true);
                }
            }
            else if (this.button2.Text == "Desmarcar tudo")
            {
                this.button2.Text = "Marcar tudo";
                for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                {
                    this.checkedListBox1.SetItemChecked(i, false);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AtualizarData();
        }
    }
}
