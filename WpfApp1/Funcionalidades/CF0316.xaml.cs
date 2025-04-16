using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0316 : Window
    {
        private Consultar db;

        public class DadoEntrada
        {
            public string IdRecebimento { get; set; }
            public string CdTinta { get; set; }
            public string IdLoteFornecedor { get; set; }
            public string NmFornecedor { get; set; }
            public string DtAmostra { get; set; }
            public string HrAmostra { get; set; }
            public string DtValidade { get; set; }
            public string StCor { get; set; }
            public string NrPesoDoVolume { get; set; }
            public string NrViscosidade { get; set; }
            public string DsStatusAmostra { get; set; }
            public string NmInspetor { get; set; }
        }

        public class Caracteristica
        {
            public string NrMinimo { get; set; }
            public string DsTarget { get; set; }
            public string NrMaximo { get; set; }
            public string DsCaracteristica { get; set; }
        }

        public CF0316()
        {
            InitializeComponent();
            db = new Consultar();

            string query = "SELECT RBT.\"IdRecebimento\", RBT.\"CdTinta\", RBT.\"IdLoteFornecedor\", RBT.\"NmFornecedor\", TO_CHAR(AMT.\"DtAmostra\", 'DD/MM/YYYY') \"DtAmostra\", TO_CHAR(AMT.\"HrAmostra\", 'DD/MM/YYYY') \"HrAmostra\", AMT.\"StCor\" , AMT.\"NrPesoDoVolume\" , AMT.\"NrViscosidade\" , AMT.\"DsStatusAmostra\", AMT.\"NmInspetor\" FROM \"T0203_RecebimentoTinta\" RBT LEFT JOIN \"T0306_AmostraTinta\" AMT ON AMT.\"IdRecebimento\" = RBT.\"IdRecebimento\" WHERE RBT.\"DsStatus\" = 'Laboratório';";

            DataTable tabela = db.ExecutarConsulta(query);

            ObservableCollection<DadoEntrada> materiaprima = new ObservableCollection<DadoEntrada>();

            foreach (DataRow row in tabela.Rows)
            {
                materiaprima.Add(new DadoEntrada
                {
                    IdRecebimento = row["IdRecebimento"].ToString(),
                    CdTinta = row["CdTinta"].ToString(),
                    IdLoteFornecedor = row["IdLoteFornecedor"].ToString(),
                    NmFornecedor = row["NmFornecedor"].ToString(),
                    StCor = row["StCor"].ToString(),
                    NrPesoDoVolume = row["NrPesoDoVolume"].ToString(),
                    NrViscosidade = row["NrViscosidade"].ToString(),
                    DsStatusAmostra = row["DsStatusAmostra"].ToString(),
                    NmInspetor = row["NmInspetor"].ToString(),
                    DtAmostra = DateTime.Now.ToShortDateString(),
                    HrAmostra = DateTime.Now.ToString("HH:mm")
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
                            string queryLoteFornecedor = "SELECT \"IdLoteFornecedor\" FROM \"T02_Recebimento\" WHERE \"IdRecebimento\" = @IdRecebimento";
                            string idLoteFornecedor = null;

                            using (var cmdLoteFornecedor = new Npgsql.NpgsqlCommand(queryLoteFornecedor, conn))
                            {
                                cmdLoteFornecedor.Parameters.AddWithValue("IdRecebimento", item.IdRecebimento);
                                idLoteFornecedor = (string)cmdLoteFornecedor.ExecuteScalar();
                            }

                            string comando = "INSERT INTO public.\"T03_AmostraMateriaPrima\" (\"IdRecebimento\", \"NrGramatura\", \"NrLargura\", \"StAlvejante\",\"NrTracaoSeca\", \"NrTracaoUmida\", \"NrPorosidadeGurley\", \"NrAbsorcaoCoob\", \"DsStatusAmostra\", \"NmResponsavel\", \"DsDesvio\")                                " +
                                "SELECT RCB.\"IdRecebimento\", @NrGramatura, @NrLargura, @StAlvejante, @NrTracaoSeca, @NrTracaoUmida, @NrPorosidadeGurley, @NrAbsorcaoCoob, @DsStatusAmostra, @NmResponsavel, @DsDesvio FROM \"T02_Recebimento\" RCB WHERE RCB.\"IdLoteFornecedor\" = @IdLoteFornecedor AND NOT EXISTS(SELECT 1 FROM public.\"T03_AmostraMateriaPrima\" AMP WHERE AMP.\"IdRecebimento\" = RCB.\"IdRecebimento\")";

                            using (var cmdInsert = new Npgsql.NpgsqlCommand(comando, conn))
                            {
                                //cmdInsert.Parameters.AddWithValue("NrGramatura", string.IsNullOrEmpty(item.NrGramatura) ? DBNull.Value : item.NrGramatura);
                                //cmdInsert.Parameters.AddWithValue("NrLargura", int.TryParse(item.NrLargura, out int nrLargura) ? nrLargura : DBNull.Value);
                                //cmdInsert.Parameters.AddWithValue("StAlvejante", bool.TryParse(item.StAlvejante, out bool stAlvejante) ? stAlvejante : DBNull.Value);
                                //cmdInsert.Parameters.AddWithValue("NrTracaoSeca", string.IsNullOrEmpty(item.NrTracaoSeca) ? DBNull.Value : item.NrTracaoSeca);
                                //cmdInsert.Parameters.AddWithValue("NrTracaoUmida", string.IsNullOrEmpty(item.NrTracaoUmida) ? DBNull.Value : item.NrTracaoUmida);
                                //cmdInsert.Parameters.AddWithValue("NrPorosidadeGurley", string.IsNullOrEmpty(item.NrPorosidadeGurley) ? DBNull.Value : item.NrPorosidadeGurley);
                                //cmdInsert.Parameters.AddWithValue("NrAbsorcaoCoob", string.IsNullOrEmpty(item.NrAbsorcaoCoob) ? DBNull.Value : item.NrAbsorcaoCoob);
                                //cmdInsert.Parameters.AddWithValue("DsStatusAmostra", string.IsNullOrEmpty(item.DsStatusAmostra) ? DBNull.Value : item.DsStatusAmostra);
                                //cmdInsert.Parameters.AddWithValue("NmResponsavel", string.IsNullOrEmpty(item.NmResponsavel) ? DBNull.Value : item.NmResponsavel);
                                //cmdInsert.Parameters.AddWithValue("DsDesvio", string.IsNullOrEmpty(item.DsDesvio) ? DBNull.Value : item.DsDesvio);
                                //cmdInsert.Parameters.AddWithValue("IdLoteFornecedor", idLoteFornecedor);

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao inserir dados: " + ex.Message);
                }
            }

            CF0302 materiaPrima = new CF0302();
            materiaPrima.Show();
            Close();
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