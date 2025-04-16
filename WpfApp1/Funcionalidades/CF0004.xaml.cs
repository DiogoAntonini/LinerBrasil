using System.Collections.ObjectModel;
using System.Windows;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0004 : Window
    {
        private readonly string usuarioId;

        public class DadoEntrada
        {
            public string NmCliente { get; set; }
            public string CdMaterialInterno { get; set; }
            public int CdMaterialCliente { get; set; }
            public string NrMetrosLinear { get; set; }
            public int NrLargura { get; set; }
            public string NrMetrosQuadradoBobina { get; set; }
            public string NrMetrosQuadradoPalete { get; set; }
            public int QtBobinasPorPalete { get; set; }
            public int NrColunasPorPalete { get; set; }
            public int NrBobinasPorColuna { get; set; }
            public string StEtiquetaDentroEFora { get; set; }
            public int NrDiaMetroMin { get; set; }
            public int NrDiametroMax { get; set; }
            public string DsEmendaAutomatica { get; set; }
            public string DsEmendaManual { get; set; }
        }

        public CF0004()
        {
            InitializeComponent();

            ObservableCollection<DadoEntrada> listaDeDados = new ObservableCollection<DadoEntrada> { };

            dtgGrade.ItemsSource = listaDeDados;
        }

        private async void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = (ObservableCollection<DadoEntrada>)dtgGrade.ItemsSource;

            if (itemsSource != null)
            {
                try
                {
                    await InserirDadosAsync(itemsSource);
                    CF0003 cadastro = new CF0003(usuarioId);
                    cadastro.Show();
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
                    string comando = "INSERT INTO public.\"T0002_ControleCodigoCliente\" (\"NmCliente\", \"CdMaterialInterno\", \"CdMaterialCliente\", \"NrMetrosLinear\", \"NrLargura\", \"NrMetrosQuadradoBobina\", \"NrMetrosQuadradoPalete\", \"QtBobinasPorPalete\", \"NrColunasPorPalete\", \"NrBobinasPorColuna\", \"StEtiquetaDentroEFora\", \"NrDiaMetroMin\", \"NrDiametroMax\", \"DsEmendaAutomatica\", \"DsEmendaManual\")" +
                                     "VALUES (@NmCliente, @CdMaterialInterno, @CdMaterialCliente, @NrMetrosLinear, @NrLargura, @NrMetrosQuadradoBobina, @NrMetrosQuadradoPalete, @QtBobinasPorPalete, @NrColunasPorPalete, @NrBobinasPorColuna, @StEtiquetaDentroEFora, @NrDiaMetroMin, @NrDiametroMax, @DsEmendaAutomatica, @DsEmendaManual)";

                    using (var cmdInsert = new NpgsqlCommand(comando, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("NmCliente", string.IsNullOrEmpty(item.NmCliente) ? DBNull.Value : item.NmCliente);
                        cmdInsert.Parameters.AddWithValue("CdMaterialInterno", string.IsNullOrEmpty(item.CdMaterialInterno) ? DBNull.Value : item.CdMaterialInterno);
                        cmdInsert.Parameters.AddWithValue("CdMaterialCliente", item.CdMaterialCliente);
                        cmdInsert.Parameters.AddWithValue("NrMetrosLinear", double.TryParse(item.NrMetrosLinear, out double nrMetrosLinear) ? nrMetrosLinear : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("NrLargura", item.NrLargura);
                        cmdInsert.Parameters.AddWithValue("NrMetrosQuadradoBobina", double.TryParse(item.NrMetrosQuadradoBobina, out double nrMetrosQuadradoBobina) ? nrMetrosQuadradoBobina : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("NrMetrosQuadradoPalete", double.TryParse(item.NrMetrosQuadradoPalete, out double nrMetrosQuadradoPalete) ? nrMetrosQuadradoPalete : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("QtBobinasPorPalete", item.QtBobinasPorPalete);
                        cmdInsert.Parameters.AddWithValue("NrColunasPorPalete", item.NrColunasPorPalete);
                        cmdInsert.Parameters.AddWithValue("NrBobinasPorColuna", item.NrBobinasPorColuna);
                        cmdInsert.Parameters.AddWithValue("StEtiquetaDentroEFora", item.StEtiquetaDentroEFora?.ToUpper() == "SIM" ? true : false);
                        cmdInsert.Parameters.AddWithValue("NrDiaMetroMin", item.NrDiaMetroMin);
                        cmdInsert.Parameters.AddWithValue("NrDiametroMax", item.NrDiametroMax);
                        cmdInsert.Parameters.AddWithValue("DsEmendaAutomatica", string.IsNullOrEmpty(item.DsEmendaAutomatica) ? DBNull.Value : item.DsEmendaAutomatica);
                        cmdInsert.Parameters.AddWithValue("DsEmendaManual", string.IsNullOrEmpty(item.DsEmendaManual) ? DBNull.Value : item.DsEmendaManual);

                        await cmdInsert.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0003 cadastro = new CF0003(usuarioId);
            cadastro.Show();
            this.Close();
        }
    }
}