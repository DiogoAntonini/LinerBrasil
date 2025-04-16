using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0202 : Window
    {
        private readonly string usuarioId;

        public ObservableCollection<DadoEntrada> Dados { get; set; }

        public class DadoEntrada
        {
            public string DsMaterial { get; set; }
            public string DtRecebimento { get; set; }
            public string HrRecebimento { get; set; }
            public string NrNF { get; set; }
            public string NmFornecedor { get; set; }
            public string IdLoteFornecedor { get; set; }
            public string NrPeso { get; set; }
        }

        public CF0202(string usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            Dados = new ObservableCollection<DadoEntrada>();
            dtgGrade.ItemsSource = Dados;

            dtgGrade.PreviewKeyDown += DataGrid_PreviewKeyDown;
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                PasteDataFromClipboard();
            }
        }

        private void PasteDataFromClipboard()
        {
            var clipboardText = Clipboard.GetText();
            var rows = clipboardText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var row in rows)
            {
                var columns = row.Split('\t');
                if (columns.Length == 7)
                {
                    var dadoEntrada = new DadoEntrada
                    {
                        DsMaterial = columns[0],
                        DtRecebimento = columns[1],
                        HrRecebimento = columns[2],
                        NrNF = columns[3],
                        IdLoteFornecedor = columns[4],
                        NmFornecedor = columns[5],
                        NrPeso = columns[6]
                    };
                    Dados.Add(dadoEntrada);
                }
            }
        }

        private async void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = (ObservableCollection<DadoEntrada>)dtgGrade.ItemsSource;

            if (itemsSource != null)
            {
                try
                {
                    await InserirDadosAsync(itemsSource);
                    CF0201 recebimento = new CF0201(usuarioId);
                    recebimento.Show();
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao inserir dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("A fonte de dados não é uma ObservableCollection ou tipo esperado. Por favor, verifique!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task InserirDadosAsync(ObservableCollection<DadoEntrada> itemsSource)
        {
            using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
            {
                await conn.OpenAsync();

                foreach (var item in itemsSource)
                {
                    string comando = "INSERT INTO public.\"T02_Recebimento\" (\"DsMaterial\",\"DtRecebimento\",\"HrRecebimento\",\"NrNF\",\"NmFornecedor\",\"IdLoteFornecedor\",\"NrPeso\")" +
                                     "VALUES (@DsMaterial, @DtRecebimento, @HrRecebimento, @NrNF, @NmFornecedor, @IdLoteFornecedor, @NrPeso)";

                    using (var cmdInsert = new NpgsqlCommand(comando, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("DsMaterial", string.IsNullOrEmpty(item.DsMaterial) ? DBNull.Value : item.DsMaterial);
                        cmdInsert.Parameters.AddWithValue("DtRecebimento", DateTime.TryParse(item.DtRecebimento, out DateTime dtRecebimento) ? dtRecebimento : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("HrRecebimento", DateTime.TryParse(item.HrRecebimento, out DateTime hrRecebimento) ? hrRecebimento : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("NrPeso", int.TryParse(item.NrPeso, out int nrPeso) ? nrPeso : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("NrNF", int.TryParse(item.NrNF, out int nrNF) ? nrNF : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("IdLoteFornecedor", string.IsNullOrEmpty(item.IdLoteFornecedor) ? DBNull.Value : item.IdLoteFornecedor);
                        cmdInsert.Parameters.AddWithValue("NmFornecedor", string.IsNullOrEmpty(item.NmFornecedor) ? DBNull.Value : item.NmFornecedor);

                        await cmdInsert.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0201 recebimento = new CF0201(usuarioId);
            recebimento.Show();
            Close();
        }
    }
}