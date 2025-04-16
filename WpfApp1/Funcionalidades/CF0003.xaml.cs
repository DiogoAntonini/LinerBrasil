using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0003 : Window
    {
        private readonly string usuarioId;
        private readonly Consultar db;

        public CF0003(string usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            db = new Consultar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnCodigoCliente_Click(this, new RoutedEventArgs());
        }

        private async void btnCodigoCliente_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT CCC.\"NmCliente\", CCC.\"CdMaterialInterno\" , CCC.\"CdMaterialCliente\" , CCC.\"NrMetrosLinear\" , CCC.\"NrLargura\" , CCC.\"NrMetrosQuadradoBobina\" , CCC.\"NrMetrosQuadradoPalete\" , CCC.\"QtBobinasPorPalete\" , CCC.\"NrColunasPorPalete\" , CCC.\"NrBobinasPorColuna\" , CCC.\"StEtiquetaDentroEFora\" , CCC.\"NrDiaMetroMin\" , CCC.\"NrDiametroMax\" , CCC.\"DsEmendaAutomatica\" , CCC.\"DsEmendaManual\" FROM \"T0002_ControleCodigoCliente\" CCC;";

            try
            {
                DataTable tabela = await Task.Run(() => db.ExecutarConsulta(query));
                dtgGrade.ItemsSource = tabela.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao carregar os dados: {ex.Message}");
            }
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            CF0004 adicionarCodigo = new CF0004();
            adicionarCodigo.ShowDialog();
            this.Close();
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            CF0005 alterar = new CF0005();
            alterar.ShowDialog();
            this.Close();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            CF0006 excluir = new CF0006(usuarioId);
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