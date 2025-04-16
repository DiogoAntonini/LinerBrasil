using System.Collections.ObjectModel;
using System.Windows;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0102 : Window
    {
        private string usuarioId;

        public class DadoEntrada
        {
            public string CdCliente { get; set; }
            public string CdInternoMaterial { get; set; }
            public string CdOS { get; set; }
            public string DsCorte { get; set; }
            public string DsObservacao { get; set; }
            public string DsSiliconizacao { get; set; }
            public string DtNovaEntregaLiner { get; set; }
            public string DtPedido { get; set; }
            public string DtSolicitacaoEntrega { get; set; }
            public string DtSolicitacaoNovaDataCliente { get; set; }
            public string NmCliente { get; set; }
            public string NrDtPedidoVsDtSolicitacaoEntrega { get; set; }
            public string NrMetrosLinear { get; set; }
            public string NrMetrosPorPalete { get; set; }
            public string NrNFSaida { get; set; }
            public string PdCliente { get; set; }
            public string QtPL { get; set; }
        }

        public CF0102(string usuarioId)
        {
            InitializeComponent();

            ObservableCollection<DadoEntrada> listaDeDados = new ObservableCollection<DadoEntrada> { };

            dtgGrade.ItemsSource = listaDeDados;

            this.usuarioId = usuarioId;
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = (ObservableCollection<DadoEntrada>)dtgGrade.ItemsSource;

            string comando = "INSERT INTO public.\"T01_Planejamento\" (\"CdOS\",\"PdCliente\",\"NmCliente\",\"DtPedido\",\"DtSolicitacaoEntrega\",\"DtSolicitacaoNovaDataCliente\",\"NrDtPedidoVsDtSolicitacaoEntrega\",\"CdInternoMaterial\",\"CdCliente\",\"NrMetrosLinear\",\"NrMetrosPorPalete\",\"DsCorte\",\"DsSiliconizacao\",\"DtNovaEntregaLiner\",\"NrNFSaida\",\"DsObservacao\",\"QtPL\")" +
                             "VALUES (@CdOS, @PdCliente, @NmCliente, @DtPedido, @DtSolicitacaoEntrega, @DtSolicitacaoNovaDataCliente, @NrDtPedidoVsDtSolicitacaoEntrega, @CdInternoMaterial, @CdCliente, @NrMetrosLinear, @NrMetrosPorPalete, @DsCorte, @DsSiliconizacao, @DtNovaEntregaLiner, @NrNFSaida, @DsObservacao, @QtPL)";

            using (var conn = new Npgsql.NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
            {
                conn.Open();

                foreach (var item in itemsSource)
                {
                    using (var cmdInsert = new NpgsqlCommand(comando, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("CdOS", string.IsNullOrEmpty(item.CdOS) ? DBNull.Value : item.CdOS);
                        cmdInsert.Parameters.AddWithValue("PdCliente", string.IsNullOrEmpty(item.PdCliente) ? DBNull.Value : item.PdCliente);
                        cmdInsert.Parameters.AddWithValue("NmCliente", string.IsNullOrEmpty(item.NmCliente) ? DBNull.Value : item.NmCliente);
                        cmdInsert.Parameters.AddWithValue("DtPedido", DateTime.TryParse(item.DtPedido, out DateTime dtPedido) ? dtPedido : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("DtSolicitacaoEntrega", DateTime.TryParse(item.DtSolicitacaoEntrega, out DateTime dtSolicitacaoEntrega) ? dtSolicitacaoEntrega : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("DtSolicitacaoNovaDataCliente", DateTime.TryParse(item.DtSolicitacaoNovaDataCliente, out DateTime dtSolicitacaoNovaDataCliente) ? dtSolicitacaoNovaDataCliente : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("NrDtPedidoVsDtSolicitacaoEntrega", int.TryParse(item.NrDtPedidoVsDtSolicitacaoEntrega, out int nrDtPedidoVsDtSolicitacaoEntrega) ? nrDtPedidoVsDtSolicitacaoEntrega : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("CdInternoMaterial", string.IsNullOrEmpty(item.CdInternoMaterial) ? DBNull.Value : item.CdInternoMaterial);
                        cmdInsert.Parameters.AddWithValue("CdCliente", string.IsNullOrEmpty(item.CdCliente) ? DBNull.Value : item.CdCliente);
                        cmdInsert.Parameters.AddWithValue("NrMetrosLinear", double.TryParse(item.NrMetrosLinear, out double nrMetrosLinear) ? nrMetrosLinear : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("NrMetrosPorPalete", double.TryParse(item.NrMetrosPorPalete, out double nrMetrosPorPalete) ? nrMetrosPorPalete : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("DsCorte", string.IsNullOrEmpty(item.DsCorte) ? DBNull.Value : item.DsCorte);
                        cmdInsert.Parameters.AddWithValue("DsSiliconizacao", string.IsNullOrEmpty(item.DsSiliconizacao) ? DBNull.Value : item.DsSiliconizacao);
                        cmdInsert.Parameters.AddWithValue("DtNovaEntregaLiner", DateTime.TryParse(item.DtNovaEntregaLiner, out DateTime dtNovaEntregaLiner) ? dtNovaEntregaLiner : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("NrNFSaida", int.TryParse(item.NrNFSaida, out int nrNFSaida) ? nrNFSaida : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("DsObservacao", string.IsNullOrEmpty(item.DsObservacao) ? DBNull.Value : item.DsObservacao);
                        cmdInsert.Parameters.AddWithValue("QtPL", int.TryParse(item.QtPL, out int qtPL) ? qtPL : DBNull.Value);

                        try
                        {
                            cmdInsert.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao executar a consulta: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }

            new CF0101(usuarioId).Show();
            this.Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            new CF0101(usuarioId).Show();
            this.Close();
        }
    }
}