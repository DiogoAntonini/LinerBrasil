using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0206 : Window
    {
        private readonly string usuarioId;

        public ObservableCollection<DadoEntrada> Dados { get; set; }

        public class DadoEntrada
        {
            public string NmDescricao { get; set; }
            public string DsUnidade { get; set; }
            public string StAnalise { get; set; }
        }

        public CF0206(string usuarioId)
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
                if (columns.Length == 2)
                {
                    if (!string.IsNullOrEmpty(columns[0]))
                    {
                        var dadoEntrada = new DadoEntrada
                        {
                            NmDescricao = columns[0],
                            DsUnidade = columns[1]
                        };

                        bool isDuplicate = Dados.Any(d => d.NmDescricao == dadoEntrada.NmDescricao && d.DsUnidade == dadoEntrada.DsUnidade);

                        if (!isDuplicate)
                        {
                            Dados.Add(dadoEntrada);
                        }
                    }
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
                    CF0201 main = new CF0201(usuarioId);
                    main.Show();
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
                    if (!string.IsNullOrEmpty(item.NmDescricao))
                    {
                        string comando = "INSERT INTO \"T0201_CadastroMateriaRecebimento\" (\"NmDescricao\", \"DsUnidade\", \"StAnalise\")" +
                                         "VALUES (@NmDescricao, @DsUnidade, @StAnalise)";

                        using (var cmdInsert = new NpgsqlCommand(comando, conn))
                        {
                            cmdInsert.Parameters.AddWithValue("NmDescricao", string.IsNullOrEmpty(item.NmDescricao) ? DBNull.Value : item.NmDescricao);
                            cmdInsert.Parameters.AddWithValue("DsUnidade", string.IsNullOrEmpty(item.DsUnidade) ? DBNull.Value : item.DsUnidade);
                            cmdInsert.Parameters.AddWithValue("StAnalise", bool.TryParse(item.StAnalise, out bool stAnalise) ? stAnalise : DBNull.Value);

                            await cmdInsert.ExecuteNonQueryAsync();
                        }
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