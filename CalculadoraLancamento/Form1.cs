using CalculadoraLancamento.imprimir;
using CalculadoraLancamento.leitura;
using CalculadoraLancamento.modelar;
using CalculadoraLancamento.telas;
using CalculadoraLancamento.variaveis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CalculadoraLancamento
{
    public enum PeriodosCompletos { P1_ATIVA, P1_STAND, P2_ATIVA, P2_STAND, P3_ATIVA, P3_STAND, P4_ATIVA, P4_STAND }
    public enum PeriodosSimples { P1, P2, P3, P4 }
    public enum TipoPeriodo { ATIVA, STAND }
    public enum TipoDePlantao { NORMAL, PLANTONISTA }

    public partial class Form1 : Form
    {
        private string path;
        private string salvar;
        private Dictionary<string, List<HorasLancamento>> lancamentos;
        public Form1()
        {
            InitializeComponent();
            OrdenarControlesRecentes();
        }

        #region metodos parametros
        private string ResumoHoras()
        {
            var t = ModelarDados.UnificarPeriodoPorPessoa(ModelarDados.ObterLancamentosSemHorasZeradas(this.lancamentos));

            List<string> lines = new List<string>()
            {
                "<tr>",
                "<td>&nbsp;</td>",
                "<td>P1</td>",
                "<td>P2</td>",
                "<td>P3</td>",
                "<td>P4</td>",
                "<td>Total</td>",
                "</tr>"
            };
            TimeSpan tp1 = new TimeSpan();
            TimeSpan tp2 = new TimeSpan();
            TimeSpan tp3 = new TimeSpan();
            TimeSpan tp4 = new TimeSpan();
            TimeSpan tt = new TimeSpan();

            foreach (string nome in t.Keys)
            {
                Dictionary<PeriodosSimples, TimeSpan> periodos = t[nome];
                string[] colunas = new string[6] { "<td></td>", "<td></td>", "<td></td>", "<td></td>", "<td></td>", "<td></td>" };
                colunas[0] = $"<td>{nome}</td>";
                foreach (PeriodosSimples p in periodos.Keys)
                {
                    TimeSpan time = periodos[p];
                    switch (p)
                    {
                        case PeriodosSimples.P1:
                            colunas[1] = $"<td>{ArredondarTempo(time)}</td>";
                            tp1 += time;
                            break;
                        case PeriodosSimples.P2:
                            colunas[2] = $"<td>{ArredondarTempo(time)}</td>";
                            tp2 += time;
                            break;
                        case PeriodosSimples.P3:
                            colunas[3] = $"<td>{ArredondarTempo(time)}</td>";
                            tp3 += time;
                            break;
                        case PeriodosSimples.P4:
                            colunas[4] = $"<td>{ArredondarTempo(time)}</td>";
                            tp4 += time;
                            break;
                        default:
                            break;
                    }
                }
                TimeSpan total = periodos.Sum(x => x.Value.Duration());
                tt += total;
                colunas[5] = $"<td>{ArredondarTempo(total)}</td>";
                lines.Add("<tr>");
                lines.AddRange(colunas.ToList());
                lines.Add("</tr>");
            }
            string bs = File.ReadAllText("base.txt");
            List<string> ls = new List<string>()
            {
                bs,
                "<div class=\"box\">",
                "<h2>Tempo dos períodos</h2>",
                "<table>"
            };
            lines.AddRange(new List<string>()
                {
                    "<tr>",
                    "<td>Total</td>",
                    $"<td>{ArredondarTempo(tp1)}</td>",
                    $"<td>{ArredondarTempo(tp2)}</td>",
                    $"<td>{ArredondarTempo(tp3)}</td>",
                    $"<td>{ArredondarTempo(tp4)}</td>",
                    $"<td>{ArredondarTempo(tt)}</td>",
                    "</tr>"
                });
            ls.AddRange(lines);
            ls.Add("</table>");
            ls.Add("</div>");

            return ArrayParaString(ls);
        }

        private string ResumoHorasQtdRequisicao()
        {
            var t = ModelarDados.UnificaQtdDiaRequisicaoPorPessoa(ModelarDados.ObterLancamentosSemHorasZeradas(this.lancamentos));

            List<string> lines = new List<string>();

            List<string> reqs = new List<string>();
            foreach (string l in t.Keys)
                foreach (string r in t[l].Keys)
                    if (!reqs.Contains(r)) reqs.Add(r);

            foreach (string nome in t.Keys)
            {
                Dictionary<string, int> rss = t[nome];
                Dictionary<string, string> d = ObterPadraoRss(reqs, "");
                foreach (string rs in rss.Keys)
                {
                    d[rs] = $"<td>{rss[rs]}</td>";
                }
                lines.Add("<tr>");
                lines.Add($"<td>{nome}</td>");
                lines.AddRange(d.Values.ToList());
                lines.Add($"<td>{rss.Sum(x => x.Value)}</td>");
                lines.Add("</tr>");
            }
            List<string> cab = new List<string>() { "<tr>", "<td>&nbsp;</td>" };
            List<string> rod = new List<string>() { "<tr>", "<td>Total</td>" };
            int tt = 0;

            for (int i = 0; i < reqs.Count; i++)
            {
                cab.Add($"<td>{reqs[i]}</td>");
                int total = t.Sum(x => x.Value.TryGetValue(reqs[i], out int result) ? result : 0);
                tt += total;
                rod.Add($"<td>{total}</td>");
                //var s = t.Sum(y => y.Value.Where(l => l.Key.Contains(reqs[i])));
            }
            cab.Add("<td>TOTAL</td>");
            cab.Add("</tr>");
            rod.Add($"<td>{tt}</td>");
            rod.Add("</tr>");

            string bs = File.ReadAllText("base.txt");
            List<string> ls = new List<string>()
            {
                bs,
                "<div class=\"box\">",
                "<h2>Quantidade de lançamentos nas requisições</h2>",
                "<table>"
            };
            ls.AddRange(cab);
            ls.AddRange(lines);
            ls.AddRange(rod);
            ls.Add("</table>");
            ls.Add("</div>");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ls.Count; i++)
            {
                sb.Append($"{ls[i]}");
            }
            return sb.ToString();
        }

        private string ResumoTempoRequisicao()
        {
            var t = ModelarDados.UnificarTempoPorRequisicaoPessoa(this.lancamentos);
            List<string> rss = ModelarDados.ObterRequisicoes(this.lancamentos);
            List<string> lines = new List<string>();
            lines.Add($"<tr>\n<td></td>");
            for (int i = 0; i < rss.Count; i++)
                lines.Add($"<td>{rss[i]}</td>");
            lines.Add($"<td>Total</td></tr>");

            TimeSpan tt = new TimeSpan();
            Dictionary<string, TimeSpan> tempoReq = new Dictionary<string, TimeSpan>();
            TimeSpan[] tiraBug = new TimeSpan[rss.Count];

            foreach (string nome in t.Keys)
            {
                Dictionary<string, TimeSpan> reqs = t[nome];                
                string[] colunas = new string[rss.Count];
                for (int i = 0; i < rss.Count; i++) colunas[i] = $"<td></td>";
                foreach (string r in reqs.Keys)
                {
                    TimeSpan time = reqs[r];
                    if (tempoReq.ContainsKey(r)) tempoReq[r] += time;
                    else tempoReq.Add(r, time);
                    for (int i = 0; i < rss.Count; i++)
                        if (r.Contains(rss[i])) colunas[i] = $"<td>{ArredondarTempo(time)}</td>";
                }
                TimeSpan total = reqs.Sum(x => x.Value.Duration());
                tt += total;
                lines.Add("<tr>");
                lines.Add($"<td>{nome}</td>");
                lines.AddRange(colunas);
                lines.Add($"<td>{ArredondarTempo(total)}</td>");
                lines.Add("</tr>");
            }
            string bs = File.ReadAllText("base.txt");
            List<string> ls = new List<string>()
            {
                bs,
                "<div class=\"box\">",
                "<h2>Tempo das requisições</h2>",
                "<table>"
            };
            lines.AddRange(new List<string>()
                {
                    "<tr>",
                    "<td>Total</td>"
                });

            List<string> tp = new List<string>(tempoReq.Keys);
            if (tp.Count == rss.Count)
            {
                for (int i = 0; i < rss.Count; i++)
                {
                    if (rss[i].Contains(tp[i])) tiraBug[i] = tempoReq[tp[i]];
                }
                for (int i = 0; i < tiraBug.Length; i++)
                {
                    lines.Add($"<td>{ArredondarTempo(tiraBug[i])}</td>");
                }
                lines.Add($"<td>{ArredondarTempo(tt)}</td>");
            }
            lines.Add("<tr>");
            ls.AddRange(lines);
            ls.Add("</table>");
            ls.Add("</div>");

            return ArrayParaString(ls);
        }

        private string LancamentoResumo()
        {
            List<string> ls = ModelarDados.ObterNomes(lancamentos);
            List<string> divs = new List<string>();

            for (int i = 0; i < ls.Count; i++)
            {
                string nome = ls[i];
                var t = ModelarDados.ObterTempoDiaSimples(this.lancamentos, nome);
                List<string> lines = new List<string>()
                {
                    "<tr>",
                    "<td>&nbsp;</td>",
                    "<td>P1</td>",
                    "<td>P2</td>",
                    "<td>P3</td>",
                    "<td>P4</td>",
                    "<td>Total</td>",
                };

                foreach (string dia in t.Keys)
                {
                    Dictionary<PeriodosSimples, TimeSpan> tempo = t[dia];
                    string[] colunas = new string[6] { $"<td>{dia}</td>", "<td>&nbsp;</td>", "<td>&nbsp;</td>", "<td>&nbsp;</td>", "<td>&nbsp;</td>", "<td>&nbsp;</td>", };
                    foreach (PeriodosSimples p in tempo.Keys)
                    {
                        TimeSpan time = tempo[p];
                        switch (p)
                        {
                            case PeriodosSimples.P1:
                                colunas[1] = $"<td>{ArredondarTempo(time)}</td>";
                                break;
                            case PeriodosSimples.P2:
                                colunas[2] = $"<td>{ArredondarTempo(time)}</td>";
                                break;
                            case PeriodosSimples.P3:
                                colunas[3] = $"<td>{ArredondarTempo(time)}</td>";
                                break;
                            case PeriodosSimples.P4:
                                colunas[4] = $"<td>{ArredondarTempo(time)}</td>";
                                break;
                            default:
                                break;
                        }
                    }
                    lines.Add("<tr>");
                    colunas[5] = $"<td>{ArredondarTempo(tempo.Sum(x => x.Value.Duration()))}</td>";
                    lines.AddRange(colunas.ToList());
                    lines.Add("</tr>");
                }
                string bs = File.ReadAllText("base.txt");
                divs.AddRange(new List<string>()
                {
                 bs,
                "<div class=\"box\">",
                $"<h2>Tempo resumido: {nome}</h2>",
                "<table>"
                });
                divs.AddRange(lines);
                divs.Add("</table>");
                divs.Add("</div>");
                lines = new List<string>();
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < divs.Count; i++)
            {
                sb.Append(divs[i]);
            }
            return sb.ToString();
        }

        private string LancamentoResumoPessoa(string nome)
        {
            List<string> divs = new List<string>();

            var t = ModelarDados.ObterTempoDiaSimples(this.lancamentos, nome);
            List<string> lines = new List<string>()
                {
                    "<tr>",
                    "<td>&nbsp;</td>",
                    "<td>P1</td>",
                    "<td>P2</td>",
                    "<td>P3</td>",
                    "<td>P4</td>",
                    "<td>Total</td>",
                };

            foreach (string dia in t.Keys)
            {
                Dictionary<PeriodosSimples, TimeSpan> tempo = t[dia];
                string[] colunas = new string[6] { $"<td>{dia}</td>", "<td>&nbsp;</td>", "<td>&nbsp;</td>", "<td>&nbsp;</td>", "<td>&nbsp;</td>", "<td>&nbsp;</td>", };
                foreach (PeriodosSimples p in tempo.Keys)
                {
                    TimeSpan time = tempo[p];
                    switch (p)
                    {
                        case PeriodosSimples.P1:
                            colunas[1] = $"<td>{ArredondarTempo(time)}</td>";
                            break;
                        case PeriodosSimples.P2:
                            colunas[2] = $"<td>{ArredondarTempo(time)}</td>";
                            break;
                        case PeriodosSimples.P3:
                            colunas[3] = $"<td>{ArredondarTempo(time)}</td>";
                            break;
                        case PeriodosSimples.P4:
                            colunas[4] = $"<td>{ArredondarTempo(time)}</td>";
                            break;
                        default:
                            break;
                    }
                }
                lines.Add("<tr>");
                colunas[5] = $"<td>{ArredondarTempo(tempo.Sum(x => x.Value.Duration()))}</td>";
                lines.AddRange(colunas.ToList());
                lines.Add("</tr>");
            }

            string bs = File.ReadAllText("base.txt");
            divs.AddRange(new List<string>()
                {
                bs,
                "<div class=\"box\">",
                $"<h2>Tempo resumido: {nome}</h2>",
                "<table>"
                });
            divs.AddRange(lines);
            divs.Add("</table>");
            divs.Add("</div>");
            lines = new List<string>();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < divs.Count; i++)
            {
                sb.Append(divs[i]);
            }
            return sb.ToString();
        }

        private string LancamentoDetalhe()
        {
            List<string> ls = ModelarDados.ObterNomes(lancamentos);
            List<string> divs = new List<string>();


            for (int i = 0; i < ls.Count; i++)
            {
                string nome = ls[i];
                var t = ModelarDados.ObterDiaCompleto(this.lancamentos, nome);

                List<string> corpo = new List<string>();

                foreach (string dia in t.Keys)
                {
                    List<HorasLancamento> hls = t[dia];
                    List<string[]> colunas = new List<string[]>();

                    for (int l = 0; l < hls.Count; l++)
                    {
                        HorasLancamento hl = hls[l];
                        string[] coluna = new string[7]
                        {
                            "<tr>",
                            $"<td>{hl.Requisicao}</td>",
                            $"<td>{hl.Periodos}</td>",
                            $"<td>{ArredondarTempo(hl.HoraInicial)}</td>",
                            $"<td>{ArredondarTempo(hl.HoraFinal)}</td>",
                            $"<td>{ArredondarTempo(hl.HoraFinal - hl.HoraInicial)}</td>",
                            "</tr>",
                        };
                        colunas.Add(coluna);
                    }
                    corpo.AddRange(new List<string>()
                    {
                       $"<label>{(hls.Count>0 ? string.Format("{0} | {1} | {2}",ModelarDados.ObterDiaDaSemana(hls[0].TempoInicial), ModelarDados.ObterMes(hls[0].TempoInicial), hls[0].TempoInicial.Year) : dia)}</label>",
                        "<table>",
                        "<tr>",
                        "<td>REQUISICAO</td>",
                        "<td>PERIODO</td>",
                        "<td>HORA INICIAL</td>",
                        "<td>HORA FINAL</td>",
                        "<td>HORAS TRABALHADAS</td>",
                        "</tr>",

                    });
                    for (int l = 0; l < colunas.Count; l++)
                    {
                        corpo.AddRange(colunas[l].ToList());
                    }
                    corpo.AddRange(new List<string>()
                    {
                        "<tr>",
                        "<td>&nbsp;</td>",
                        "<td>&nbsp;</td>",
                        "<td>&nbsp;</td>",
                        "<td>&nbsp;</td>",
                        $"<td>{ArredondarTempo(hls.Sum(x=> new TimeSpan(x.HoraInicial.Ticks - x.HoraFinal.Ticks).Duration()))}</td>",
                        "</tr>",
                    });
                    corpo.Add("</table>");
                    corpo.Add("<br>");
                    corpo.Add("<br>");
                }
                string bs = File.ReadAllText("base.txt");
                divs.AddRange(new List<string>()
                {bs,
                    "<div class=\"box\">",
                    $"<h2>Tempo detalhe: {nome}</h2>"
                });
                divs.AddRange(corpo);
                divs.Add("</div>");
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < divs.Count; i++)
            {
                sb.Append(divs[i]);
            }
            return sb.ToString();
        }

        private string LancamentoDetalhePessoa(string nome)
        {
            List<string> ls = ModelarDados.ObterNomes(lancamentos);
            List<string> divs = new List<string>();

            var t = ModelarDados.ObterDiaCompleto(this.lancamentos, nome);

            List<string> corpo = new List<string>();

            foreach (string dia in t.Keys)
            {
                List<HorasLancamento> hls = t[dia];
                List<string[]> colunas = new List<string[]>();

                for (int l = 0; l < hls.Count; l++)
                {
                    HorasLancamento hl = hls[l];
                    string[] coluna = new string[7]
                    {
                            "<tr>",
                            $"<td>{hl.Requisicao}</td>",
                            $"<td>{hl.Periodos}</td>",
                            $"<td>{ArredondarTempo(hl.HoraInicial)}</td>",
                            $"<td>{ArredondarTempo(hl.HoraFinal)}</td>",
                            $"<td>{ArredondarTempo(hl.HoraFinal - hl.HoraInicial)}</td>",
                            "</tr>",
                    };
                    colunas.Add(coluna);
                }
                corpo.AddRange(new List<string>()
                    {
                        $"<label>{(hls.Count>0 ? string.Format("{0} | {1} | {2}",ModelarDados.ObterDiaDaSemana(hls[0].TempoInicial), ModelarDados.ObterMes(hls[0].TempoInicial), hls[0].TempoInicial.Year) : dia)}</label>",
                        "<table>",
                        "<tr>",
                        "<td>REQUISICAO</td>",
                        "<td>PERIODO</td>",
                        "<td>HORA INICIAL</td>",
                        "<td>HORA FINAL</td>",
                        "<td>HORAS TRABALHADAS</td>",
                        "</tr>",

                    });
                for (int l = 0; l < colunas.Count; l++)
                {
                    corpo.AddRange(colunas[l].ToList());
                }
                corpo.AddRange(new List<string>()
                    {
                        "<tr>",
                        "<td>&nbsp;</td>",
                        "<td>&nbsp;</td>",
                        "<td>&nbsp;</td>",
                        "<td>&nbsp;</td>",
                        $"<td>{ArredondarTempo(hls.Sum(x=> new TimeSpan(x.HoraInicial.Ticks - x.HoraFinal.Ticks).Duration()))}</td>",
                        "</tr>",
                    });
                corpo.Add("</table>");
                corpo.Add("<br>");
                corpo.Add("<br>");
            }
            string bs = File.ReadAllText("base.txt");
            divs.AddRange(new List<string>()
                {bs,
                    "<div class=\"box\">",
                    $"<h2>Tempo detalhe: {nome}</h2>"
                });
            divs.AddRange(corpo);
            divs.Add("</div>");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < divs.Count; i++)
            {
                sb.Append(divs[i]);
            }
            return sb.ToString();
        }
        #endregion

        #region utilidades
        private void OrdenarControlesRecentes()
        {
            string[] lines = File.ReadAllLines("dirs.txt");
            List<Panel> ps = new List<Panel>();
            this.flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                FileInfo f = new FileInfo(lines[i]);
                if (f.Exists)
                {
                    ps.Add(ObterPanel(i.ToString(), f.FullName, f.Name));
                }
                else
                {
                    ps.Add(ObterPanel(i.ToString(), f.FullName, f.Name, Color.Red));
                }
            }
            if (ps.Count > 0) this.flowLayoutPanel1.Controls.AddRange(ps.ToArray());
            this.Focus();
        }

        private Panel ObterPanel(string name, string diretorio, string titulo)
        {
            Panel panel1 = new Panel();
            Label label1 = new Label();
            Label label2 = new Label();
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new System.Drawing.Point(3, 3);
            panel1.Name = name;
            panel1.Size = new System.Drawing.Size(510, 76);
            panel1.TabIndex = 0;
            panel1.Cursor = Cursors.Hand;
            panel1.BackColor = Color.FromArgb(186, 186, 186);
            panel1.Tag = diretorio;
            panel1.Click += new EventHandler(Panel_click);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(15, 41);
            label1.Name = name;
            label1.Size = new System.Drawing.Size(209, 20);
            label1.TabIndex = 0;
            label1.Text = diretorio;
            label1.Cursor = Cursors.Hand;
            label1.Tag = diretorio;
            label1.Click += new EventHandler(Panel_click);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.Location = new System.Drawing.Point(15, 15);
            label2.Name = name;
            label2.Size = new System.Drawing.Size(161, 24);
            label2.TabIndex = 1;
            label2.Text = titulo;
            label2.Cursor = Cursors.Hand;
            label2.Tag = diretorio;
            label2.Click += new EventHandler(Panel_click);
            return panel1;
        }

        private Panel ObterPanel(string name, string diretorio, string titulo, Color forecolor)
        {
            Panel panel1 = new Panel();
            Label label1 = new Label();
            Label label2 = new Label();
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new System.Drawing.Point(3, 3);
            panel1.Name = name;
            panel1.Size = new System.Drawing.Size(510, 76);
            panel1.TabIndex = 0;
            panel1.Cursor = Cursors.Hand;
            panel1.BackColor = Color.FromArgb(186, 186, 186);
            panel1.Tag = diretorio;
            panel1.Click += new EventHandler(Panel_click);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(15, 41);
            label1.Name = name;
            label1.Size = new System.Drawing.Size(209, 20);
            label1.TabIndex = 0;
            label1.ForeColor = forecolor;
            label1.Text = diretorio;
            label1.Cursor = Cursors.Hand;
            label1.Tag = diretorio;
            label1.Click += new EventHandler(Panel_click);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.Location = new System.Drawing.Point(15, 15);
            label2.Name = name;
            label2.Size = new System.Drawing.Size(161, 24);
            label2.TabIndex = 1;
            label2.ForeColor = forecolor;
            label2.Text = titulo;
            label2.Cursor = Cursors.Hand;
            label2.Tag = diretorio;
            label2.Click += new EventHandler(Panel_click);
            return panel1;
        }

        private void HabilitarVerRecentes(bool habilitado)
        {
            this.flowLayoutPanel1.Visible = habilitado;
            this.label1.Visible = habilitado;
        }

        private bool AbrirArquivo(string file, Panel panel)
        {
            try
            {
                FileInfo fi = new FileInfo(file);
                if (!fi.Exists)
                {
                    if (MessageBox.Show("O arquivo selecionado não existe\nDeseja eliminar da lista?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.flowLayoutPanel1.Controls.Remove(panel);
                        string[] lines = File.ReadAllLines("dirs.txt");
                        lines = lines.Where(x => !x.Contains(fi.FullName)).ToArray();
                        File.WriteAllLines("dirs.txt", lines);
                        return false;
                    }
                    MessageBox.Show("O arquivo selecionado não existe\nDeseja eliminar da lista?");
                }
                if (fi.Extension == ".xls" || fi.Extension == ".xlsx")
                {
                    this.path = fi.FullName;
                    string[] lines = File.ReadAllLines("dirs.txt");
                    List<string> ls = new List<string>();

                    if (lines.Length > 5)
                    {
                        ls = lines.ToList();
                        for (int i = 5; i < lines.Length; i++)
                        {
                            ls.RemoveAt(i);
                        }
                        lines = ls.ToArray();
                        ls = new List<string>();
                    }

                    if (!lines.Contains(fi.FullName))
                    {
                        ls.Add(fi.FullName);
                        ls.AddRange(lines.ToList());
                    }
                    else // quando clica no panel
                    {
                        int index = Array.FindIndex(lines, x => x.Contains(fi.FullName));
                        if (index != -1)
                        {
                            ls.Add(lines[index]);
                            lines = lines.Where(x => !x.Contains(lines[index])).ToArray();
                            ls.AddRange(lines.ToList());
                        }
                    }
                    File.WriteAllLines("dirs.txt", ls);
                    OrdenarControlesRecentes();
                    Dictionary<string, List<HorasLancamento>> lancamentos = Leitura.ObterLancamento(this.path);
                    ModelarDados.AlinharTempo(ref lancamentos);
                    this.lancamentos = lancamentos;
                }
                else MessageBox.Show("A extensão do arquivo selecionado é inválido!");
                return true;
            }
            catch 
            { 
                HabilitarVerRecentes(false);
                return false;
            }
        }

        private void AbrirArquivo()
        {
            try
            {
                this.openFileDialog1.Title = "Selecionar planilha";
                this.openFileDialog1.Filter = "Tipos de Planilhas (.xls, .xlsx)|*.XLS;*.XLSX";
                this.openFileDialog1.DefaultExt = ".xls";
                this.openFileDialog1.FileName = "";
                if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(this.openFileDialog1.FileName);
                    if (fi.Extension == ".xls" || fi.Extension == ".xlsx")
                    {
                        this.path = fi.FullName;
                        string[] lines = File.ReadAllLines("dirs.txt");
                        List<string> ls = new List<string>();

                        if (lines.Length > 5)
                        {
                            ls = lines.ToList();
                            for (int i = 5; i < lines.Length; i++)
                            {
                                ls.RemoveAt(i);
                            }
                            lines = ls.ToArray();
                            ls = new List<string>();
                        }

                        if (!lines.Contains(fi.FullName))
                        {
                            ls.Add(fi.FullName);
                            ls.AddRange(lines.ToList());
                        }
                        else // quando clica no panel
                        {
                            int index = Array.FindIndex(lines, x => x.Contains(fi.FullName));
                            if (index != -1)
                            {
                                ls.Add(lines[index]);
                                lines = lines.Where(x => !x.Contains(lines[index])).ToArray();
                                ls.AddRange(lines.ToList());
                            }
                        }
                        File.WriteAllLines("dirs.txt", ls);
                        OrdenarControlesRecentes();
                        Dictionary<string, List<HorasLancamento>> lancamentos = Leitura.ObterLancamento(this.path);
                        ModelarDados.AlinharTempo(ref lancamentos);
                        this.lancamentos = lancamentos;
                        HabilitarVerRecentes(false);
                        this.visualizaçãoToolStripMenuItem.Enabled = true;
                    }
                    else MessageBox.Show("A extensão do arquivo selecionado é inválido!");
                }
            }
            catch { HabilitarVerRecentes(false); }
        }

        private Dictionary<string, string> ObterPadraoRss(List<string> rss, string value)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            for (int i = 0; i < rss.Count; i++)
            {
                if (d.ContainsKey(rss[i])) d[rss[i]] = $"<td>{value}</td>";
                else d.Add(rss[i], $"<td>{value}</td>");
            }
            return d;
        }

        private string ArredondarTempo(TimeSpan time)
        {            
            TimeSpan ntime = time;
            Regex rg = new Regex("9$");
            if (rg.IsMatch(time.Minutes.ToString()))
                ntime = ntime + TimeSpan.Parse("00:01:00");
            int h = 0;
            if (ntime.Days > 0) h = ntime.Days * 24;
            return string.Format("{0:D2}h{1:D2}", ntime.Hours + h, ntime.Minutes);
        }

        private string ArrayParaString(List<string> ls)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ls.Count; i++)
            {
                sb.Append(ls[i]);
            }
            return sb.ToString();
        }
        #endregion

        #region parametros
        private void menuAbrirArquivo_click(object sender, EventArgs e)
        {
            try { AbrirArquivo(); }
            catch { }
        }

        private void menuResumoHorasRequisicao_click(object sender, EventArgs e)
        {
            try
            {
                this.webBrowser1.Navigate("about:blank");
                StringBuilder sb = new StringBuilder();
                sb.Append($"{ResumoHoras()}{ResumoTempoRequisicao()}{ResumoHorasQtdRequisicao()}</body></html>");
                this.salvar = sb.ToString();
                this.webBrowser1.Document.Write(sb.ToString());
                this.webBrowser1.Refresh();
                HabilitarVerRecentes(false);
                this.webBrowser1.Visible = true;
            }
            catch { }
        }

        private void menuSimularRegua_click(object sender, EventArgs e)
        {
            try
            {
                this.webBrowser1.Navigate("about:blank");
                string html = LancamentoDetalhe();
                this.salvar = html;
                this.webBrowser1.Document.Write($"{html}</body></html>");
                this.webBrowser1.Refresh();
                HabilitarVerRecentes(false);
                this.webBrowser1.Visible = true;
            }
            catch { }
        }

        private void menuSimularReguaPessoa_click(object sender, EventArgs e)
        {
            try
            {
                Nomes n = new Nomes(ModelarDados.ObterNomes(lancamentos));
                n.ShowDialog();
                if (!string.IsNullOrEmpty(n.receb))
                {
                    this.webBrowser1.Navigate("about:blank");
                    string html = LancamentoDetalhePessoa(n.receb);
                    this.webBrowser1.Document.Write($"{html}</body></html>");
                    this.salvar = html;
                    this.webBrowser1.Refresh();
                }
                this.webBrowser1.Visible = true;
                HabilitarVerRecentes(false);
            }
            catch { }
        }

        private void menuLancamentoResumo_click(object sender, EventArgs e)
        {
            try
            {
                this.webBrowser1.Navigate("about:blank");
                string html = LancamentoResumo();
                this.webBrowser1.Document.Write($"{html}</body></html>");
                this.salvar = html;
                this.webBrowser1.Refresh();
                this.webBrowser1.Visible = true;
                HabilitarVerRecentes(false);
            }
            catch { }
        }

        private void menuLancamentoResumoPessoa_click(object sender, EventArgs e)
        {
            try
            {
                Nomes n = new Nomes(ModelarDados.ObterNomes(lancamentos));
                n.ShowDialog();
                if (!string.IsNullOrEmpty(n.receb))
                {
                    this.webBrowser1.Navigate("about:blank");
                    string html = LancamentoResumoPessoa(n.receb);
                    this.webBrowser1.Document.Write($"{html}</body></html>");
                    this.salvar = html;
                    this.webBrowser1.Refresh();
                }
                this.webBrowser1.Visible = true;
                HabilitarVerRecentes(false);
            }
            catch { }
        }

        private void menuVerRecentes_click(object sender, EventArgs e)
        {
            try
            {
                this.webBrowser1.Visible = false;
                HabilitarVerRecentes(true);
            }
            catch { }
        }

        private void menuDefinirFeriado_click(object sender, EventArgs e)
        {
            try
            {
                TelaFeriado tf = new TelaFeriado();
                tf.ShowDialog();
            }
            catch { }
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sobre s = new Sobre();
            s.ShowDialog();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region outros
        private void Panel_click(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType() == typeof(Panel))
                {
                    Panel p = sender as Panel;
                    if (p.Tag is null) return;
                    if (AbrirArquivo(p.Tag as string, p))
                    {
                        HabilitarVerRecentes(false);
                        this.visualizaçãoToolStripMenuItem.Enabled = true;
                    }
                    else
                        HabilitarVerRecentes(true);
                }
                else if (sender.GetType() == typeof(Label))
                {
                    Label l = sender as Label;
                    if (l.Tag is null) return;
                    if (AbrirArquivo(l.Tag as string, l.Parent as Panel))
                    {
                        HabilitarVerRecentes(false);
                        this.visualizaçãoToolStripMenuItem.Enabled = true;
                    }
                    else
                        HabilitarVerRecentes(true);
                }
                return;
            }
            catch { HabilitarVerRecentes(false); }      
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                AbrirArquivo();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                this.salvarToolStripMenuItem.PerformClick();
            }
        }

        #endregion

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.salvar))
            {
                SaveFileDialog f = new SaveFileDialog();
                f.DefaultExt = ".html";
                f.Title = "Salvar Horas";
                f.FileName = "Horas.html";
                f.AddExtension = false;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string path = f.FileName;
                    File.WriteAllText(path, this.salvar);
                }
            }
        }

        private void criarPlanilhaGeralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlanilhaGeral.Criar(this.lancamentos);
        }
    }

    public static class StatisticExtensions
    {
        public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
        {
            return source.Select(selector).Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
        }
    }
}
