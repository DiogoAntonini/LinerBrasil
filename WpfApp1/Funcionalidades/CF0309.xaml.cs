using System.Collections.ObjectModel;
using System.Windows;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0309 : Window
    {
        private readonly string usuarioId;

        public ObservableCollection<DadoEntrada> Dados { get; set; }

        public class DadoEntrada
        {
            public string HrInicio { get; set; }
            public string HrFim { get; set; }
            public string NrMetragem { get; set; }
            public string NrOS { get; set; }
            public string NmCliente { get; set; }
            public string DtProducao { get; set; }
            public string DsTurno { get; set; }
            public string DsPapel { get; set; }
            public string NrTotalMetragem { get; set; }
        }

        public CF0309()
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            Dados = new ObservableCollection<DadoEntrada>();
            dtgGrade.ItemsSource = Dados;
        }

        private async void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = (ObservableCollection<DadoEntrada>)dtgGrade.ItemsSource;

            if (itemsSource != null)
            {
                try
                {
                    await InserirDadosAsync(itemsSource);
                    CF0308 extrusao = new CF0308();
                    extrusao.Show();
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
                    string comando = "INSERT INTO public.\"T10_Extrusao\" (\"HrInicio\",\"HrFim\",\"NrMetragem\",\"NrOS\",\"NmCliente\",\"DtProducao\",\"DsTurno\",\"DsPapel\",\"NrTotalMetragem\")" +
                                     "VALUES (@HrInicio, @HrFim, @NrMetragem, @NrOS, @NmCliente, @DtProducao, @DsTurno, @DsPapel, @NrTotalMetragem)";

                    using (var cmdInsert = new NpgsqlCommand(comando, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("HrInicio", DateTime.TryParse(item.HrInicio, out DateTime hrInicio) ? hrInicio : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("HrFim", DateTime.TryParse(item.HrFim, out DateTime hrFim) ? hrFim : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("NrMetragem", string.IsNullOrEmpty(item.NrMetragem) ? DBNull.Value : item.NrMetragem);
                        cmdInsert.Parameters.AddWithValue("NrOS", string.IsNullOrEmpty(item.NrOS) ? DBNull.Value : item.NrOS);
                        cmdInsert.Parameters.AddWithValue("NmCliente", string.IsNullOrEmpty(item.NmCliente) ? DBNull.Value : item.NmCliente);
                        cmdInsert.Parameters.AddWithValue("DtProducao", DateTime.TryParse(item.DtProducao, out DateTime dtProducao) ? dtProducao : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("DsTurno", string.IsNullOrEmpty(item.DsTurno) ? DBNull.Value : item.DsTurno);
                        cmdInsert.Parameters.AddWithValue("DsPapel", string.IsNullOrEmpty(item.DsPapel) ? DBNull.Value : item.DsPapel);
                        cmdInsert.Parameters.AddWithValue("NrTotalMetragem", string.IsNullOrEmpty(item.NrTotalMetragem) ? DBNull.Value : item.NrTotalMetragem);

                        await cmdInsert.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0308 extrusao = new CF0308();
            extrusao.Show();
            Close();
        }

    }
}