using CalculadoraLancamento.variaveis;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculadoraLancamento.leitura
{
    public class Leitura
    {
        public static Dictionary<string, List<HorasLancamento>> ObterLancamento(string caminho)
        {
            Dictionary<string, List<HorasLancamento>> lancamentos = new Dictionary<string, List<HorasLancamento>>();
            XLWorkbook wb = null;
            try
            {
                wb = new XLWorkbook(caminho);
            }
            catch (IOException io)
            {
                MessageBox.Show(io.Message);
                return null;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
           
            var planilha = wb.Worksheet(1);

            int line = 1;
            while (true)
            {
                string nome = planilha.Cell($"B{line}").Value.ToString().Trim();
                string tempoInicio = planilha.Cell($"D{line}").Value.ToString().Trim();
                string tempoFim = planilha.Cell($"E{line}").Value.ToString().Trim();
                string tempototal = planilha.Cell($"F{line}").Value.ToString().Trim();
                string tipoPeriodo = planilha.Cell($"G{line}").Value.ToString().Trim();
                string requisicao = planilha.Cell($"H{line}").Value.ToString().Trim();
                string eferiado = planilha.Cell($"I{line}").Value.ToString().Trim();

                bool a = string.IsNullOrEmpty(nome) == true;
                bool b = string.IsNullOrEmpty(tempoInicio) == true;
                bool c = string.IsNullOrEmpty(tempoFim) == true;
                bool d = string.IsNullOrEmpty(tempototal) == true;
                bool e = string.IsNullOrEmpty(tipoPeriodo) == true;
                bool f = string.IsNullOrEmpty(requisicao) == true;
                bool g = string.IsNullOrEmpty(eferiado) == true;

                if (a && b && c && e && f && g) break;

                HorasLancamento hl = new HorasLancamento();

                if (!a) hl.Nome = nome;
                if (!b) hl.TempoInicial = Regex.IsMatch(tempoInicio, "^[0-9]+.[0-9]+$") ? DateTime.FromOADate(double.Parse(tempoInicio)) : DateTime.Parse(tempoInicio);
                if (!c) hl.TempoFinal = Regex.IsMatch(tempoFim, "^[0-9]+.[0-9]+$") ? DateTime.FromOADate(double.Parse(tempoFim)) : DateTime.Parse(tempoFim);
                if (!b) hl.HoraInicial = Regex.IsMatch(tempoInicio, "^[0-9]+.[0-9]+$") ? TimeSpan.FromTicks(DateTime.FromOADate(double.Parse(tempoInicio)).Ticks) : TimeSpan.FromTicks(DateTime.Parse(tempoInicio).Ticks);
                if (!c) hl.HoraFinal = Regex.IsMatch(tempoFim, "^[0-9]+.[0-9]+$") ? TimeSpan.FromTicks(DateTime.FromOADate(double.Parse(tempoFim)).Ticks) : TimeSpan.FromTicks(DateTime.Parse(tempoFim).Ticks);
                if (!d) hl.TempoTotal = Regex.IsMatch(tempototal, "^[0-9]+.[0-9]+$") ? TimeSpan.FromTicks(DateTime.FromOADate(double.Parse(tempototal)).Ticks) : TimeSpan.FromTicks(DateTime.Parse(tempototal).Ticks);
                if (!e) hl.TipoPeriodo = (tipoPeriodo.ToUpper() == "ATIVA" ? TipoPeriodo.ATIVA : TipoPeriodo.STAND);
                if (!f) hl.Requisicao = requisicao.ToUpper();
                if (!g) hl.EFeriado = (eferiado == "feriado");

                if (!lancamentos.ContainsKey(nome)) lancamentos.Add(nome, new List<HorasLancamento>() { hl });
                else lancamentos[nome].Add(hl);

                line++;
            }
            wb.Dispose();
            Dictionary<string, List<HorasLancamento>> nl = new Dictionary<string, List<HorasLancamento>>();
            foreach (string i in lancamentos.Keys)
            {
                nl.Add(i, lancamentos[i].OrderBy(x => x.TempoInicial).ToList());
            }
            return nl;
        }
    }
}
