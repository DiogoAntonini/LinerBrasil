using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0303 : Window
    {
        private Consultar db;

        public class DadoEntrada
        {
            public int IdRecebimento { get; set; }
            public string DsMaterial { get; set; }
            public string NrGramatura { get; set; }
            public string NrLargura { get; set; }
            public string StAlvejante { get; set; }
            public string NrTracaoSeca { get; set; }
            public string NrTracaoUmida { get; set; }
            public string NrPorosidadeGurley { get; set; }
            public string NrAbsorcaoCoob { get; set; }
            public string DsStatusAmostra { get; set; }
            public string NmResponsavel { get; set; }
            public string DsDesvio { get; set; }
        }

        public class Caracteristica
        {
            public string NrMinimo { get; set; }
            public string DsTarget { get; set; }
            public string NrMaximo { get; set; }
            public string DsCaracteristica { get; set; }
        }

        public CF0303()
        {
            InitializeComponent();
            db = new Consultar();

            string query = "SELECT RCB.\"IdRecebimento\", RCB.\"DsMaterial\", AMT.\"NrGramatura\", AMT.\"NrLargura\", AMT.\"StAlvejante\", AMT.\"NrTracaoSeca\", AMT.\"NrTracaoUmida\", AMT.\"NrPorosidadeGurley\", AMT.\"NrAbsorcaoCoob\", AMT.\"DsStatusAmostra\", AMT.\"NmResponsavel\", AMT.\"DsDesvio\" FROM \"T02_Recebimento\" RCB LEFT JOIN \"T0305_ResultadoAmostra\" AMT ON AMT.\"IdRecebimento\" = RCB.\"IdRecebimento\" WHERE RCB.\"DsStatus\" = 'Laboratório';";

            DataTable tabela = db.ExecutarConsulta(query);

            ObservableCollection<DadoEntrada> materiaprima = new ObservableCollection<DadoEntrada>();

            foreach (DataRow row in tabela.Rows)
            {
                materiaprima.Add(new DadoEntrada
                {
                    IdRecebimento = Convert.ToInt32(row["IdRecebimento"].ToString()),
                    DsMaterial = row["DsMaterial"].ToString(),
                    NrGramatura = row["NrGramatura"].ToString(),
                    NrLargura = row["NrLargura"].ToString(),
                    StAlvejante = row["StAlvejante"].ToString(),
                    NrTracaoSeca = row["NrTracaoSeca"].ToString(),
                    NrTracaoUmida = row["NrTracaoUmida"].ToString(),
                    NrPorosidadeGurley = row["NrPorosidadeGurley"].ToString(),
                    NrAbsorcaoCoob = row["NrAbsorcaoCoob"].ToString(),
                    DsStatusAmostra = row["DsStatusAmostra"].ToString(),
                    NmResponsavel = row["NmResponsavel"].ToString(),
                    DsDesvio = row["DsDesvio"].ToString()
                });
            }

            dtgGrade.ItemsSource = materiaprima;
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = (ObservableCollection<DadoEntrada>)dtgGrade.ItemsSource;

            if (itemsSource != null)
            {
                try
                {
                    using (var conn = new Npgsql.NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
                    {
                        conn.Open();

                        foreach (var item in itemsSource)
                        {
                            if (string.IsNullOrWhiteSpace(item.NrGramatura) ||
                                string.IsNullOrWhiteSpace(item.NrLargura) ||
                                string.IsNullOrWhiteSpace(item.StAlvejante) ||
                                string.IsNullOrWhiteSpace(item.NrTracaoSeca) ||
                                string.IsNullOrWhiteSpace(item.NrTracaoUmida) ||
                                string.IsNullOrWhiteSpace(item.NrPorosidadeGurley) ||
                                string.IsNullOrWhiteSpace(item.NrAbsorcaoCoob) ||
                                string.IsNullOrWhiteSpace(item.DsStatusAmostra) ||
                                string.IsNullOrWhiteSpace(item.NmResponsavel) ||
                                string.IsNullOrWhiteSpace(item.DsDesvio))
                            {
                                continue;
                            }

                            string queryLoteFornecedor = "SELECT \"IdLoteFornecedor\" FROM \"T02_Recebimento\" WHERE \"IdRecebimento\" = @IdRecebimento";
                            string idLoteFornecedor = null;

                            using (var cmdLoteFornecedor = new Npgsql.NpgsqlCommand(queryLoteFornecedor, conn))
                            {
                                cmdLoteFornecedor.Parameters.AddWithValue("IdRecebimento", item.IdRecebimento);
                                idLoteFornecedor = (string)cmdLoteFornecedor.ExecuteScalar();
                            }

                            string comando = "INSERT INTO public.\"T0305_ResultadoAmostra\" (\"IdRecebimento\", \"NrGramatura\", \"NrLargura\", \"StAlvejante\",\"NrTracaoSeca\", \"NrTracaoUmida\", \"NrPorosidadeGurley\", \"NrAbsorcaoCoob\", \"DsStatusAmostra\", \"NmResponsavel\", \"DsDesvio\")" +
                                "SELECT RCB.\"IdRecebimento\", @NrGramatura, @NrLargura, @StAlvejante, @NrTracaoSeca, @NrTracaoUmida, @NrPorosidadeGurley, @NrAbsorcaoCoob, @DsStatusAmostra, @NmResponsavel, @DsDesvio FROM \"T02_Recebimento\" RCB WHERE RCB.\"IdLoteFornecedor\" = @IdLoteFornecedor AND NOT EXISTS(SELECT 1 FROM public.\"T03_AmostraMateriaPrima\" AMP WHERE AMP.\"IdRecebimento\" = RCB.\"IdRecebimento\")";

                            using (var cmdInsert = new Npgsql.NpgsqlCommand(comando, conn))
                            {
                                cmdInsert.Parameters.AddWithValue("IdRecebimento", item.IdRecebimento);
                                cmdInsert.Parameters.AddWithValue("NrGramatura", double.Parse(item.NrGramatura));
                                cmdInsert.Parameters.AddWithValue("NrLargura", int.Parse(item.NrLargura));
                                cmdInsert.Parameters.AddWithValue("StAlvejante", bool.Parse(item.StAlvejante));
                                cmdInsert.Parameters.AddWithValue("NrTracaoSeca", double.Parse(item.NrTracaoSeca));
                                cmdInsert.Parameters.AddWithValue("NrTracaoUmida", double.Parse(item.NrTracaoUmida));
                                cmdInsert.Parameters.AddWithValue("NrPorosidadeGurley", double.Parse(item.NrPorosidadeGurley));
                                cmdInsert.Parameters.AddWithValue("NrAbsorcaoCoob", double.Parse(item.NrAbsorcaoCoob));
                                cmdInsert.Parameters.AddWithValue("DsStatusAmostra", item.DsStatusAmostra);
                                cmdInsert.Parameters.AddWithValue("NmResponsavel", item.NmResponsavel);
                                cmdInsert.Parameters.AddWithValue("DsDesvio", item.DsDesvio);
                                cmdInsert.Parameters.AddWithValue("IdLoteFornecedor", idLoteFornecedor);

                                cmdInsert.ExecuteNonQuery();
                            }

                            string updateQuery = "UPDATE \"T02_Recebimento\" SET \"DsStatus\" = 'Aprovado' WHERE \"IdLoteFornecedor\" = @IdLoteFornecedor";

                            using (var cmdUpdate = new Npgsql.NpgsqlCommand(updateQuery, conn))
                            {
                                cmdUpdate.Parameters.AddWithValue("IdLoteFornecedor", idLoteFornecedor);
                                cmdUpdate.ExecuteNonQuery();
                            }
                        }
                    }

                    CF0302 materiaPrima = new CF0302();
                    materiaPrima.Show();
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao inserir dados: " + ex.Message);
                }
            }
        }

        private void btnInformacoes_Click(object sender, RoutedEventArgs e)
        {
            string queryCaracteristicas = "SELECT * FROM public.\"T09_Caracteristica\";";
            DataTable tabelaCaracteristicas = db.ExecutarConsulta(queryCaracteristicas);

            ObservableCollection<Caracteristica> caracteristicas = new ObservableCollection<Caracteristica>();

            foreach (DataRow row in tabelaCaracteristicas.Rows)
            {
                caracteristicas.Add(new Caracteristica
                {
                    NrMinimo = row["NrMinimo"].ToString(),
                    NrMaximo = row["NrMaximo"].ToString(),
                    DsTarget = row["DsTarget"].ToString(),
                    DsCaracteristica = row["DsCaracteristica"].ToString()
                });
            }

            dtgCaracteristicas.ItemsSource = caracteristicas;

            infoPopup.IsOpen = true;
        }

        private void FecharPopup_Click(object sender, RoutedEventArgs e)
        {
            infoPopup.IsOpen = false;
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0302 materiaPrima = new CF0302();
            materiaPrima.Show();
            Close();
        }
    }
}