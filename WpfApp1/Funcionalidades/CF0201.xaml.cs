using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0201 : Window
    {
        private readonly string usuarioId;
        private readonly Consultar db;
        private DataTable tabelaGramatura;

        public CF0201(string usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            db = new Consultar();
            CarregarPerfilDoUsuarioAsync();
        }

        private async Task CarregarPerfilDoUsuarioAsync()
        {
            try
            {
                string queryperfil = "SELECT \"DsPerfil\" FROM \"T00_Configuracoes\" WHERE \"IdUsuario\" = @usuario;";
                var parametros = new Dictionary<string, object>
                {
                    { "@usuario", usuarioId }
                };

                DataTable tabela = await Task.Run(() => db.ExecutarConsultaComParametros(queryperfil, parametros));

                if (tabela.Rows.Count > 0)
                {
                    string perfil = tabela.Rows[0]["DsPerfil"].ToString();

                    switch (perfil)
                    {
                        case "Recebimento":
                            btnVoltar.Visibility = Visibility.Hidden;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar perfil do usuário: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnCadastro_Click(this, new RoutedEventArgs());
        }

        private void txtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textoPesquisa = txtPesquisa.Text.ToLower();

            if (dtgGradeRecebimento.Visibility == Visibility.Visible)
            {
                FiltrarDataGrid(dtgGradeRecebimento, textoPesquisa);
            }
            else if (dtgGradeCadastro.Visibility == Visibility.Visible)
            {
                FiltrarDataGrid(dtgGradeCadastro, textoPesquisa);
            }
            else if (dtgMateriasParaAnalise.Visibility == Visibility.Visible)
            {
                FiltrarDataGrid(dtgMateriasParaAnalise, textoPesquisa);
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

        private void dpDataInicial_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltroPorDataAsync();
        }

        private void dpDataFinal_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltroPorDataAsync();
        }

        private async Task AplicarFiltroPorDataAsync()
        {
            if (dpDataInicial.SelectedDate == null || dpDataFinal.SelectedDate == null)
                return;

            DateTime dataInicio = dpDataInicial.SelectedDate.Value;
            DateTime dataFim = dpDataFinal.SelectedDate.Value;

            try
            {
                if (dtgGradeRecebimento.Visibility == Visibility.Visible)
                {
                    string query = "SELECT RCB.\"IdRecebimento\", RCB.\"DsMaterial\", TO_CHAR(RCB.\"DtRecebimento\", 'DD/MM/YY') \"DtRecebimento\", TO_CHAR(RCB.\"HrRecebimento\", 'HH24:MI') \"HrRecebimento\", TO_CHAR (RCB.\"DtAmostragem\", 'DD/MM/YY') \"DtAmostragem\", TO_CHAR(RCB.\"HrAmostragem\", 'HH24:MI') \"HrAmostragem\", RCB.\"NrNF\", RCB.\"IdLoteFornecedor\", RCB.\"NmFornecedor\" FROM \"T02_Recebimento\" RCB WHERE RCB.\"DsStatus\" IN ('Pendente', 'Aprovado') AND RCB.\"DtRecebimento\" BETWEEN @dataInicio AND @dataFim;";

                    var parametros = new Dictionary<string, object>
                    {
                        { "@dataInicio", dataInicio },
                        { "@dataFim", dataFim }
                    };

                    tabelaGramatura = await Task.Run(() => db.ExecutarConsultaComParametros(query, parametros));
                    dtgGradeRecebimento.ItemsSource = tabelaGramatura.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao aplicar filtro por data: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnCadastro_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rctCadastro = (Rectangle)btnCadastro.Template.FindName("rctCadastro", btnCadastro);
            Rectangle rctMateriasParaAnalise = (Rectangle)btnMateriasParaAnalise.Template.FindName("rctMateriasParaAnalise", btnMateriasParaAnalise);
            Rectangle rctRecebimento = (Rectangle)btnRecebimento.Template.FindName("rctRecebimento", btnRecebimento);

            if (rctCadastro != null)
            {
                txtDtInicial.Visibility = Visibility.Hidden;
                dpDataInicial.Visibility = Visibility.Hidden;
                txtDtFinal.Visibility = Visibility.Hidden;
                dpDataFinal.Visibility = Visibility.Hidden;
                txtPesquisa.Visibility = Visibility.Visible;
                txPesquisa.Visibility = Visibility.Visible;
                txtPesquisa.Margin = new Thickness(1131, 118, 0, 0);
                txPesquisa.Margin = new Thickness(1076, 119, 0, 0);
                btnAdicionarMaterial.Visibility = Visibility.Visible;
                btnAdicionarRomaneio.Visibility = Visibility.Hidden;
                btnAlterarMaterial.Visibility = Visibility.Visible;
                btnExcluirMaterial.Visibility = Visibility.Visible;
                btnEnviarEstoque.Visibility = Visibility.Hidden;
                btnAlterarRomaneio.Visibility = Visibility.Hidden;
                btnEnviarAmostra.Visibility = Visibility.Hidden;
                dtgGradeRecebimento.Visibility = Visibility.Hidden;
                dtgGradeCadastro.Visibility = Visibility.Visible;
                dtgMateriasParaAnalise.Visibility = Visibility.Hidden;
                rctMateriasParaAnalise.Visibility = Visibility.Hidden;
                rctRecebimento.Visibility = Visibility.Hidden;
                rctCadastro.Visibility = Visibility.Visible;
                rctLinha3.Visibility = Visibility.Visible;
                rctLinha4.Visibility = Visibility.Visible;
                rctLinha5.Visibility = Visibility.Visible;
                rctLinha6.Visibility = Visibility.Hidden;
            }

            try
            {
                string query = "SELECT CMR.\"NmDescricao\", CMR.\"DsUnidade\", CMR.\"StAnalise\" FROM \"T0201_CadastroMateriaRecebimento\" CMR;";
                DataTable tabela = await Task.Run(() => db.ExecutarConsulta(query));
                dtgGradeCadastro.ItemsSource = tabela.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar cadastro: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnMateriasParaAnalise_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rctCadastro = (Rectangle)btnCadastro.Template.FindName("rctCadastro", btnCadastro);
            Rectangle rctMateriasParaAnalise = (Rectangle)btnMateriasParaAnalise.Template.FindName("rctMateriasParaAnalise", btnMateriasParaAnalise);
            Rectangle rctRecebimento = (Rectangle)btnRecebimento.Template.FindName("rctRecebimento", btnRecebimento);

            if (rctMateriasParaAnalise != null)
            {
                txtDtInicial.Visibility = Visibility.Hidden;
                dpDataInicial.Visibility = Visibility.Hidden;
                txtDtFinal.Visibility = Visibility.Hidden;
                dpDataFinal.Visibility = Visibility.Hidden;
                txtPesquisa.Visibility = Visibility.Visible;
                txPesquisa.Visibility = Visibility.Visible;
                txtPesquisa.Margin = new Thickness(1131, 118, 0, 0);
                txPesquisa.Margin = new Thickness(1076, 119, 0, 0);
                btnEnviarEstoque.Visibility = Visibility.Hidden;
                btnEnviarAmostra.Visibility = Visibility.Hidden;
                btnAdicionarMaterial.Visibility = Visibility.Hidden;
                btnAdicionarRomaneio.Visibility = Visibility.Hidden;
                btnAlterarRomaneio.Visibility = Visibility.Hidden;
                btnAlterarRomaneio.Visibility = Visibility.Hidden;
                btnAlterarMaterial.Visibility = Visibility.Hidden;
                btnExcluirMaterial.Visibility = Visibility.Hidden;
                dtgGradeRecebimento.Visibility = Visibility.Hidden;
                dtgGradeCadastro.Visibility = Visibility.Hidden;
                dtgMateriasParaAnalise.Visibility = Visibility.Visible;
                rctMateriasParaAnalise.Visibility = Visibility.Visible;
                rctCadastro.Visibility = Visibility.Hidden;
                rctRecebimento.Visibility = Visibility.Hidden;
                rctLinha3.Visibility = Visibility.Hidden;
                rctLinha4.Visibility = Visibility.Hidden;
                rctLinha5.Visibility = Visibility.Hidden;
                rctLinha6.Visibility = Visibility.Hidden;
            }

            try
            {
                string query = "SELECT CMR.\"NmDescricao\", CMR.\"DsUnidade\", CMR.\"StAnalise\" FROM \"T0201_CadastroMateriaRecebimento\" CMR WHERE CMR.\"StAnalise\" = TRUE;";
                DataTable tabela = await Task.Run(() => db.ExecutarConsulta(query));
                dtgMateriasParaAnalise.ItemsSource = tabela.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar matérias para análise: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnRecebimento_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rctCadastro = (Rectangle)btnCadastro.Template.FindName("rctCadastro", btnCadastro);
            Rectangle rctMateriasParaAnalise = (Rectangle)btnMateriasParaAnalise.Template.FindName("rctMateriasParaAnalise", btnMateriasParaAnalise);
            Rectangle rctRecebimento = (Rectangle)btnRecebimento.Template.FindName("rctRecebimento", btnRecebimento);

            if (rctRecebimento != null)
            {
                txtDtInicial.Visibility = Visibility.Visible;
                dpDataInicial.Visibility = Visibility.Visible;
                txtDtFinal.Visibility = Visibility.Visible;
                dpDataFinal.Visibility = Visibility.Visible;
                txtDtInicial.Margin = new Thickness(794, 118, 0, 0);
                dpDataInicial.Margin = new Thickness(859, 116, 267, 0);
                txtDtFinal.Margin = new Thickness(1038, 119, 0, 0);
                dpDataFinal.Margin = new Thickness(1098, 114, 28, 0);
                txPesquisa.Margin = new Thickness(596, 119, 0, 0);
                txtPesquisa.Margin = new Thickness(651, 118, 0, 0);
                btnEnviarAmostra.Visibility = Visibility.Visible;
                btnAdicionarRomaneio.Visibility = Visibility.Visible;
                btnAdicionarMaterial.Visibility = Visibility.Hidden;
                btnEnviarEstoque.Visibility = Visibility.Visible;
                btnAlterarMaterial.Visibility = Visibility.Hidden;
                btnAlterarRomaneio.Visibility = Visibility.Visible;
                btnExcluirMaterial.Visibility = Visibility.Hidden;
                rctLinha6.Visibility = Visibility.Visible;
                rctLinha5.Visibility = Visibility.Visible;
                rctCadastro.Visibility = Visibility.Hidden;
                rctMateriasParaAnalise.Visibility = Visibility.Hidden;
                rctRecebimento.Visibility = Visibility.Visible;
                dtgGradeRecebimento.Visibility = Visibility.Visible;
                dtgGradeCadastro.Visibility = Visibility.Hidden;
                dtgMateriasParaAnalise.Visibility = Visibility.Hidden;
                rctLinha3.Visibility = Visibility.Visible;
                rctLinha4.Visibility = Visibility.Visible;
                rctLinha5.Visibility = Visibility.Visible;
                rctLinha6.Visibility = Visibility.Visible;
            }

            try
            {
                string query = "SELECT RCB.\"IdRecebimento\", RCB.\"DsMaterial\", TO_CHAR(RCB.\"DtRecebimento\", 'DD/MM/YY') \"DtRecebimento\", TO_CHAR(RCB.\"HrRecebimento\", 'HH24:MI') \"HrRecebimento\", TO_CHAR (RCB.\"DtAmostragem\", 'DD/MM/YY') \"DtAmostragem\", TO_CHAR(RCB.\"HrAmostragem\", 'HH24:MI') \"HrAmostragem\", RCB.\"NrNF\", RCB.\"IdLoteFornecedor\", RCB.\"NmFornecedor\", RCB.\"NrPeso\" FROM \"T02_Recebimento\" RCB WHERE RCB.\"DsStatus\" IN ('Pendente', 'Aprovado');";
                tabelaGramatura = await Task.Run(() => db.ExecutarConsulta(query));
                dtgGradeRecebimento.ItemsSource = tabelaGramatura.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar recebimento: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdicionarRomaneio_Click(object sender, RoutedEventArgs e)
        {
            CF0202 adicionar = new CF0202(usuarioId);
            adicionar.ShowDialog();
            Close();
        }

        private void btnAdicionarMaterial_Click(object sender, RoutedEventArgs e)
        {
            CF0206 adicionar = new CF0206(usuarioId);
            adicionar.ShowDialog();
            Close();
        }

        private void btnAlterarMaterial_Click(object sender, RoutedEventArgs e)
        {
            CF0207 alterar = new CF0207(usuarioId);
            alterar.ShowDialog();
            Close();
        }

        private void btnAlterarRomaneio_Click(object sender, RoutedEventArgs e)
        {
            CF0203 alterar = new CF0203();
            alterar.ShowDialog();
            Close();
        }

        private void btnExcluirMaterial_Click(object sender, RoutedEventArgs e)
        {
            CF0208 excluir = new CF0208(usuarioId);
            excluir.ShowDialog();
            Close();
        }

        private void btnEnviarAmostra_Click(object sender, RoutedEventArgs e)
        {
            CF0204 enviarAnalise = new CF0204();
            enviarAnalise.ShowDialog();
            Close();
        }

        private void btnEnviarEstoque_Click(object sender, RoutedEventArgs e)
        {
            CF0205 enviarEstoque = new CF0205();
            enviarEstoque.ShowDialog();
            Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0002 modulos = new CF0002(usuarioId);
            modulos.Show();
            Close();
        }
    }
}