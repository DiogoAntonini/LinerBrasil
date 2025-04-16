using System.Windows;

namespace LinerApp.Funcionalidades.db
{
    public partial class LayoutEtiquetaAprovacaoSemiAcab : Window
    {
        public LayoutEtiquetaAprovacaoSemiAcab()
        {
            InitializeComponent();
        }

        public string DsDyeTesteValue => DsDyeTeste.Text;
        public string DsCorValue => DsCor.Text;
        public string StAlvejanteValue => StAlvejante.Text;
        public string NrGramaturaValue => NrGramatura.Text;
        public string NrReleaseValue => NrRelease.Text;
        public string NrMigracaoValue => NrMigracao.Text;
        public string NrAdesaoValue => NrAdesao.Text;
        public string NrLarguraValue => NrLargura.Text;
        public string DsStatusSiliconizacaoValue => DsStatusSiliconizacao.Text;
        public string IdRecebimentoValue => IdRecebimento.Text;
        public string NmOperadorValue => NmOperador.Text;
        public string DtAtualValue => DtAtual.Text;
        public string HrAtualValue => HrAtual.Text;
    }
}
