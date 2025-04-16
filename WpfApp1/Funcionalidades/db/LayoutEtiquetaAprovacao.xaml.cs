using System.Windows;

namespace LinerApp.Funcionalidades.db
{
    public partial class LayoutEtiquetaAprovacao : Window
    {
        public LayoutEtiquetaAprovacao()
        {
            InitializeComponent();
        }

        public string StAlvejanteValue => StAlvejante.Text;
        public string NrGramaturaValue => NrGramatura.Text;
        public string NrLarguraValue => NrLargura.Text;
        public string IdRecebimentoValue => IdRecebimento.Text;
        public string NrBobinaValue => NrBobina.Text;
        public string NmResponsavelValue => NmResponsavel.Text;
        public string DsStatusAmostraValue => DsStatusAmostra.Text;
        public string HtImpressaoValue => HtImpressao.Text;
        public string DtImpressaoValue => DtImpressao.Text;
    }
}