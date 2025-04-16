using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0205 : Window
    {
        private readonly string usuarioId;
        private readonly Consultar db;

        public class RecebimentoModel
        {
            public string DsMaterial { get; set; }
            public string DsMaterialOriginal { get; set; }

            public string DtRecebimento { get; set; }
            public string DtRecebimentoOriginal { get; set; }

            public string HrRecebimento { get; set; }
            public string HrRecebimentoOriginal { get; set; }

            public string DtAmostragem { get; set; }
            public string DtAmostragemOriginal { get; set; }

            public string HrAmostragem { get; set; }
            public string HrAmostragemOriginal { get; set; }

            public string NrNF { get; set; }
            public string NrNFOriginal { get; set; }

            public string NmFornecedor { get; set; }
            public string NmFornecedorOriginal { get; set; }

            public string NrPeso { get; set; }
            public string NrPesoOriginal { get; set; }

            public string IdRecebimento { get; set; }
        }

        public CF0205()
        {
            InitializeComponent();
            db = new Consultar();
            CarregarDadosAsync();
        }

        private async void CarregarDadosAsync()
        {
            string query = "SELECT RCB.\"IdRecebimento\", RCB.\"DsMaterial\", TO_CHAR(RCB.\"DtRecebimento\", 'DD/MM/YYYY') \"DtRecebimento\", TO_CHAR(RCB.\"HrRecebimento\", 'HH24:MI') \"HrRecebimento\", TO_CHAR (RCB.\"DtAmostragem\", 'DD/MM/YYYY') \"DtAmostragem\", TO_CHAR(RCB.\"HrAmostragem\", 'HH24:MI') \"HrAmostragem\", RCB.\"NrNF\", RCB.\"IdRecebimento\", RCB.\"NmFornecedor\", RCB.\"NrPeso\" FROM \"T02_Recebimento\" RCB WHERE RCB.\"DsStatus\" IN ('Aprovado');";

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
                        NrPeso = row["NrPeso"].ToString(),
                        NrPesoOriginal = row["NrPeso"].ToString(),
                        DtAmostragem = row["DtAmostragem"].ToString(),
                        DtAmostragemOriginal = row["DtAmostragem"].ToString(),
                        HrAmostragem = row["HrAmostragem"].ToString(),
                        HrAmostragemOriginal = row["HrAmostragem"].ToString(),
                        NrNF = row["NrNF"].ToString(),
                        NrNFOriginal = row["NrNF"].ToString(),
                        IdRecebimento = row["IdRecebimento"].ToString(),
                        NmFornecedor = row["NmFornecedor"].ToString(),
                        NmFornecedorOriginal = row["NmFornecedor"].ToString()
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
                    string comando = @"
                        UPDATE public.""T02_Recebimento""
                        SET ""DsStatus"" = 'Estoque'
                        WHERE ""IdRecebimento"" = @IdRecebimento;";

                    using (var cmd = new NpgsqlCommand(comando, conn))
                    {
                        cmd.Parameters.AddWithValue("IdRecebimento", int.TryParse(item.IdRecebimento, out int idRecebimento) ? idRecebimento : DBNull.Value);
                        await cmd.ExecuteNonQueryAsync();
                    }
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
}