using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0104 : Window
    {
        private string usuarioId;
        private Consultar db;
        private ObservableCollection<DadoEntrada> planejamento;

        public class DadoEntrada
        {
            public string CdOS { get; set; }
            public string PdCliente { get; set; }
            public string NmCliente { get; set; }
            public string DtPedido { get; set; }
            public string DtSolicitacaoEntrega { get; set; }
            public string DtSolicitacaoNovaDataCliente { get; set; }
            public string NrDtPedidoVsDtSolicitacaoEntrega { get; set; }
            public string CdInternoMaterial { get; set; }
            public string CdCliente { get; set; }
            public string NrMetrosLinear { get; set; }
            public string NrMetrosPorPalete { get; set; }
            public string DsCorte { get; set; }
            public string DsSiliconizacao { get; set; }
            public string DtNovaEntregaLiner { get; set; }
            public string NrNFSaida { get; set; }
            public string DsObservacao { get; set; }
            public string QtPL { get; set; }
        }

        public CF0104()
        {
            InitializeComponent();
            db = new Consultar();
            planejamento = new ObservableCollection<DadoEntrada>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string query = "SELECT PLJ.\"CdOS\", PLJ.\"PdCliente\", PLJ.\"NmCliente\", " +
                           "TO_CHAR(PLJ.\"DtPedido\", 'DD/MM/YY') \"DtPedidoFormatada\", " +
                           "TO_CHAR(PLJ.\"DtSolicitacaoEntrega\", 'DD/MM/YY') \"DtSolicitacaoEntregaFormatada\", " +
                           "TO_CHAR(PLJ.\"DtSolicitacaoNovaDataCliente\", 'DD/MM/YY') \"DtSolicitacaoNovaDataClienteFormatada\", " +
                           "PLJ.\"NrDtPedidoVsDtSolicitacaoEntrega\", PLJ.\"CdInternoMaterial\", PLJ.\"CdCliente\", " +
                           "PLJ.\"NrMetrosLinear\", PLJ.\"NrMetrosPorPalete\", PLJ.\"DsCorte\", " +
                           "PLJ.\"DsSiliconizacao\", TO_CHAR(PLJ.\"DtNovaEntregaLiner\", 'DD/MM/YY') \"DtNovaEntregaLinerFormatada\", " +
                           "PLJ.\"NrNFSaida\", PLJ.\"DsObservacao\", PLJ.\"QtPL\" FROM \"T01_Planejamento\" PLJ;";

            DataTable tabela = db.ExecutarConsulta(query);

            if (planejamento == null)
            {
                planejamento = new ObservableCollection<DadoEntrada>();
                dtgGrade.ItemsSource = planejamento;
            }

            planejamento.Clear();

            foreach (DataRow row in tabela.Rows)
            {
                planejamento.Add(new DadoEntrada
                {
                    CdOS = row["CdOS"].ToString(),
                    PdCliente = row["PdCliente"].ToString(),
                    NmCliente = row["NmCliente"].ToString(),
                    DtPedido = row["DtPedidoFormatada"].ToString(),
                    DtSolicitacaoEntrega = row["DtSolicitacaoEntregaFormatada"].ToString(),
                    DtSolicitacaoNovaDataCliente = row["DtSolicitacaoNovaDataClienteFormatada"].ToString(),
                    NrDtPedidoVsDtSolicitacaoEntrega = row["NrDtPedidoVsDtSolicitacaoEntrega"].ToString(),
                    CdInternoMaterial = row["CdInternoMaterial"].ToString(),
                    CdCliente = row["CdCliente"].ToString(),
                    NrMetrosLinear = row["NrMetrosLinear"].ToString(),
                    NrMetrosPorPalete = row["NrMetrosPorPalete"].ToString(),
                    DsCorte = row["DsCorte"].ToString(),
                    DsSiliconizacao = row["DsSiliconizacao"].ToString(),
                    DtNovaEntregaLiner = row["DtNovaEntregaLinerFormatada"].ToString(),
                    NrNFSaida = row["NrNFSaida"].ToString(),
                    DsObservacao = row["DsObservacao"].ToString(),
                    QtPL = row["QtPL"].ToString()
                });
            }
        }


        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is DadoEntrada row)
            {
                try
                {
                    using (var conn = new Npgsql.NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
                    {
                        conn.Open();
                        string comando = "DELETE FROM public.\"T01_Planejamento\" WHERE \"CdOS\" = @CdOS;";
                        using (var cmd = new Npgsql.NpgsqlCommand(comando, conn))
                        {
                            cmd.Parameters.AddWithValue("CdOS", row.CdOS);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    planejamento.Remove(row);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir registro: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0101 planejamento = new CF0101(usuarioId);
            planejamento.Show();
            this.Close();
        }
    }
}