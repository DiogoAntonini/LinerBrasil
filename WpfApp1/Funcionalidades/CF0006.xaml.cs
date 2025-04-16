using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0006 : Window
    {
        private readonly string usuarioId;
        private readonly Consultar db;

        public class DadoEntrada
        {
            public string NmCliente { get; set; }
            public string CdMaterialInterno { get; set; }
            public string CdMaterialCliente { get; set; }
            public string NrMetrosLinear { get; set; }
            public string NrLargura { get; set; }
            public string NrMetrosQuadradoBobina { get; set; }
            public string NrMetrosQuadradoPalete { get; set; }
            public string QtBobinasPorPalete { get; set; }
            public string NrColunasPorPalete { get; set; }
            public string NrBobinasPorColuna { get; set; }
            public string StEtiquetaDentroEFora { get; set; }
            public string NrDiaMetroMin { get; set; }
            public string NrDiametroMax { get; set; }
            public string DsEmendaAutomatica { get; set; }
            public string DsEmendaManual { get; set; }
        }

        public CF0006(string usuarioId)
        {
            InitializeComponent();
            db = new Consultar();
            this.usuarioId = usuarioId;
            CarregarDados();
        }

        private async void CarregarDados()
        {
            string query = "SELECT CCC.\"NmCliente\", CCC.\"CdMaterialInterno\" , CCC.\"CdMaterialCliente\" , CCC.\"NrMetrosLinear\" , CCC.\"NrLargura\" , CCC.\"NrMetrosQuadradoBobina\" , CCC.\"NrMetrosQuadradoPalete\" , CCC.\"QtBobinasPorPalete\" , CCC.\"NrColunasPorPalete\" , CCC.\"NrBobinasPorColuna\" , CCC.\"StEtiquetaDentroEFora\" , CCC.\"NrDiaMetroMin\" , CCC.\"NrDiametroMax\" , CCC.\"DsEmendaAutomatica\" , CCC.\"DsEmendaManual\" FROM \"T0002_ControleCodigoCliente\" CCC;";

            try
            {
                DataTable tabela = await Task.Run(() => db.ExecutarConsulta(query));
                List<DadoEntrada> codigoCliente = new List<DadoEntrada>();

                foreach (DataRow row in tabela.Rows)
                {
                    codigoCliente.Add(new DadoEntrada
                    {
                        NmCliente = row["NmCliente"].ToString(),
                        CdMaterialInterno = row["CdMaterialInterno"].ToString(),
                        CdMaterialCliente = row["CdMaterialCliente"].ToString(),
                        NrMetrosLinear = row["NrMetrosLinear"].ToString(),
                        NrLargura = row["NrLargura"].ToString(),
                        NrMetrosQuadradoBobina = row["NrMetrosQuadradoBobina"].ToString(),
                        NrMetrosQuadradoPalete = row["NrMetrosQuadradoPalete"].ToString(),
                        QtBobinasPorPalete = row["QtBobinasPorPalete"].ToString(),
                        NrColunasPorPalete = row["NrColunasPorPalete"].ToString(),
                        NrBobinasPorColuna = row["NrBobinasPorColuna"].ToString(),
                        StEtiquetaDentroEFora = row["StEtiquetaDentroEFora"].ToString(),
                        NrDiaMetroMin = row["NrDiaMetroMin"].ToString(),
                        NrDiametroMax = row["NrDiametroMax"].ToString(),
                        DsEmendaAutomatica = row["DsEmendaAutomatica"].ToString(),
                        DsEmendaManual = row["DsEmendaManual"].ToString()
                    });
                }

                dtgGrade.ItemsSource = new ObservableCollection<DadoEntrada>(codigoCliente);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var row = button.DataContext as DadoEntrada;
                if (row != null)
                {
                    try
                    {
                        await ExcluirRegistroAsync(row);
                        var itemsSource = dtgGrade.ItemsSource as ObservableCollection<DadoEntrada>;
                        if (itemsSource != null)
                        {
                            itemsSource.Remove(row);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao excluir registro: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async Task ExcluirRegistroAsync(DadoEntrada row)
        {
            using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
            {
                await conn.OpenAsync();

                string comando = @"DELETE FROM public.""T0002_ControleCodigoCliente"" WHERE ""CdMaterialInterno"" = @CdMaterialInterno;";
                using (var cmd = new NpgsqlCommand(comando, conn))
                {
                    cmd.Parameters.AddWithValue("CdMaterialInterno", row.CdMaterialInterno);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0003 planejamento = new CF0003(usuarioId);
            planejamento.Show();
            this.Close();
        }
    }
}