using CalculadoraLancamento.variaveis;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalculadoraLancamento.modelar
{
    public class ModelarDados
    {
        /// <summary>
        /// Esse método serve para ajustar o limite do dia, mes e ano atual
        /// </summary>
        /// <param name="lancamentos"></param>
        public static void AlinharTempo(ref Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            if (lancamentos != null)
            {
                foreach (string nome in lancamentos.Keys)
                {
                    List<HorasLancamento> hls = lancamentos[nome];
                    for (int i = 0; i < hls.Count; i++)
                    {
                        HorasLancamento hl = hls[i];
                        if (hl.TempoFinal.Day > hl.TempoInicial.Day || hl.TempoFinal.Month > hl.TempoInicial.Month || hl.TempoFinal.Year > hl.TempoInicial.Year)
                        {
                            hl.TempoFinal = new DateTime(hl.TempoInicial.Year, hl.TempoInicial.Month, hl.TempoInicial.Day, 23, 59, 59);
                            hl.HoraFinal = new TimeSpan(23, 59, 59);
                        }
                    }
                }
            }
        }

        public static Dictionary<string, List<HorasLancamento>> ObterLancamentosSemHorasZeradas(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<string, List<HorasLancamento>> d = new Dictionary<string, List<HorasLancamento>>();
            foreach (string n in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[n];
                List<HorasLancamento> ls = new List<HorasLancamento>();
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    if (hl.HoraInicial >= TimeSpan.Parse("00:20:00") || hl.HoraFinal >= TimeSpan.Parse("00:20:00") || hl.HoraFinal - hl.HoraInicial >= TimeSpan.Parse("00:20:00"))
                        ls.Add(hl);
                }
                d.Add(n, ls);
            }
            return d;
        }

        public static Dictionary<string, Dictionary<string, TimeSpan>> UnificarTempoPorRequisicaoPessoa(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<string, Dictionary<string, TimeSpan>> d = new Dictionary<string, Dictionary<string, TimeSpan>>();
            foreach (string n in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[n];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    if (d.ContainsKey(n))
                    {
                        if (d[n].ContainsKey(hl.Requisicao)) d[n][hl.Requisicao] += args.Value;
                        else d[n].Add(hl.Requisicao, args.Value);
                    }
                    else d.Add(n, new Dictionary<string, TimeSpan>() { { hl.Requisicao, args.Value } });
                }
            }
            return d;
        }

        public static Dictionary<string, TimeSpan> UnificarTempoPorRequisicao(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<string, TimeSpan> ts = new Dictionary<string, TimeSpan>();
            foreach (string nome in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[nome];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    if (ts.ContainsKey(hl.Requisicao)) ts[hl.Requisicao] += args.Value;
                    else ts.Add(hl.Requisicao, args.Value);
                }
            }
            return ts;
        }

        public static void UnificarPeriodoPoRequisicao(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<string, Dictionary<PeriodosSimples, TimeSpan>> ps = new Dictionary<string, Dictionary<PeriodosSimples, TimeSpan>>();
            foreach (string nome in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[nome];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    if (ps.ContainsKey(hl.Requisicao))
                    {
                        if (ps[hl.Requisicao].ContainsKey(args.Key)) ps[hl.Requisicao][args.Key] += args.Value;
                        else ps[hl.Requisicao].Add(args.Key, args.Value);
                    }
                    else ps.Add(hl.Requisicao, new Dictionary<PeriodosSimples, TimeSpan>() { { args.Key, args.Value } });
                }
            }
        }

        public static void UnificarTempoAtivaStandPorRequisicao(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<string, Dictionary<TipoPeriodo, TimeSpan>> t = new Dictionary<string, Dictionary<TipoPeriodo, TimeSpan>>();
            foreach (string nome in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[nome];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    if (t.ContainsKey(hl.Requisicao))
                    {
                        if (t[hl.Requisicao].ContainsKey(hl.TipoPeriodo)) t[hl.Requisicao][hl.TipoPeriodo] += args.Value;
                        else t[hl.Requisicao].Add(hl.TipoPeriodo, args.Value);
                    }
                    else t.Add(hl.Requisicao, new Dictionary<TipoPeriodo, TimeSpan>() { { hl.TipoPeriodo, args.Value } });
                }
            }
        }

        public static void UnificarTempoPorPeriodos(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<PeriodosSimples, TimeSpan> ps = new Dictionary<PeriodosSimples, TimeSpan>();
            foreach (string nome in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[nome];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    if (ps.ContainsKey(args.Key)) ps[args.Key] += args.Value;
                    else ps.Add(args.Key, args.Value);
                }
            }
        }

        public static void UnificarTempoPorDia(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<int, TimeSpan> ts = new Dictionary<int, TimeSpan>();
            foreach (string nome in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[nome];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    if (ts.ContainsKey(hl.TempoInicial.Day)) ts[hl.TempoInicial.Day] += args.Value;
                    else ts.Add(hl.TempoInicial.Day, args.Value);
                }
            }
        }

        public static void UnificarTempoPorAtivaStand(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<TipoPeriodo, TimeSpan> t = new Dictionary<TipoPeriodo, TimeSpan>();
            foreach (string nome in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[nome];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    if (t.ContainsKey(hl.TipoPeriodo)) t[hl.TipoPeriodo] += args.Value;
                    else t.Add(hl.TipoPeriodo, args.Value);
                }
            }
        }

        public static List<string> ObterNomes(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            List<string> l = new List<string>();
            foreach (string n in lancamentos.Keys) if (!l.Contains(n)) l.Add(n);
            return l;
        }

        public static List<string> ObterRequisicoes(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            List<string> l = new List<string>();
            foreach (string n in lancamentos.Keys)
                for (int i = 0; i < lancamentos[n].Count; i++)
                    if (!l.Contains(lancamentos[n][i].Requisicao)) l.Add(lancamentos[n][i].Requisicao);
            return l;
        }

        public static Dictionary<string, Dictionary<PeriodosSimples, TimeSpan>> ObterTempoDiaSimples(Dictionary<string, List<HorasLancamento>> lancamentos, string nm)
        {            
            Dictionary<string, Dictionary<PeriodosSimples, TimeSpan>> d = new Dictionary<string, Dictionary<PeriodosSimples, TimeSpan>>();
            if (lancamentos.ContainsKey(nm))
            {
                List<HorasLancamento> hls = lancamentos[nm];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    string sem = ObterDiaDaSemana(hl.TempoInicial);
                    if (d.ContainsKey(sem))
                    {
                        if (d[sem].ContainsKey(args.Key)) d[sem][args.Key] += args.Value;
                        else d[sem].Add(args.Key, args.Value);
                    }
                    else d.Add(sem, new Dictionary<PeriodosSimples, TimeSpan>() { { args.Key, args.Value } });
                }
            }
            return d;
        }

        public static Dictionary<string, List<HorasLancamento>> ObterDiaCompleto(Dictionary<string, List<HorasLancamento>> lancamentos, string nm)
        {
            Dictionary<string, List<HorasLancamento>> d = new Dictionary<string, List<HorasLancamento>>();
            if (lancamentos.ContainsKey(nm))
            {
                List<HorasLancamento> hls = lancamentos[nm];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    hl.Periodos = args.Key;
                    string sem = ObterDiaDaSemana(hl.TempoInicial);
                    if (d.ContainsKey(sem)) d[sem].Add(hl);
                    else d.Add(sem, new List<HorasLancamento>() { hl });
                }
            }
            return d;
        }

        public static void UnificarTempoAtivaStandPorPessoa(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<string, Dictionary<TipoPeriodo, TimeSpan>> t = new Dictionary<string, Dictionary<TipoPeriodo, TimeSpan>>();
            foreach (string nome in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[nome];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    if (t.ContainsKey(nome))
                    {
                        if (t[nome].ContainsKey(hl.TipoPeriodo)) t[nome][hl.TipoPeriodo] += args.Value;
                        else t[nome].Add(hl.TipoPeriodo, args.Value);
                    }
                    else t.Add(nome, new Dictionary<TipoPeriodo, TimeSpan>() { { hl.TipoPeriodo, args.Value } });
                }
            }
        }

        public static void UnificarTempoAtivaStandPorPessoaRequisicao(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<string, Dictionary<string, TipoPeriodoTempo>> t = new Dictionary<string, Dictionary<string, TipoPeriodoTempo>>();
            foreach (string nome in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[nome];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    if (t.ContainsKey(nome))
                    {
                        if (t[nome].ContainsKey(hl.Requisicao)) t[nome][hl.Requisicao].Tempo += args.Value;
                        else t[nome].Add(hl.Requisicao, new TipoPeriodoTempo() { Tempo = args.Value, TipoPeriodo = hl.TipoPeriodo });
                    }
                    else t.Add(nome, new Dictionary<string, TipoPeriodoTempo>() { { hl.Requisicao, new TipoPeriodoTempo() { TipoPeriodo = hl.TipoPeriodo, Tempo = args.Value } } });
                }
            }
        }

        public static Dictionary<string, Dictionary<string, int>> UnificaQtdDiaRequisicaoPorPessoa(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<string, Dictionary<string, int>> ts = new Dictionary<string, Dictionary<string, int>>();
            foreach (string nome in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[nome];

                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    if (ts.ContainsKey(nome))
                    {
                        if (ts[nome].ContainsKey(hl.Requisicao)) ts[nome][hl.Requisicao]++;
                        else ts[nome].Add(hl.Requisicao, 1);
                    }
                    else ts.Add(nome, new Dictionary<string, int>() { { hl.Requisicao, 1 } });
                }
            }
            return ts;
        }

        public static Dictionary<string, Dictionary<PeriodosSimples, TimeSpan>> UnificarPeriodoPorPessoa(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            Dictionary<string, Dictionary<PeriodosSimples, TimeSpan>> ps = new Dictionary<string, Dictionary<PeriodosSimples, TimeSpan>>();
            foreach (string nome in lancamentos.Keys)
            {
                List<HorasLancamento> hls = lancamentos[nome];
                for (int i = 0; i < hls.Count; i++)
                {
                    HorasLancamento hl = hls[i];
                    KeyValuePair<PeriodosSimples, TimeSpan> args = ObterPeriodo(hl.TempoInicial, hl.HoraInicial, hl.HoraFinal, hl.EFeriado);
                    if (ps.ContainsKey(nome))
                    {
                        if (ps[nome].ContainsKey(args.Key)) ps[nome][args.Key] += args.Value;
                        else ps[nome].Add(args.Key, args.Value);
                    }
                    else ps.Add(nome, new Dictionary<PeriodosSimples, TimeSpan>() { { args.Key, args.Value } });
                }
            }
            return ps;
        }

        public static KeyValuePair<PeriodosSimples, TimeSpan> ObterPeriodo(DateTime data, TimeSpan inicio, TimeSpan fim, bool eferiado)
        {
            TimeSpan t0 = new TimeSpan(0, 0, 0);
            TimeSpan t8 = new TimeSpan(8, 0, 0);
            TimeSpan t17 = new TimeSpan(17, 0, 0);
            TimeSpan t24 = new TimeSpan(1, 0, 0, 0);

            string[] l = File.ReadAllLines("feriados.txt");
            for (int i = 0; i < l.Length; i++)
                l[i] = l[i].Trim();
            if (data.DayOfWeek == DayOfWeek.Sunday || l.Contains(data.Day.ToString().Trim()))
            {
                return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P4, fim - inicio);
            }
            else if (data.DayOfWeek == DayOfWeek.Saturday)
            {
                if (inicio >= t0 && fim <= t8) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P3, fim - inicio);
                else if (inicio >= t8 && fim <= t24) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P4, fim - inicio);
                else if (inicio >= t0 && fim > t8) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P3, fim - inicio);
            }
            else if (data.DayOfWeek == DayOfWeek.Monday)
            {
                if (inicio >= t0 && fim <= t8) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P4, fim - inicio);
                else if (inicio >= t8 && fim <= t17) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P1, fim - inicio);
                else if (inicio >= t17 && fim <= t24) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P2, fim - inicio);
            }
            else
            {
                if (inicio >= t0 && fim <= t8) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P3, fim - inicio);
                else if (inicio >= t8 && fim <= t17) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P1, fim - inicio);
                else if (inicio >= t17 && fim <= t24) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P2, fim - inicio);
                else if (inicio >= t0 && fim > t8) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P3, fim - inicio);
                else if (inicio >= t8 && fim > t17) return new KeyValuePair<PeriodosSimples, TimeSpan>(PeriodosSimples.P1, fim - inicio);
            }
            return new KeyValuePair<PeriodosSimples, TimeSpan>();
        }

        public static string ObterDiaDaSemana(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday: return string.Format("{0:D2} Domingo", date.Day);
                case DayOfWeek.Monday: return string.Format("{0:D2} Segunda", date.Day);
                case DayOfWeek.Tuesday: return string.Format("{0:D2} Terça", date.Day);
                case DayOfWeek.Wednesday: return string.Format("{0:D2} Quarta", date.Day);
                case DayOfWeek.Thursday: return string.Format("{0:D2} Quinta", date.Day);
                case DayOfWeek.Friday: return string.Format("{0:D2} Sexta", date.Day);
                case DayOfWeek.Saturday: return string.Format("{0:D2} Sábado", date.Day);
                default:
                    break;
            }
            return null;
        }

        public static string ObterMes(DateTime date)
        {
            CultureInfo ci = new CultureInfo("pt-BR");
            char[] a = ci.DateTimeFormat.GetMonthName(date.Month).ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
