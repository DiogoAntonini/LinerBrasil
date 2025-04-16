using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0302 : Window
    {
        private Consultar db;

        public CF0302()
        {
            InitializeComponent();
            db = new Consultar();
        }

        #region Abas

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnPapel_Click(this, new RoutedEventArgs());
        }

        private void btnPapel_Click(object sender, RoutedEventArgs e)
        {
            btnPendente.Foreground = (Brush)new BrushConverter().ConvertFromString("White");
            btnPendente.Background = (Brush)new BrushConverter().ConvertFromString("#003C1E");

            btnConcluido.Foreground = (Brush)new BrushConverter().ConvertFromString("#003C1E");
            btnConcluido.Background = (Brush)new BrushConverter().ConvertFromString("White");

            Rectangle rctPapel = (Rectangle)btnPapel.Template.FindName("rctPapel", btnPapel);
            Rectangle rctQuimico = (Rectangle)btnQuimico.Template.FindName("rctQuimico", btnQuimico);
            Rectangle rctTinta = (Rectangle)btnTinta.Template.FindName("rctTinta", btnTinta);

            if (rctPapel != null)
            {
                rctLinha1.Visibility = Visibility.Visible;
                rctLinha2.Visibility = Visibility.Visible;
                rctLinha3.Visibility = Visibility.Visible;
                rctLinha4.Visibility = Visibility.Visible;
                rctLinha5.Visibility = Visibility.Visible;
                rctLinha7.Visibility = Visibility.Visible;
                dtgGradePapel.Visibility = Visibility.Visible;
                dtgGradeTinta.Visibility = Visibility.Hidden;
                dtgGradeQuimico.Visibility = Visibility.Hidden;
                dtgPapelConcluido.Visibility = Visibility.Hidden;
                dtgTintaConcluido.Visibility = Visibility.Hidden;
                dtgQuimicoConcluido.Visibility = Visibility.Hidden;
                rctPapel.Visibility = Visibility.Visible;
                rctQuimico.Visibility = Visibility.Hidden;
                rctTinta.Visibility = Visibility.Hidden;
                btnApurarPapel.Visibility = Visibility.Visible;
                btnApurarTinta.Visibility = Visibility.Hidden;
                btnApurarQuimico.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaPapel.Visibility = Visibility.Visible;
                btnImprimirEtiquetaQuimico.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaTinta.Visibility = Visibility.Hidden;
                btnApurarLotePapel.Visibility = Visibility.Visible;
                btnApurarLoteTinta.Visibility = Visibility.Hidden;
                btnApurarLoteQuimico.Visibility = Visibility.Hidden;
            }

            string query = "SELECT \"DsMaterial\", TO_CHAR(\"DtRecebimento\", 'DD/MM/YY') \"DtRecebimento\", TO_CHAR(\"HrRecebimento\", 'HH24:MI') \"HrRecebimento\", TO_CHAR (\"DtAmostragem\", 'DD/MM/YY') \"DtAmostragem\", TO_CHAR(\"HrAmostragem\", 'HH24:MI') \"HrAmostragem\", \"NrNF\", \"NmFornecedor\", \"IdLoteFornecedor\", \"IdRecebimento\" FROM \"T02_Recebimento\" WHERE \"DsStatus\" IN ('Laboratório');";

            DataTable tabela = db.ExecutarConsulta(query);

            dtgGradePapel.ItemsSource = tabela.DefaultView;
        }

        private void btnQuimico_Click(object sender, RoutedEventArgs e)
        {
            btnPendente.Foreground = (Brush)new BrushConverter().ConvertFromString("White");
            btnPendente.Background = (Brush)new BrushConverter().ConvertFromString("#003C1E");

            btnConcluido.Foreground = (Brush)new BrushConverter().ConvertFromString("#003C1E");
            btnConcluido.Background = (Brush)new BrushConverter().ConvertFromString("White");

            Rectangle rctPapel = (Rectangle)btnPapel.Template.FindName("rctPapel", btnPapel);
            Rectangle rctQuimico = (Rectangle)btnQuimico.Template.FindName("rctQuimico", btnQuimico);
            Rectangle rctTinta = (Rectangle)btnTinta.Template.FindName("rctTinta", btnTinta);

            if (rctQuimico != null)
            {
                rctLinha1.Visibility = Visibility.Visible;
                rctLinha2.Visibility = Visibility.Visible;
                rctLinha3.Visibility = Visibility.Visible;
                rctLinha4.Visibility = Visibility.Visible;
                rctLinha5.Visibility = Visibility.Visible;
                rctLinha7.Visibility = Visibility.Visible;
                dtgGradePapel.Visibility = Visibility.Hidden;
                dtgGradeTinta.Visibility = Visibility.Hidden;
                dtgGradeQuimico.Visibility = Visibility.Visible;
                dtgPapelConcluido.Visibility = Visibility.Hidden;
                dtgTintaConcluido.Visibility = Visibility.Hidden;
                dtgQuimicoConcluido.Visibility = Visibility.Hidden;
                rctPapel.Visibility = Visibility.Hidden;
                rctQuimico.Visibility = Visibility.Visible;
                rctTinta.Visibility = Visibility.Hidden;
                btnApurarPapel.Visibility = Visibility.Hidden;
                btnApurarTinta.Visibility = Visibility.Hidden;
                btnApurarQuimico.Visibility = Visibility.Visible;
                btnImprimirEtiquetaPapel.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaQuimico.Visibility = Visibility.Visible;
                btnImprimirEtiquetaTinta.Visibility = Visibility.Hidden;
                btnApurarLotePapel.Visibility = Visibility.Hidden;
                btnApurarLoteTinta.Visibility = Visibility.Hidden;
                btnApurarLoteQuimico.Visibility = Visibility.Visible;
            }

            string query = "SELECT \"DsMaterial\", TO_CHAR(\"DtRecebimento\", 'DD/MM/YY') \"DtRecebimento\", TO_CHAR(\"HrRecebimento\", 'HH24:MI') \"HrRecebimento\", TO_CHAR (\"DtAmostragem\", 'DD/MM/YY') \"DtAmostragem\", TO_CHAR(\"HrAmostragem\", 'HH24:MI') \"HrAmostragem\", \"NrNF\", \"NmFornecedor\", \"IdLoteFornecedor\", \"IdRecebimento\" FROM \"T02_Recebimento\" WHERE \"DsStatus\" IN ('Laboratório');";

            DataTable tabela = db.ExecutarConsulta(query);

            dtgGradeQuimico.ItemsSource = tabela.DefaultView;
        }

        private void btnTinta_Click(object sender, RoutedEventArgs e)
        {
            btnPendente.Foreground = (Brush)new BrushConverter().ConvertFromString("White");
            btnPendente.Background = (Brush)new BrushConverter().ConvertFromString("#003C1E");

            btnConcluido.Foreground = (Brush)new BrushConverter().ConvertFromString("#003C1E");
            btnConcluido.Background = (Brush)new BrushConverter().ConvertFromString("White");

            Rectangle rctPapel = (Rectangle)btnPapel.Template.FindName("rctPapel", btnPapel);
            Rectangle rctQuimico = (Rectangle)btnQuimico.Template.FindName("rctQuimico", btnQuimico);
            Rectangle rctTinta = (Rectangle)btnTinta.Template.FindName("rctTinta", btnTinta);

            if (rctTinta != null)
            {
                rctLinha1.Visibility = Visibility.Visible;
                rctLinha2.Visibility = Visibility.Visible;
                rctLinha3.Visibility = Visibility.Visible;
                rctLinha4.Visibility = Visibility.Visible;
                rctLinha5.Visibility = Visibility.Visible;
                rctLinha7.Visibility = Visibility.Visible;
                dtgGradePapel.Visibility = Visibility.Hidden;
                dtgGradeTinta.Visibility = Visibility.Visible;
                dtgGradeQuimico.Visibility = Visibility.Hidden;
                dtgPapelConcluido.Visibility = Visibility.Hidden;
                dtgTintaConcluido.Visibility = Visibility.Hidden;
                dtgQuimicoConcluido.Visibility = Visibility.Hidden;
                rctPapel.Visibility = Visibility.Hidden;
                rctQuimico.Visibility = Visibility.Hidden;
                rctTinta.Visibility = Visibility.Visible;
                btnApurarPapel.Visibility = Visibility.Hidden;
                btnApurarTinta.Visibility = Visibility.Visible;
                btnApurarQuimico.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaPapel.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaQuimico.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaTinta.Visibility = Visibility.Visible;
                btnApurarLotePapel.Visibility = Visibility.Hidden;
                btnApurarLoteTinta.Visibility = Visibility.Visible;
                btnApurarLoteQuimico.Visibility = Visibility.Hidden;
            }

            string query = "SELECT RBT.\"CdTinta\", RBT.\"DsTinta\", RBT.\"NrNF\", RBT.\"NrQuantidade\", TO_CHAR(RBT.\"DtValidade\", 'DD/MM/YYYY') \"DtValidade\", TO_CHAR(RBT.\"DtFabricacao\",'DD/MM/YYYY') \"DtFabricacao\", RBT.\"NmFornecedor\", RBT.\"IdLoteFornecedor\" FROM \"T0203_RecebimentoTinta\" RBT;";

            DataTable tabela = db.ExecutarConsulta(query);

            dtgGradeTinta.ItemsSource = tabela.DefaultView;
        }

        #endregion

        #region Filtros
        private void txtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textoPesquisa = txtPesquisa.Text.ToLower();

            if (dtgGradePapel.Visibility == Visibility.Visible)
            {
                FiltrarDataGrid(dtgGradePapel, textoPesquisa);
            }
            else if (dtgGradeTinta.Visibility == Visibility.Visible)
            {
                FiltrarDataGrid(dtgGradeTinta, textoPesquisa);
            }
            else if (dtgGradeQuimico.Visibility == Visibility.Visible)
            {
                FiltrarDataGrid(dtgGradeQuimico, textoPesquisa);
            }
        }

        private void FiltrarDataGrid(DataGrid dataGrid, string textoPesquisa)
        {
            DataView dataView = dataGrid.ItemsSource as DataView;

            if (dataView != null)
            {
                if (string.IsNullOrEmpty(textoPesquisa))
                {
                    dataView.RowFilter = "";
                }
                else
                {
                    string filtro = "";
                    foreach (DataColumn column in dataView.Table.Columns)
                    {
                        if (filtro != "")
                            filtro += " OR ";

                        filtro += $"CONVERT([{column.ColumnName}], System.String) LIKE '%{textoPesquisa}%'";
                    }

                    dataView.RowFilter = filtro;
                }
            }
        }

        private void btnPendente_Click(object sender, RoutedEventArgs e)
        {
            btnPendente.Foreground = (Brush)new BrushConverter().ConvertFromString("White");
            btnPendente.Background = (Brush)new BrushConverter().ConvertFromString("#003C1E");

            btnConcluido.Foreground = (Brush)new BrushConverter().ConvertFromString("#003C1E");
            btnConcluido.Background = (Brush)new BrushConverter().ConvertFromString("White");

            if (dtgPapelConcluido.Visibility == Visibility.Visible)
            {
                btnPapel_Click(sender, e);
            }
            else if (dtgTintaConcluido.Visibility == Visibility.Visible)
            {
                btnTinta_Click(sender, e);
            }
            else if (dtgQuimicoConcluido.Visibility == Visibility.Visible)
            {
                btnQuimico_Click(sender, e);
            }
        }

        private void btnConcluido_Click(object sender, RoutedEventArgs e)
        {
            btnConcluido.Foreground = (Brush)new BrushConverter().ConvertFromString("White");
            btnConcluido.Background = (Brush)new BrushConverter().ConvertFromString("#003C1E");

            btnPendente.Foreground = (Brush)new BrushConverter().ConvertFromString("#003C1E");
            btnPendente.Background = (Brush)new BrushConverter().ConvertFromString("White");

            if (dtgGradePapel.Visibility == Visibility.Visible)
            {
                rctLinha1.Visibility = Visibility.Visible;
                rctLinha2.Visibility = Visibility.Visible;
                rctLinha3.Visibility = Visibility.Hidden;
                rctLinha4.Visibility = Visibility.Hidden;
                rctLinha5.Visibility = Visibility.Hidden;
                rctLinha7.Visibility = Visibility.Visible;
                btnApurarPapel.Visibility = Visibility.Hidden;
                btnApurarTinta.Visibility = Visibility.Hidden;
                btnApurarQuimico.Visibility = Visibility.Hidden;
                dtgGradePapel.Visibility = Visibility.Hidden;
                dtgGradeQuimico.Visibility = Visibility.Hidden;
                dtgPapelConcluido.Visibility = Visibility.Visible;
                dtgTintaConcluido.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaPapel.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaTinta.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaQuimico.Visibility = Visibility.Hidden;
                btnApurarLotePapel.Visibility = Visibility.Hidden;
                btnApurarLoteTinta.Visibility = Visibility.Hidden;

                string query = "SELECT RCB.\"IdRecebimento\", RCB.\"DsMaterial\", RAM.\"NrGramatura\", RAM.\"NrLargura\", RAM.\"StAlvejante\", RAM.\"NrTracaoSeca\", RAM.\"NrTracaoUmida\", RAM.\"NrPorosidadeGurley\", RAM.\"NrAbsorcaoCoob\", RAM.\"DsStatusAmostra\", RAM.\"NmResponsavel\", RAM.\"DsDesvio\" FROM \"T0305_ResultadoAmostra\" RAM INNER JOIN \"T02_Recebimento\" RCB ON RCB.\"IdRecebimento\" = RAM.\"IdRecebimento\" WHERE RCB.\"DsStatus\" IN('APROVADO', 'REPROVADO', 'aprovado', 'reprovado', 'Aprovado', 'Reprovado');";
                DataTable tabela = db.ExecutarConsulta(query);

                dtgPapelConcluido.ItemsSource = tabela.DefaultView;
            }
            else if (dtgGradeTinta.Visibility == Visibility.Visible)
            {
                rctLinha1.Visibility = Visibility.Visible;
                rctLinha2.Visibility = Visibility.Visible;
                rctLinha3.Visibility = Visibility.Hidden;
                rctLinha4.Visibility = Visibility.Hidden;
                rctLinha5.Visibility = Visibility.Hidden;
                rctLinha7.Visibility = Visibility.Visible;
                btnApurarPapel.Visibility = Visibility.Hidden;
                btnApurarTinta.Visibility = Visibility.Hidden;
                btnApurarQuimico.Visibility = Visibility.Hidden;
                dtgGradePapel.Visibility = Visibility.Hidden;
                dtgGradeQuimico.Visibility = Visibility.Hidden;
                dtgGradeTinta.Visibility = Visibility.Hidden;
                dtgPapelConcluido.Visibility = Visibility.Hidden;
                dtgTintaConcluido.Visibility = Visibility.Visible;
                btnImprimirEtiquetaPapel.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaTinta.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaQuimico.Visibility = Visibility.Hidden;
                btnApurarLotePapel.Visibility = Visibility.Hidden;
                btnApurarLoteTinta.Visibility = Visibility.Hidden;

                string query = "SELECT RBT.\"IdRecebimento\", RBT.\"CdTinta\", RBT.\"DsTinta\", RBT.\"IdLoteFornecedor\", RBT.\"NmFornecedor\", AMT.\"StCor\", AMT.\"NrPesoDoVolume\", AMT.\"NrViscosidade\", AMT.\"DsStatusAmostra\", AMT.\"NmInspetor\" FROM \"T0203_RecebimentoTinta\" RBT LEFT JOIN \"T0306_AmostraTinta\" AMT ON AMT.\"IdRecebimento\" = RBT.\"IdRecebimento\" WHERE RBT.\"DsStatus\" IN ('APROVADO', 'REPROVADO', 'aprovado', 'reprovado', 'Aprovado', 'Reprovado');";

                DataTable tabela = db.ExecutarConsulta(query);

                dtgTintaConcluido.ItemsSource = tabela.DefaultView;
            }
            else if (dtgGradeQuimico.Visibility == Visibility.Visible)
            {
                rctLinha1.Visibility = Visibility.Visible;
                rctLinha2.Visibility = Visibility.Visible;
                rctLinha3.Visibility = Visibility.Hidden;
                rctLinha4.Visibility = Visibility.Hidden;
                rctLinha5.Visibility = Visibility.Hidden;
                rctLinha7.Visibility = Visibility.Visible;
                btnApurarPapel.Visibility = Visibility.Hidden;
                btnApurarTinta.Visibility = Visibility.Hidden;
                btnApurarQuimico.Visibility = Visibility.Hidden;
                dtgGradePapel.Visibility = Visibility.Hidden;
                dtgGradeQuimico.Visibility = Visibility.Visible;
                dtgPapelConcluido.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaPapel.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaTinta.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaQuimico.Visibility = Visibility.Hidden;
                dtgGradeTinta.Visibility = Visibility.Visible;
                dtgPapelConcluido.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaPapel.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaTinta.Visibility = Visibility.Hidden;
                btnImprimirEtiquetaQuimico.Visibility = Visibility.Hidden;
                btnApurarLotePapel.Visibility = Visibility.Hidden;
                btnApurarLoteTinta.Visibility = Visibility.Hidden;

                string query = "";

                DataTable tabela = db.ExecutarConsulta(query);

                dtgPapelConcluido.ItemsSource = tabela.DefaultView;
            }
        }

        #endregion

        #region Açoes

        private void btnApurarPapel_Click(object sender, RoutedEventArgs e)
        {
            CF0303 apurar = new CF0303();
            apurar.ShowDialog();
            this.Close();
        }

        private void btnImprimirEtiquetaPapel_Click(object sender, RoutedEventArgs e)
        {
            CF0311 imprimirEtiqueta = new CF0311();
            imprimirEtiqueta.ShowDialog();
            this.Close();
        }

        private void btnApurarLotePapel_Click(object sender, RoutedEventArgs e)
        {
            CF0314 apurarlote = new CF0314();
            apurarlote.ShowDialog();
            this.Close();
        }

        private void btnApurarLoteTinta_Click(object sender, RoutedEventArgs e)
        {
            CF0314 apurarlote = new CF0314();
            apurarlote.ShowDialog();
            this.Close();
        }

        private void btnApurarLoteQuimico_Click(object sender, RoutedEventArgs e)
        {
            CF0314 apurarlote = new CF0314();
            apurarlote.ShowDialog();
            this.Close();
        }

        private void btnApurarTinta_Click(object sender, RoutedEventArgs e)
        {
            CF0316 apurarTinta = new CF0316();
            apurarTinta.ShowDialog();
            this.Close();
        }

        private void btnImprimirEtiquetaTinta_Click(object sender, RoutedEventArgs e)
        {
            CF0311 imprimirEtiquetaTinta = new CF0311();
            imprimirEtiquetaTinta.ShowDialog();
            this.Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0301 moduloLaboratorio = new CF0301();
            moduloLaboratorio.Show();
            this.Close();
        }

        #endregion
    }
}