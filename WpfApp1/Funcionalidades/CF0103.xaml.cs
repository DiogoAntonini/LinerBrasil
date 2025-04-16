using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0103 : Window
    {
        private string usuarioId;

        private Consultar db;

        public class CaracteristicaModel
        {
            public string CdOS { get; set; }
            public string CdOSOriginal { get; set; }

            public string PdCliente { get; set; }
            public string PdClienteOriginal { get; set; }

            public string NmCliente { get; set; }
            public string NmClienteOriginal { get; set; }

            public string DtPedido { get; set; }
            public string DtPedidoOriginal { get; set; }

            public string DtSolicitacaoEntrega { get; set; }
            public string DtSolicitacaoEntregaOriginal { get; set; }

            public string DtSolicitacaoNovaDataCliente { get; set; }
            public string DtSolicitacaoNovaDataClienteOriginal { get; set; }

            public string NrDtPedidoVsDtSolicitacaoEntrega { get; set; }
            public string NrDtPedidoVsDtSolicitacaoEntregaOriginal { get; set; }

            public string CdInternoMaterial { get; set; }
            public string CdInternoMaterialOriginal { get; set; }

            public string CdCliente { get; set; }
            public string CdClienteOriginal { get; set; }

            public string NrMetrosLinear { get; set; }
            public string NrMetrosLinearOriginal { get; set; }

            public string NrMetrosPorPalete { get; set; }
            public string NrMetrosPorPaleteOriginal { get; set; }

            public string DsCorte { get; set; }
            public string DsCorteOriginal { get; set; }

            public string DsSiliconizacao { get; set; }
            public string DsSiliconizacaoOriginal { get; set; }

            public string DtNovaEntregaLiner { get; set; }
            public string DtNovaEntregaLinerOriginal { get; set; }

            public string NrNFSaida { get; set; }
            public string NrNFSaidaOriginal { get; set; }

            public string DsObservacao { get; set; }
            public string DsObservacaoOriginal { get; set; }

            public string QtPL { get; set; }
            public string QtPLOriginal { get; set; }
        }

        public CF0103(string usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            db = new Consultar();

            string query = "SELECT PLJ.\"CdOS\", PLJ.\"PdCliente\", PLJ.\"NmCliente\", TO_CHAR(PLJ.\"DtPedido\", 'DD/MM/YY') \"DtPedido\", TO_CHAR(PLJ.\"DtSolicitacaoEntrega\", 'DD/MM/YY') \"DtSolicitacaoEntrega\", TO_CHAR(PLJ.\"DtSolicitacaoNovaDataCliente\", 'DD/MM/YY') \"DtSolicitacaoNovaDataCliente\", PLJ.\"NrDtPedidoVsDtSolicitacaoEntrega\", PLJ.\"CdInternoMaterial\", PLJ.\"CdCliente\", PLJ.\"NrMetrosLinear\", PLJ.\"NrMetrosPorPalete\", PLJ.\"DsCorte\", PLJ.\"DsSiliconizacao\",TO_CHAR(PLJ.\"DtNovaEntregaLiner\", 'DD/MM/YY') \"DtNovaEntregaLiner\", PLJ.\"NrNFSaida\", PLJ.\"DsObservacao\", PLJ.\"QtPL\" FROM \"T01_Planejamento\" PLJ;";

            DataTable tabela = db.ExecutarConsulta(query);

            List<CaracteristicaModel> caracteristicas = new List<CaracteristicaModel>();

            foreach (DataRow row in tabela.Rows)
            {
                caracteristicas.Add(new CaracteristicaModel
                {
                    CdOS = row["CdOS"].ToString(),
                    CdOSOriginal = row["CdOS"].ToString(),

                    PdCliente = row["PdCliente"].ToString(),
                    PdClienteOriginal = row["PdCliente"].ToString(),

                    NmCliente = row["NmCliente"].ToString(),
                    NmClienteOriginal = row["NmCliente"].ToString(),

                    DtPedido = row["DtPedido"].ToString(),
                    DtPedidoOriginal = row["DtPedido"].ToString(),

                    DtSolicitacaoEntrega = row["DtSolicitacaoEntrega"].ToString(),
                    DtSolicitacaoEntregaOriginal = row["DtSolicitacaoEntrega"].ToString(),

                    DtSolicitacaoNovaDataCliente = row["DtSolicitacaoNovaDataCliente"].ToString(),
                    DtSolicitacaoNovaDataClienteOriginal = row["DtSolicitacaoNovaDataCliente"].ToString(),

                    NrDtPedidoVsDtSolicitacaoEntrega = row["NrDtPedidoVsDtSolicitacaoEntrega"].ToString(),
                    NrDtPedidoVsDtSolicitacaoEntregaOriginal = row["NrDtPedidoVsDtSolicitacaoEntrega"].ToString(),

                    CdInternoMaterial = row["CdInternoMaterial"].ToString(),
                    CdInternoMaterialOriginal = row["CdInternoMaterial"].ToString(),

                    CdCliente = row["CdCliente"].ToString(),
                    CdClienteOriginal = row["CdCliente"].ToString(),

                    NrMetrosLinear = row["NrMetrosLinear"].ToString(),
                    NrMetrosLinearOriginal = row["NrMetrosLinear"].ToString(),

                    NrMetrosPorPalete = row["NrMetrosPorPalete"].ToString(),
                    NrMetrosPorPaleteOriginal = row["NrMetrosPorPalete"].ToString(),

                    DsCorte = row["DsCorte"].ToString(),
                    DsCorteOriginal = row["DsCorte"].ToString(),

                    DsSiliconizacao = row["DsSiliconizacao"].ToString(),
                    DsSiliconizacaoOriginal = row["DsSiliconizacao"].ToString(),

                    DtNovaEntregaLiner = row["DtNovaEntregaLiner"].ToString(),
                    DtNovaEntregaLinerOriginal = row["DtNovaEntregaLiner"].ToString(),

                    NrNFSaida = row["NrNFSaida"].ToString(),
                    NrNFSaidaOriginal = row["NrNFSaida"].ToString(),

                    DsObservacao = row["DsObservacao"].ToString(),
                    DsObservacaoOriginal = row["DsObservacao"].ToString(),

                    QtPL = row["QtPL"].ToString(),
                    QtPLOriginal = row["QtPL"].ToString()
                });
            }

            dtgGrade.ItemsSource = new ObservableCollection<CaracteristicaModel>(caracteristicas);
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = (ObservableCollection<CaracteristicaModel>)dtgGrade.ItemsSource;

            if (itemsSource != null)
            {
                try
                {
                    using (var conn = new Npgsql.NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
                    {
                        conn.Open();

                        foreach (var item in itemsSource)
                        {
                            if (
                                item.CdOS != item.CdOSOriginal ||
                                item.PdCliente != item.PdClienteOriginal ||
                                item.NmCliente != item.NmClienteOriginal ||
                                item.DtPedido != item.DtPedidoOriginal ||
                                item.DtSolicitacaoEntrega != item.DtSolicitacaoEntregaOriginal ||
                                item.DtSolicitacaoNovaDataCliente != item.DtSolicitacaoNovaDataClienteOriginal ||
                                item.NrDtPedidoVsDtSolicitacaoEntrega != item.NrDtPedidoVsDtSolicitacaoEntregaOriginal ||
                                item.CdInternoMaterial != item.CdInternoMaterialOriginal ||
                                item.CdCliente != item.CdClienteOriginal ||
                                item.NrMetrosLinear != item.NrMetrosLinearOriginal ||
                                item.NrMetrosPorPalete != item.NrMetrosPorPaleteOriginal ||
                                item.DsCorte != item.DsCorteOriginal ||
                                item.DsSiliconizacao != item.DsSiliconizacaoOriginal ||
                                item.DtNovaEntregaLiner != item.DtNovaEntregaLinerOriginal ||
                                item.NrNFSaida != item.NrNFSaidaOriginal ||
                                item.DsObservacao != item.DsObservacaoOriginal ||
                                item.QtPL != item.QtPLOriginal)
                            {
                                string comando = "UPDATE public.\"T01_Planejamento\" SET " +
                                                 "\"CdOS\" = @CdOS, " +
                                                 "\"PdCliente\" = @PdCliente, " +
                                                 "\"NmCliente\" = @NmCliente, " +
                                                 "\"DtPedido\" = @DtPedido, " +
                                                 "\"DtSolicitacaoEntrega\" = @DtSolicitacaoEntrega, " +
                                                 "\"DtSolicitacaoNovaDataCliente\" = @DtSolicitacaoNovaDataCliente, " +
                                                 "\"NrDtPedidoVsDtSolicitacaoEntrega\" = @NrDtPedidoVsDtSolicitacaoEntrega, " +
                                                 "\"CdInternoMaterial\" = @CdInternoMaterial, " +
                                                 "\"CdCliente\" = @CdCliente, " +
                                                 "\"NrMetrosLinear\" = @NrMetrosLinear, " +
                                                 "\"NrMetrosPorPalete\" = @NrMetrosPorPalete, " +
                                                 "\"DsCorte\" = @DsCorte, " +
                                                 "\"DsSiliconizacao\" = @DsSiliconizacao, " +
                                                 "\"DtNovaEntregaLiner\" = @DtNovaEntregaLiner, " +
                                                 "\"NrNFSaida\" = @NrNFSaida, " +
                                                 "\"DsObservacao\" = @DsObservacao, " +
                                                 "\"QtPL\" = @QtPL " +
                                                 "WHERE \"CdOS\" = @CdOS;";

                                using (var cmd = new Npgsql.NpgsqlCommand(comando, conn))
                                {
                                    cmd.Parameters.AddWithValue("CdOS", string.IsNullOrEmpty(item.CdOS) ? DBNull.Value : item.CdOS);
                                    cmd.Parameters.AddWithValue("PdCliente", string.IsNullOrEmpty(item.PdCliente) ? DBNull.Value : item.PdCliente);
                                    cmd.Parameters.AddWithValue("NmCliente", string.IsNullOrEmpty(item.NmCliente) ? DBNull.Value : item.NmCliente);
                                    cmd.Parameters.AddWithValue("DtPedido", DateTime.TryParse(item.DtPedido, out DateTime dtPedido) ? dtPedido : DBNull.Value);
                                    cmd.Parameters.AddWithValue("DtSolicitacaoEntrega", DateTime.TryParse(item.DtSolicitacaoEntrega, out DateTime dtSolicitacaoEntrega) ? dtSolicitacaoEntrega : DBNull.Value);
                                    cmd.Parameters.AddWithValue("DtSolicitacaoNovaDataCliente", DateTime.TryParse(item.DtSolicitacaoNovaDataCliente, out DateTime dtSolicitacaoNovaDataCliente) ? dtSolicitacaoNovaDataCliente : DBNull.Value);
                                    cmd.Parameters.AddWithValue("NrDtPedidoVsDtSolicitacaoEntrega", int.TryParse(item.NrDtPedidoVsDtSolicitacaoEntrega, out int nrDtPedidoVsDtSolicitacaoEntrega) ? nrDtPedidoVsDtSolicitacaoEntrega : DBNull.Value);
                                    cmd.Parameters.AddWithValue("CdInternoMaterial", string.IsNullOrEmpty(item.CdInternoMaterial) ? DBNull.Value : item.CdInternoMaterial);
                                    cmd.Parameters.AddWithValue("CdCliente", string.IsNullOrEmpty(item.CdCliente) ? DBNull.Value : item.CdCliente);
                                    cmd.Parameters.AddWithValue("NrMetrosLinear", double.TryParse(item.NrMetrosLinear, out double nrMetrosLinear) ? nrMetrosLinear : DBNull.Value);
                                    cmd.Parameters.AddWithValue("NrMetrosPorPalete", double.TryParse(item.NrMetrosPorPalete, out double nrMetrosPorPalete) ? nrMetrosPorPalete : DBNull.Value);
                                    cmd.Parameters.AddWithValue("DsCorte", string.IsNullOrEmpty(item.DsCorte) ? DBNull.Value : item.DsCorte);
                                    cmd.Parameters.AddWithValue("DsSiliconizacao", string.IsNullOrEmpty(item.DsSiliconizacao) ? DBNull.Value : item.DsSiliconizacao);
                                    cmd.Parameters.AddWithValue("DtNovaEntregaLiner", DateTime.TryParse(item.DtNovaEntregaLiner, out DateTime dtNovaEntregaLiner) ? dtNovaEntregaLiner : DBNull.Value);
                                    cmd.Parameters.AddWithValue("NrNFSaida", int.TryParse(item.NrNFSaida, out int nrNFSaida) ? nrNFSaida : DBNull.Value);
                                    cmd.Parameters.AddWithValue("DsObservacao", string.IsNullOrEmpty(item.DsObservacao) ? DBNull.Value : item.DsObservacao);
                                    cmd.Parameters.AddWithValue("QtPL", int.TryParse(item.QtPL, out int qtPL) ? qtPL : DBNull.Value);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    CF0101 planejamento = new CF0101(usuarioId);
                    planejamento.Show();
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

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            new CF0101(usuarioId).Show();
            this.Close();
        }
    }
}