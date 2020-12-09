using CalculadoraLancamento.variaveis;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculadoraLancamento.imprimir
{
    public class PlanilhaGeral
    {
        public static void Criar(Dictionary<string, List<HorasLancamento>> lancamentos)
        {
            File.Create("planilha_geral.xlsx");
            if (File.Exists("planilha_geral.xlsx"))
            {
                XLWorkbook wb = null;
                try
                {
                    wb = new XLWorkbook();
                }
                catch (IOException io)
                {
                    MessageBox.Show(io.Message);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
}
