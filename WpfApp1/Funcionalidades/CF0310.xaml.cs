using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0310 : Window
    {
        private Consultar db;

        public class CaracteristicaModel
        {
            public string HrInicioFormatada { get; set; }
            public string HrInicioFormatadaOriginal { get; set; }

            public string HrFimFormatada { get; set; }
            public string HrFimFormatadaOriginal { get; set; }

            public string NrMetragem { get; set; }
            public string NrMetragemOriginal { get; set; }

            public string NrOS { get; set; }
            public string NrOSOriginal { get; set; }

            public string NmCliente { get; set; }
            public string NmClienteOriginal { get; set; }

            public string DtProducaoFormatada { get; set; }
            public string DtProducaoFormatadaOriginal { get; set; }

            public string DsTurno { get; set; }
            public string DsTurnoOriginal { get; set; }

            public string DsPapel { get; set; }
            public string DsPapelOriginal { get; set; }

            public string NrTotalMetragem { get; set; }
            public string NrTotalMetragemOriginal { get; set; }
        }

        public CF0310()
        {
            InitializeComponent();
            db = new Consultar();

            string query = "SELECT TO_CHAR(\"HrInicio\", 'HH24:MI') \"HrInicioFormatada\", TO_CHAR(\"HrFim\", 'HH24:MI') \"HrFimFormatada\", EXT.\"NrMetragem\", EXT.\"NrOS\", EXT.\"NmCliente\", TO_CHAR(\"DtProducao\", 'DD/MM/YY') \"DtProducaoFormatada\", EXT.\"DsTurno\", EXT.\"DsPapel\", EXT.\"NrTotalMetragem\" FROM \"T10_Extrusao\" EXT;";

            DataTable tabela = db.ExecutarConsulta(query);

            List<CaracteristicaModel> caracteristicas = new List<CaracteristicaModel>();

            foreach (DataRow row in tabela.Rows)
            {
                caracteristicas.Add(new CaracteristicaModel
                {
                    HrInicioFormatada = row["HrInicioFormatada"].ToString(),
                    HrInicioFormatadaOriginal = row["HrInicioFormatada"].ToString(),

                    HrFimFormatada = row["HrFimFormatada"].ToString(),
                    HrFimFormatadaOriginal = row["HrFimFormatada"].ToString(),

                    NrMetragem = row["NrMetragem"].ToString(),
                    NrMetragemOriginal = row["NrMetragem"].ToString(),

                    NrOS = row["NrOS"].ToString(),
                    NrOSOriginal = row["NrOS"].ToString(),

                    NmCliente = row["NmCliente"].ToString(),
                    NmClienteOriginal = row["NmCliente"].ToString(),

                    DtProducaoFormatada = row["DtProducaoFormatada"].ToString(),
                    DtProducaoFormatadaOriginal = row["DtProducaoFormatada"].ToString(),

                    DsTurno = row["DsTurno"].ToString(),
                    DsTurnoOriginal = row["DsTurno"].ToString(),

                    DsPapel = row["DsPapel"].ToString(),
                    DsPapelOriginal = row["DsPapel"].ToString(),

                    NrTotalMetragem = row["NrTotalMetragem"].ToString(),
                    NrTotalMetragemOriginal = row["NrTotalMetragem"].ToString(),
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
                            if (item.HrInicioFormatada != item.HrInicioFormatadaOriginal ||
                                item.HrFimFormatada != item.HrFimFormatadaOriginal ||
                                item.NrMetragem != item.NrMetragemOriginal ||
                                item.NrOS != item.NrOSOriginal ||
                                item.NmCliente != item.NmClienteOriginal ||
                                item.DtProducaoFormatada != item.DtProducaoFormatadaOriginal ||
                                item.DsTurno != item.DsTurnoOriginal ||
                                item.DsPapel != item.DsPapelOriginal ||
                                item.NrTotalMetragem != item.NrTotalMetragemOriginal)
                            {
                                string comando = "UPDATE public.\"T10_Extrusao\" SET " +
                                                 "\"HrInicio\" = @HrInicio, " +
                                                 "\"HrFim\" = @HrFim, " +
                                                 "\"NrMetragem\" = @NrMetragem, " +
                                                 "\"NrOS\" = @NrOS, " +
                                                 "\"NmCliente\" = @NmCliente, " +
                                                 "\"DtProducao\" = @DtProducao, " +
                                                 "\"DsTurno\" = @DsTurno, " +
                                                 "\"DsPapel\" = @DsPapel, " +
                                                 "\"NrTotalMetragem\" = @NrTotalMetragem " +
                                                 "WHERE \"NrOS\" = @NrOS;";

                                using (var cmd = new Npgsql.NpgsqlCommand(comando, conn))
                                {
                                    cmd.Parameters.AddWithValue("HrInicio", DateTime.TryParse(item.HrInicioFormatada, out DateTime hrInicioFormatada) ? hrInicioFormatada : DBNull.Value);
                                    cmd.Parameters.AddWithValue("HrFim", DateTime.TryParse(item.HrFimFormatada, out DateTime hrFimFormatada) ? hrFimFormatada : DBNull.Value);
                                    cmd.Parameters.AddWithValue("NrMetragem", string.IsNullOrEmpty(item.NrMetragem) ? DBNull.Value : item.NrMetragem);
                                    cmd.Parameters.AddWithValue("NrOS", string.IsNullOrEmpty(item.NrOS) ? DBNull.Value : item.NrOS);
                                    cmd.Parameters.AddWithValue("NmCliente", string.IsNullOrEmpty(item.NmCliente) ? DBNull.Value : item.NmCliente);
                                    cmd.Parameters.AddWithValue("DtProducao", DateTime.TryParse(item.DtProducaoFormatada, out DateTime dtProducaoFormatada) ? dtProducaoFormatada : DBNull.Value);
                                    cmd.Parameters.AddWithValue("DsTurno", string.IsNullOrEmpty(item.DsTurno) ? DBNull.Value : item.DsTurno);
                                    cmd.Parameters.AddWithValue("DsPapel", string.IsNullOrEmpty(item.DsPapel) ? DBNull.Value : item.DsPapel);
                                    cmd.Parameters.AddWithValue("NrTotalMetragem", string.IsNullOrEmpty(item.NrTotalMetragem) ? DBNull.Value : item.NrTotalMetragem);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    CF0308 extrusao = new CF0308();
                    extrusao.Show();
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
            CF0308 extrusao = new CF0308();
            extrusao.Show();
            Close();
        }
    }
}