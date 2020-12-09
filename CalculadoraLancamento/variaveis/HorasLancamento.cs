using System;

namespace CalculadoraLancamento.variaveis
{
    public class HorasLancamento
    {
        private TimeSpan ti, tf;

        public TipoPeriodo TipoPeriodo { get; set; }
        public DateTime TempoInicial { get; set; }
        public DateTime TempoFinal { get; set; }
        public TimeSpan HoraInicial { get { return tf; } set => tf = new TimeSpan(value.Hours, value.Minutes, value.Seconds); }
        public TimeSpan HoraFinal { get { return ti; } set => ti = new TimeSpan(value.Hours, value.Minutes, value.Seconds); }
        public TimeSpan TempoTotal { get; set; }
        public string Nome { get; set; }
        public string Requisicao { get; set; }
        public PeriodosSimples Periodos {get; set;}
        public bool EFeriado { get; set; }
    }
}
