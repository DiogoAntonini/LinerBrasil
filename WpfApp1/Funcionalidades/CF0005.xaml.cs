using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0005 : Window
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

            public string NmClienteOriginal { get; set; }
            public string CdMaterialInternoOriginal { get; set; }
            public string CdMaterialClienteOriginal { get; set; }
            public string NrMetrosLinearOriginal { get; set; }
            public string NrLarguraOriginal { get; set; }
            public string NrMetrosQuadradoBobinaOriginal { get; set; }
            public string NrMetrosQuadradoPaleteOriginal { get; set; }
            public string QtBobinasPorPaleteOriginal { get; set; }
            public string NrColunasPorPaleteOriginal { get; set; }
            public string NrBobinasPorColunaOriginal { get; set; }
            public string StEtiquetaDentroEForaOriginal { get; set; }
            public string NrDiaMetroMinOriginal { get; set; }
            public string NrDiametroMaxOriginal { get; set; }
            public string DsEmendaAutomaticaOriginal { get; set; }
            public string DsEmendaManualOriginal { get; set; }
        }

        public CF0005()
        {
            InitializeComponent();
            db = new Consultar();
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
                        NmClienteOriginal = row["NmCliente"].ToString(),
                        CdMaterialInterno = row["CdMaterialInterno"].ToString(),
                        CdMaterialInternoOriginal = row["CdMaterialInterno"].ToString(),
                        CdMaterialCliente = row["CdMaterialCliente"].ToString(),
                        CdMaterialClienteOriginal = row["CdMaterialCliente"].ToString(),
                        NrMetrosLinear = row["NrMetrosLinear"].ToString(),
                        NrMetrosLinearOriginal = row["NrMetrosLinear"].ToString(),
                        NrLargura = row["NrLargura"].ToString(),
                        NrLarguraOriginal = row["NrLargura"].ToString(),
                        NrMetrosQuadradoBobina = row["NrMetrosQuadradoBobina"].ToString(),
                        NrMetrosQuadradoBobinaOriginal = row["NrMetrosQuadradoBobina"].ToString(),
                        NrMetrosQuadradoPalete = row["NrMetrosQuadradoPalete"].ToString(),
                        NrMetrosQuadradoPaleteOriginal = row["NrMetrosQuadradoPalete"].ToString(),
                        QtBobinasPorPalete = row["QtBobinasPorPalete"].ToString(),
                        QtBobinasPorPaleteOriginal = row["QtBobinasPorPalete"].ToString(),
                        NrColunasPorPalete = row["NrColunasPorPalete"].ToString(),
                        NrColunasPorPaleteOriginal = row["NrColunasPorPalete"].ToString(),
                        NrBobinasPorColuna = row["NrBobinasPorColuna"].ToString(),
                        NrBobinasPorColunaOriginal = row["NrBobinasPorColuna"].ToString(),
                        StEtiquetaDentroEFora = row["StEtiquetaDentroEFora"].ToString(),
                        StEtiquetaDentroEForaOriginal = row["StEtiquetaDentroEFora"].ToString(),
                        NrDiaMetroMin = row["NrDiaMetroMin"].ToString(),
                        NrDiaMetroMinOriginal = row["NrDiaMetroMin"].ToString(),
                        NrDiametroMax = row["NrDiametroMax"].ToString(),
                        NrDiametroMaxOriginal = row["NrDiametroMax"].ToString(),
                        DsEmendaAutomatica = row["DsEmendaAutomatica"].ToString(),
                        DsEmendaAutomaticaOriginal = row["DsEmendaAutomatica"].ToString(),
                        DsEmendaManual = row["DsEmendaManual"].ToString(),
                        DsEmendaManualOriginal = row["DsEmendaManual"].ToString()
                    });
                }

                dtgGrade.ItemsSource = new ObservableCollection<DadoEntrada>(codigoCliente);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = (ObservableCollection<DadoEntrada>)dtgGrade.ItemsSource;

            if (itemsSource != null)
            {
                try
                {
                    await AtualizarDadosAsync(itemsSource);
                    CF0003 codigoCliente = new CF0003(usuarioId);
                    codigoCliente.Show();
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

        private async Task AtualizarDadosAsync(ObservableCollection<DadoEntrada> itemsSource)
        {
            using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
            {
                await conn.OpenAsync();

                foreach (var item in itemsSource)
                {
                    if (item.NmCliente != item.NmClienteOriginal ||
                        item.CdMaterialInterno != item.CdMaterialInternoOriginal ||
                        item.CdMaterialCliente != item.CdMaterialClienteOriginal ||
                        item.NrMetrosLinear != item.NrMetrosLinearOriginal ||
                        item.NrLargura != item.NrLarguraOriginal ||
                        item.NrMetrosQuadradoBobina != item.NrMetrosQuadradoBobinaOriginal ||
                        item.NrMetrosQuadradoPalete != item.NrMetrosQuadradoPaleteOriginal ||
                        item.QtBobinasPorPalete != item.QtBobinasPorPaleteOriginal ||
                        item.NrColunasPorPalete != item.NrColunasPorPaleteOriginal ||
                        item.NrBobinasPorColuna != item.NrBobinasPorColunaOriginal ||
                        item.StEtiquetaDentroEFora != item.StEtiquetaDentroEForaOriginal ||
                        item.NrDiaMetroMin != item.NrDiaMetroMinOriginal ||
                        item.NrDiametroMax != item.NrDiametroMaxOriginal ||
                        item.DsEmendaAutomatica != item.DsEmendaAutomaticaOriginal ||
                        item.DsEmendaManual != item.DsEmendaManualOriginal)
                    {
                        string comando = "UPDATE public.\"T0002_ControleCodigoCliente\" SET " +
                                         "\"NmCliente\" = @NmCliente, " +
                                         "\"CdMaterialInterno\" = @CdMaterialInterno, " +
                                         "\"CdMaterialCliente\" = @CdMaterialCliente, " +
                                         "\"NrMetrosLinear\" = @NrMetrosLinear, " +
                                         "\"NrLargura\" = @NrLargura, " +
                                         "\"NrMetrosQuadradoBobina\" = @NrMetrosQuadradoBobina, " +
                                         "\"NrMetrosQuadradoPalete\" = @NrMetrosQuadradoPalete, " +
                                         "\"QtBobinasPorPalete\" = @QtBobinasPorPalete, " +
                                         "\"NrColunasPorPalete\" = @NrColunasPorPalete, " +
                                         "\"NrBobinasPorColuna\" = @NrBobinasPorColuna, " +
                                         "\"StEtiquetaDentroEFora\" = @StEtiquetaDentroEFora, " +
                                         "\"NrDiaMetroMin\" = @NrDiaMetroMin, " +
                                         "\"NrDiametroMax\" = @NrDiametroMax, " +
                                         "\"DsEmendaAutomatica\" = @DsEmendaAutomatica, " +
                                         "\"DsEmendaManual\" = @DsEmendaManual " +
                                         "WHERE \"CdMaterialInterno\" = @CdMaterialInterno;";

                        using (var cmd = new NpgsqlCommand(comando, conn))
                        {
                            cmd.Parameters.AddWithValue("NmCliente", string.IsNullOrEmpty(item.NmCliente) ? DBNull.Value : item.NmCliente);
                            cmd.Parameters.AddWithValue("CdMaterialInterno", string.IsNullOrEmpty(item.CdMaterialInterno) ? DBNull.Value : item.CdMaterialInterno);
                            cmd.Parameters.AddWithValue("CdMaterialCliente", int.TryParse(item.CdMaterialCliente, out int cdMaterialCliente) ? cdMaterialCliente : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("NrMetrosLinear", double.TryParse(item.NrMetrosLinear, out double nrMetrosLinear) ? nrMetrosLinear : DBNull.Value);
                            cmd.Parameters.AddWithValue("NrLargura", int.TryParse(item.NrLargura, out int nrLargura) ? nrLargura : DBNull.Value);
                            cmd.Parameters.AddWithValue("NrMetrosQuadradoBobina", double.TryParse(item.NrMetrosQuadradoBobina, out double nrMetrosQuadradoBobina) ? nrMetrosQuadradoBobina : DBNull.Value);
                            cmd.Parameters.AddWithValue("NrMetrosQuadradoPalete", double.TryParse(item.NrMetrosQuadradoPalete, out double nrMetrosQuadradoPalete) ? nrMetrosQuadradoPalete : DBNull.Value);
                            cmd.Parameters.AddWithValue("QtBobinasPorPalete", int.TryParse(item.QtBobinasPorPalete, out int qtBobinasPorPalete) ? qtBobinasPorPalete : DBNull.Value);
                            cmd.Parameters.AddWithValue("NrColunasPorPalete", int.TryParse(item.NrColunasPorPalete, out int nrColunasPorPalete) ? nrColunasPorPalete : DBNull.Value);
                            cmd.Parameters.AddWithValue("NrBobinasPorColuna", int.TryParse(item.NrBobinasPorColuna, out int nrBobinasPorColuna) ? nrBobinasPorColuna : DBNull.Value);
                            cmd.Parameters.AddWithValue("StEtiquetaDentroEFora", bool.TryParse(item.StEtiquetaDentroEFora, out bool stEtiquetaDentroEFora) ? stEtiquetaDentroEFora : DBNull.Value);
                            cmd.Parameters.AddWithValue("NrDiaMetroMin", int.TryParse(item.NrDiaMetroMin, out int nrDiaMetroMin) ? nrDiaMetroMin : DBNull.Value);
                            cmd.Parameters.AddWithValue("NrDiametroMax", int.TryParse(item.NrDiametroMax, out int nrDiametroMax) ? nrDiametroMax : DBNull.Value);
                            cmd.Parameters.AddWithValue("DsEmendaAutomatica", string.IsNullOrEmpty(item.DsEmendaAutomatica) ? DBNull.Value : item.DsEmendaAutomatica);
                            cmd.Parameters.AddWithValue("DsEmendaManual", string.IsNullOrEmpty(item.DsEmendaManual) ? DBNull.Value : item.DsEmendaManual);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0003 codigoCliente = new CF0003(usuarioId);
            codigoCliente.Show();
            this.Close();
        }
    }
}