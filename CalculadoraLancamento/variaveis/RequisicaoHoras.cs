using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraLancamento.variaveis
{
    public class RequisicaoHoras
    {
        public string Requisicao { get; set; }
        public Dictionary<int, TimeSpan> Horas { get; set; }
    }
}
