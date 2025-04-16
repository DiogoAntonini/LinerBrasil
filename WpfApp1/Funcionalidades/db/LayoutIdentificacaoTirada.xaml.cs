using System.Windows;

namespace LinerApp.Funcionalidades.db
{
    public partial class LayoutIdentificacaoTirada : Window
    {
        public LayoutIdentificacaoTirada()
        {
            InitializeComponent();
        }

        public string IdMaquinaValue => IdMaquina.Text;
        public string CdOSValue => CdOS.Text;
        public string NrMetrosLinearValue => NrMetrosLinear.Text;
        public string NmOperadorValue => NmOperador.Text;
        public string IdRecebimentoValue => IdRecebimento.Text;
        public string NrPalletValue => NrPallet.Text;
        public string NrTiradaValue => NrTirada.Text;
        public string DsTurnoValue => DsTurno.Text;
        public string IdLoteCorteValue => IdLoteCorte.Text;
    }
}
