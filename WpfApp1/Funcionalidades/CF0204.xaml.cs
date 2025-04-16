using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades;

public partial class CF0204 : Window
{
    private readonly Consultar db;
    private readonly string usuarioId;

    public class RecebimentoModel
    {
        public string DsMaterial { get; set; }
        public string DtRecebimento { get; set; }
        public string HrRecebimento { get; set; }
        public string DtAmostragem { get; set; }
        public string HrAmostragem { get; set; }
        public string IdLoteFornecedor { get; set; }
        public string NmFornecedor { get; set; }
        public string NrNF { get; set; }
        public string IdRecebimento { get; set; }
        public bool IsSelected { get; set; }
    }

    public CF0204()
    {
        InitializeComponent();
        db = new Consultar();
        CarregarDadosAsync();
    }

    private async void CarregarDadosAsync()
    {
        string query = "SELECT RCB.\"IdRecebimento\", RCB.\"DsMaterial\", TO_CHAR(RCB.\"DtRecebimento\", 'DD/MM/YYYY') \"DtRecebimento\", TO_CHAR(RCB.\"HrRecebimento\", 'HH24:MI') \"HrRecebimento\", TO_CHAR (RCB.\"DtAmostragem\", 'DD/MM/YYYY') \"DtAmostragem\", TO_CHAR(RCB.\"HrAmostragem\", 'HH24:MI') \"HrAmostragem\", RCB.\"NrNF\", RCB.\"IdLoteFornecedor\", RCB.\"NmFornecedor\" FROM \"T02_Recebimento\" RCB WHERE RCB.\"DsStatus\" IN ('Pendente');";

        try
        {
            DataTable tabela = await Task.Run(() => db.ExecutarConsulta(query));
            List<RecebimentoModel> recebimentos = new List<RecebimentoModel>();

            foreach (DataRow row in tabela.Rows)
            {
                recebimentos.Add(new RecebimentoModel
                {
                    DsMaterial = row["DsMaterial"].ToString(),
                    DtRecebimento = row["DtRecebimento"].ToString(),
                    HrRecebimento = row["HrRecebimento"].ToString(),
                    DtAmostragem = DateTime.Now.ToShortDateString(),
                    HrAmostragem = DateTime.Now.ToString("HH:mm"),
                    IdLoteFornecedor = row["IdLoteFornecedor"].ToString(),
                    NmFornecedor = row["NmFornecedor"].ToString(),
                    NrNF = row["NrNF"].ToString(),
                    IdRecebimento = row["IdRecebimento"].ToString(),
                    IsSelected = false
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

            var selectedItems = itemsSource.Where(i => i.IsSelected).ToList();

            if (selectedItems.Any())
            {
                foreach (var item in selectedItems)
                {
                    string comando = "UPDATE \"T02_Recebimento\" SET " +
                                     "\"DsStatus\" = 'Laboratório', " +
                                     "\"DtAmostragem\" = @DtAmostragem, " +
                                     "\"HrAmostragem\" = @HrAmostragem " +
                                     "WHERE \"IdRecebimento\" = @IdRecebimento;";

                    using (var cmd = new NpgsqlCommand(comando, conn))
                    {
                        cmd.Parameters.AddWithValue("IdRecebimento", int.TryParse(item.IdRecebimento, out int idRecebimento) ? idRecebimento : DBNull.Value);
                        cmd.Parameters.AddWithValue("DtAmostragem", DateTime.TryParse(item.DtAmostragem, out DateTime dtAmostragem) ? dtAmostragem : DBNull.Value);
                        cmd.Parameters.AddWithValue("HrAmostragem", DateTime.TryParse(item.HrAmostragem, out DateTime hrAmostragem) ? hrAmostragem : DBNull.Value);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            else
            {
                MessageBox.Show("Nenhum registro foi selecionado para enviar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    private void btnVoltar_Click(object sender, RoutedEventArgs e)
    {
        CF0201 recebimento = new CF0201(usuarioId);
        recebimento.Show();
        this.Close();
    }
}