using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0203 : Window
    {
        private readonly Consultar db;
        private readonly string usuarioId;

        public class RecebimentoModel
        {
            public string DsMaterial { get; set; }
            public string DsMaterialOriginal { get; set; }

            public string DtRecebimento { get; set; }
            public string DtRecebimentoOriginal { get; set; }

            public string HrRecebimento { get; set; }
            public string HrRecebimentoOriginal { get; set; }

            public string NrNF { get; set; }
            public string NrNFOriginal { get; set; }

            public string NmFornecedor { get; set; }
            public string NmFornecedorOriginal { get; set; }

            public string IdLoteFornecedor { get; set; }
            public string IdLoteFornecedorOriginal { get; set; }

            public string NrPeso { get; set; }
            public string NrPesoOriginal { get; set; }

            public string IdRecebimento { get; set; }
        }

        public CF0203()
        {
            InitializeComponent();
            db = new Consultar();
            CarregarDadosAsync();
        }

        private async void CarregarDadosAsync()
        {
            string query = "SELECT RCB.\"IdRecebimento\", RCB.\"DsMaterial\", TO_CHAR(RCB.\"DtRecebimento\", 'DD/MM/YYYY') \"DtRecebimento\", TO_CHAR(RCB.\"HrRecebimento\", 'HH24:MI') \"HrRecebimento\", RCB.\"NrNF\", RCB.\"IdLoteFornecedor\", RCB.\"NmFornecedor\", RCB.\"NrPeso\" FROM \"T02_Recebimento\" RCB WHERE RCB.\"DsStatus\" IN ('Pendente');";

            try
            {
                DataTable tabela = await Task.Run(() => db.ExecutarConsulta(query));
                List<RecebimentoModel> recebimentos = new List<RecebimentoModel>();

                foreach (DataRow row in tabela.Rows)
                {
                    recebimentos.Add(new RecebimentoModel
                    {
                        DsMaterial = row["DsMaterial"].ToString(),
                        DsMaterialOriginal = row["DsMaterial"].ToString(),
                        DtRecebimento = row["DtRecebimento"].ToString(),
                        DtRecebimentoOriginal = row["DtRecebimento"].ToString(),
                        HrRecebimento = row["HrRecebimento"].ToString(),
                        HrRecebimentoOriginal = row["HrRecebimento"].ToString(),
                        IdLoteFornecedor = row["IdLoteFornecedor"].ToString(),
                        IdLoteFornecedorOriginal = row["IdLoteFornecedor"].ToString(),
                        NmFornecedor = row["NmFornecedor"].ToString(),
                        NmFornecedorOriginal = row["NmFornecedor"].ToString(),
                        NrNF = row["NrNF"].ToString(),
                        NrNFOriginal = row["NrNF"].ToString(),
                        NrPeso = row["NrPeso"].ToString(),
                        NrPesoOriginal = row["NrPeso"].ToString(),
                        IdRecebimento = row["IdRecebimento"].ToString()
                    });
                }

                dtgGrade.ItemsSource = new ObservableCollection<RecebimentoModel>(recebimentos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = (ObservableCollection<RecebimentoModel>)dtgGrade.ItemsSource;

            if (itemsSource != null)
            {
                try
                {
                    await AtualizarDadosAsync(itemsSource);
                    CF0201 recebimento = new CF0201(usuarioId);
                    recebimento.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao atualizar dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("A fonte de dados não é uma ObservableCollection ou tipo esperado. Por favor, verifique!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AtualizarDadosAsync(ObservableCollection<RecebimentoModel> itemsSource)
        {
            using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
            {
                await conn.OpenAsync();

                foreach (var item in itemsSource)
                {
                    if (item.DsMaterial != item.DsMaterialOriginal ||
                        item.DtRecebimento != item.DtRecebimentoOriginal ||
                        item.HrRecebimento != item.HrRecebimentoOriginal ||
                        item.NrNF != item.NrNFOriginal ||
                        item.NrPeso != item.NrPesoOriginal ||
                        item.IdLoteFornecedor != item.IdLoteFornecedorOriginal ||
                        item.NmFornecedor != item.NmFornecedorOriginal)
                    {
                        string comando = "UPDATE \"T02_Recebimento\" SET " +
                                         "\"DsMaterial\" = @DsMaterial, " +
                                         "\"DtRecebimento\" = @DtRecebimento, " +
                                         "\"HrRecebimento\" = @HrRecebimento, " +
                                         "\"NrNF\" = @NrNF, " +
                                         "\"IdLoteFornecedor\" = @IdLoteFornecedor, " +
                                         "\"NrPeso\" = @NrPeso, " +
                                         "\"NmFornecedor\" = @NmFornecedor " +
                                         "WHERE \"IdRecebimento\" = @IdRecebimento;";

                        using (var cmd = new NpgsqlCommand(comando, conn))
                        {
                            cmd.Parameters.AddWithValue("DsMaterial", string.IsNullOrEmpty(item.DsMaterial) ? DBNull.Value : item.DsMaterial);
                            cmd.Parameters.AddWithValue("DtRecebimento", DateTime.TryParse(item.DtRecebimento, out DateTime dtRecebimento) ? dtRecebimento : DBNull.Value);
                            cmd.Parameters.AddWithValue("HrRecebimento", DateTime.TryParse(item.HrRecebimento, out DateTime hrRecebimento) ? hrRecebimento : DBNull.Value);
                            cmd.Parameters.AddWithValue("NrNF", int.TryParse(item.NrNF, out int nrNF) ? nrNF : DBNull.Value);
                            cmd.Parameters.AddWithValue("IdLoteFornecedor", string.IsNullOrEmpty(item.IdLoteFornecedor) ? DBNull.Value : item.IdLoteFornecedor);
                            cmd.Parameters.AddWithValue("NmFornecedor", string.IsNullOrEmpty(item.NmFornecedor) ? DBNull.Value : item.NmFornecedor);
                            cmd.Parameters.AddWithValue("IdRecebimento", int.TryParse(item.IdRecebimento, out int idRecebimento) ? idRecebimento : DBNull.Value);
                            cmd.Parameters.AddWithValue("NrPeso", double.TryParse(item.NrPeso, out double nrPeso) ? nrPeso : DBNull.Value);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }

        private void txtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textoPesquisa = txtPesquisa.Text.ToLower();

            FiltrarDataGrid(dtgGrade, textoPesquisa);
        }

        private void FiltrarDataGrid(DataGrid dataGrid, string textoPesquisa)
        {
            var colecaoOriginal = dataGrid.ItemsSource as ObservableCollection<RecebimentoModel>;
            if (colecaoOriginal == null) return;

            if (string.IsNullOrEmpty(textoPesquisa))
            {
                dataGrid.ItemsSource = new ObservableCollection<RecebimentoModel>(colecaoOriginal);
            }
            else
            {
                var filtrado = new ObservableCollection<RecebimentoModel>(
                    colecaoOriginal.Where(item =>
                        item.DsMaterial?.ToLower().Contains(textoPesquisa) == true ||
                        item.DtRecebimento?.ToLower().Contains(textoPesquisa) == true ||
                        item.HrRecebimento?.ToLower().Contains(textoPesquisa) == true ||
                        item.NrNF?.ToLower().Contains(textoPesquisa) == true ||
                        item.IdLoteFornecedor?.ToLower().Contains(textoPesquisa) == true ||
                        item.NmFornecedor?.ToLower().Contains(textoPesquisa) == true ||
                        item.NrPeso?.ToLower().Contains(textoPesquisa) == true ||
                        item.IdRecebimento?.ToLower().Contains(textoPesquisa) == true
                    ));

                dataGrid.ItemsSource = filtrado;
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0201 recebimento = new CF0201(usuarioId);
            recebimento.Show();
            this.Close();
        }
    }
}