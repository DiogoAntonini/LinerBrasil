using System.Data;
using System.Windows;
using System.Windows.Shapes;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0308 : Window
    {
        private Consultar db;

        public CF0308()
        {
            InitializeComponent();
            db = new Consultar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnSiliconizacao_Click(this, new RoutedEventArgs());
        }

        private void btnExtrusao_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rctExtrusao = (Rectangle)btnExtrusao.Template.FindName("rctExtrusao", btnExtrusao);
            Rectangle rctSiliconizacao = (Rectangle)btnSiliconizacao.Template.FindName("rctSiliconizacao", btnSiliconizacao);
            Rectangle rctFormulas = (Rectangle)btnFormulas.Template.FindName("rctFormulas", btnFormulas);

            if (rctExtrusao != null)
            {
                rctExtrusao.Visibility = Visibility.Visible;
            }

            if (rctSiliconizacao != null)
            {
                rctSiliconizacao.Visibility = Visibility.Hidden;
            }

            if (rctFormulas != null)
            {
                rctFormulas.Visibility = Visibility.Hidden;
            }

            dtgGradeExtrusao.Visibility = Visibility.Visible;
            btnAdicionarExtrusao.Visibility = Visibility.Visible;
            btnAlterarExtrusao.Visibility = Visibility.Visible;
            dtgGradeFormulas.Visibility = Visibility.Hidden;
            dtgGradeSiliconizacao.Visibility = Visibility.Hidden;
            btnAdicionarFormula.Visibility = Visibility.Hidden;
            btnAlterarFormula.Visibility = Visibility.Hidden;
            btnApurarSiliconizacao.Visibility = Visibility.Hidden;
            btnImprimirEtiqueta.Visibility = Visibility.Hidden;

            string query = "SELECT TO_CHAR(\"HrInicio\", 'HH24:MI') \"HrInicioFormatada\", TO_CHAR(\"HrFim\", 'HH24:MI') \"HrFimFormatada\", EXT.\"NrMetragem\", EXT.\"NrOS\", EXT.\"NmCliente\", TO_CHAR(\"DtProducao\", 'DD/MM/YY') \"DtProducaoFormatada\", EXT.\"DsTurno\", EXT.\"DsPapel\", EXT.\"NrTotalMetragem\" FROM \"T10_Extrusao\" EXT;";

            DataTable tabela = db.ExecutarConsulta(query);

            dtgGradeExtrusao.ItemsSource = tabela.DefaultView;
        }

        private void btnSiliconizacao_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rctExtrusao = (Rectangle)btnExtrusao.Template.FindName("rctExtrusao", btnExtrusao);
            Rectangle rctSiliconizacao = (Rectangle)btnSiliconizacao.Template.FindName("rctSiliconizacao", btnSiliconizacao);
            Rectangle rctFormulas = (Rectangle)btnFormulas.Template.FindName("rctFormulas", btnFormulas);

            if (rctSiliconizacao != null)
            {
                rctSiliconizacao.Visibility = Visibility.Visible;
            }

            if (rctExtrusao != null)
            {
                rctExtrusao.Visibility = Visibility.Hidden;
            }

            if (rctFormulas != null)
            {
                rctFormulas.Visibility = Visibility.Hidden;
            }

            dtgGradeSiliconizacao.Visibility = Visibility.Visible;
            dtgGradeExtrusao.Visibility = Visibility.Hidden;
            dtgGradeFormulas.Visibility = Visibility.Hidden;
            btnAdicionarExtrusao.Visibility = Visibility.Hidden;
            btnAlterarExtrusao.Visibility = Visibility.Hidden;
            btnAdicionarFormula.Visibility = Visibility.Hidden;
            btnAlterarFormula.Visibility = Visibility.Hidden;
            btnApurarSiliconizacao.Visibility = Visibility.Visible;
            btnImprimirEtiqueta.Visibility = Visibility.Visible;
            btnImprimirEtiqueta.Margin = new Thickness(1098, 134, 0, 0);

            string query = "SELECT SLC.\"CdInterno\", SLC.\"IdMaquina\", SLC.\"DtSiliconizacao\", SLC.\"NmOperador\", SLC.\"CdOS\", SLC.\"NrMetrosLinear\", SLC.\"CdBobinaSiliconada\", SLC.\"NrBobinaProduzida\", SLC.\"NrMetrosQuadrados\", SLC.\"NmCliente\", SLC.\"IdRecebimento\", SLC.\"DsStatusSiliconizacao\"  FROM \"T04_Siliconizacao\" SLC WHERE SLC.\"DsStatusSiliconizacao\" = 'Laboratório';";

            DataTable tabela = db.ExecutarConsulta(query);

            dtgGradeSiliconizacao.ItemsSource = tabela.DefaultView;
        }

        private void btnFormulas_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rctExtrusao = (Rectangle)btnExtrusao.Template.FindName("rctExtrusao", btnExtrusao);
            Rectangle rctSiliconizacao = (Rectangle)btnSiliconizacao.Template.FindName("rctSiliconizacao", btnSiliconizacao);
            Rectangle rctFormulas = (Rectangle)btnFormulas.Template.FindName("rctFormulas", btnFormulas);

            if (rctFormulas != null)
            {
                rctFormulas.Visibility = Visibility.Visible;
            }

            if (rctExtrusao != null)
            {
                rctExtrusao.Visibility = Visibility.Hidden;
                dtgGradeExtrusao.Visibility = Visibility.Hidden;
            }

            if (rctSiliconizacao != null)
            {
                rctSiliconizacao.Visibility = Visibility.Hidden;
            }

            dtgGradeFormulas.Visibility = Visibility.Visible;
            dtgGradeExtrusao.Visibility = Visibility.Hidden;
            dtgGradeSiliconizacao.Visibility = Visibility.Hidden;
            btnAdicionarExtrusao.Visibility = Visibility.Hidden;
            btnAlterarExtrusao.Visibility = Visibility.Hidden;
            btnAdicionarFormula.Visibility = Visibility.Visible;
            btnAlterarFormula.Visibility = Visibility.Visible;
            btnApurarSiliconizacao.Visibility = Visibility.Hidden;
            btnImprimirEtiqueta.Visibility = Visibility.Hidden;
        }

        private void btnAdicionarExtrusao_Click(object sender, RoutedEventArgs e)
        {
            CF0309 adicionarExtrusao = new CF0309();
            adicionarExtrusao.ShowDialog();
            Close();
        }

        private void btnAlterarExtrusao_Click(object sender, RoutedEventArgs e)
        {
            CF0310 alterarExtrusao = new CF0310();
            alterarExtrusao.ShowDialog();
            Close();
        }

        private void btnApurarSiliconizacao_Click(object sender, RoutedEventArgs e)
        {
            CF0312 apurarSiliconizacao = new CF0312();
            apurarSiliconizacao.ShowDialog();
            Close();
        }

        private void btnAdicionarFormula_Click(object sender, RoutedEventArgs e)
        {
            CF0310 adicionarFormula = new CF0310();
            adicionarFormula.ShowDialog();
            Close();
        }

        private void btnAlterarFormula_Click(object sender, RoutedEventArgs e)
        {
            CF0310 alterarFormula = new CF0310();
            alterarFormula.ShowDialog();
            Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0301 voltarMain = new CF0301();
            voltarMain.Show();
            Close();
        }

        private void btnImprimirEtiqueta_Click(object sender, RoutedEventArgs e)
        {
            CF0315 imprimirEtiqueta = new CF0315();
            imprimirEtiqueta.ShowDialog();
            Close();
        }
    }
}