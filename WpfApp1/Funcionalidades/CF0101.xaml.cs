using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{

    public partial class CF0101 : Window
    {
        private string usuarioId;
        private Consultar db;

        public CF0101(string usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            db = new Consultar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnEntregas_Click(this, new RoutedEventArgs());
        }

        private void btnEntregas_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT PLJ.\"CdOS\",PLJ.\"PdCliente\", PLJ.\"NmCliente\", TO_CHAR(PLJ.\"DtPedido\", 'DD/MM/YY') \"DtPedido\", TO_CHAR(PLJ.\"DtSolicitacaoEntrega\", 'DD/MM/YY') \"DtSolicitacaoEntrega\", TO_CHAR(PLJ.\"DtSolicitacaoNovaDataCliente\", 'DD/MM/YY') \"DtSolicitacaoNovaDataCliente\", PLJ.\"NrDtPedidoVsDtSolicitacaoEntrega\", PLJ.\"CdInternoMaterial\", PLJ.\"CdCliente\", PLJ.\"NrMetrosLinear\", PLJ.\"NrMetrosPorPalete\", PLJ.\"DsCorte\", PLJ.\"DsSiliconizacao\",TO_CHAR(PLJ.\"DtNovaEntregaLiner\", 'DD/MM/YY') \"DtNovaEntregaLiner\", PLJ.\"NrNFSaida\", PLJ.\"DsObservacao\", PLJ.\"QtPL\" FROM \"T01_Planejamento\" PLJ;";

            DataTable tabela = db.ExecutarConsulta(query);

            dtgGrade.ItemsSource = tabela.DefaultView;
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            CF0102 adicionarplaneja = new CF0102(usuarioId);
            adicionarplaneja.ShowDialog();
            this.Close();
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            CF0103 alterar = new CF0103(usuarioId);
            alterar.ShowDialog();
            this.Close();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            CF0104 excluir = new CF0104();
            excluir.ShowDialog();
            this.Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0002 modulos = new CF0002(usuarioId);
            modulos.Show();
            this.Close();
        }
    }
}