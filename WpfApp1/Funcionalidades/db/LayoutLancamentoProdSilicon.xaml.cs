using System.Windows;

namespace LinerApp.Funcionalidades.db
{
    public partial class LayoutLancamentoProdSilicon : Window
    {
        public LayoutLancamentoProdSilicon()
        {
            InitializeComponent();
        }

        public string DtSiliconizacaoValue => DtSiliconizacao.Text;
        public string IdMaquinaValue => IdMaquina.Text;
        public string CdOSValue => CdOS.Text;
        public string NrMetrosLinearValue => NrMetrosLinear.Text;
        public string NmOperadorValue => NmOperador.Text;
        public string IdRecebimentoValue => IdRecebimento.Text;
        public string NmFornecedorValue => NmFornecedor.Text;
        public string NmClienteValue => NmCliente.Text;
        public string CdInternoValue => CdInterno.Text;
        public string NrBobinaProduzidaValue => NrBobinaProduzida.Text;
    }
}
