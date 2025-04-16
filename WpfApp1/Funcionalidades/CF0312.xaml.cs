using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0312 : Window
    {
        private Consultar db;

        public class DadoEntrada
        {
            public string IdRecebimento { get; set; }
            public string NrReleaseDireita { get; set; }
            public string NrReleaseMeio { get; set; }
            public string NrReleaseEsquerda { get; set; }
            public string NrReleaseMedia { get; set; }
            public string NrAdesao { get; set; }
            public string NrMigracao { get; set; }
            public string StAlvejante { get; set; }
            public string NrLargura { get; set; }
            public string NrCor { get; set; }
            public string NrPorosidadeGurley { get; set; }
            public string NrAbsorcaoCoob { get; set; }
            public string NrGrMP { get; set; }
            public string NrGrSA { get; set; }
            public string DsDesvio { get; set; }
            public string DsStatus { get; set; }
            public string DsStatusSiliconizacao { get; set; }
        }

        public CF0312()
        {
            InitializeComponent();
            db = new Consultar();

            string query = "SELECT SLC.\"IdRecebimento\", SMA.\"NrReleaseDireita\", SMA.\"NrReleaseMeio\", SMA.\"NrReleaseEsquerda\", SMA.\"NrReleaseMedia\", SMA.\"NrAdesao\", SMA.\"NrMigracao\", SMA.\"StAlvejante\", SMA.\"NrLargura\", SMA.\"NrCor\", SMA.\"NrPorosidadeGurley\", SMA.\"NrAbsorcaoCoob\", SMA.\"NrGrMP\", SMA.\"NrGrSA\", SMA.\"DsStatus\", SMA.\"DsDesvio\" FROM \"T04_Siliconizacao\" SLC LEFT JOIN \"T0302_SemiAcabado\" SMA ON SMA.\"IdRecebimento\" = SLC.\"IdRecebimento\" WHERE SLC.\"DsStatusSiliconizacao\" = 'Laboratório';";

            DataTable tabela = db.ExecutarConsulta(query);

            ObservableCollection<DadoEntrada> materiaprima = new ObservableCollection<DadoEntrada>();

            foreach (DataRow row in tabela.Rows)
            {
                materiaprima.Add(new DadoEntrada
                {
                    IdRecebimento = row["IdRecebimento"].ToString(),
                    NrReleaseDireita = row["NrReleaseDireita"].ToString(),
                    NrReleaseMeio = row["NrReleaseMeio"].ToString(),
                    NrReleaseEsquerda = row["NrReleaseEsquerda"].ToString(),
                    NrReleaseMedia = row["NrReleaseMedia"].ToString(),
                    NrAdesao = row["NrAdesao"].ToString(),
                    NrMigracao = row["NrMigracao"].ToString(),
                    StAlvejante = row["StAlvejante"].ToString(),
                    NrLargura = row["NrLargura"].ToString(),
                    NrCor = row["NrCor"].ToString(),
                    NrPorosidadeGurley = row["NrPorosidadeGurley"].ToString(),
                    NrAbsorcaoCoob = row["NrAbsorcaoCoob"].ToString(),
                    NrGrMP = row["NrGrMP"].ToString(),
                    NrGrSA = row["NrGrSA"].ToString(),
                    DsDesvio = row["DsDesvio"].ToString(),
                    DsStatus = row["DsStatus"].ToString(),
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
                            if (string.IsNullOrEmpty(item.IdRecebimento) ||
                                string.IsNullOrEmpty(item.NrReleaseDireita) &&
                                string.IsNullOrEmpty(item.NrReleaseMeio) &&
                                string.IsNullOrEmpty(item.NrReleaseEsquerda) &&
                                string.IsNullOrEmpty(item.NrReleaseMedia) &&
                                string.IsNullOrEmpty(item.NrAdesao) &&
                                string.IsNullOrEmpty(item.NrMigracao) &&
                                string.IsNullOrEmpty(item.StAlvejante) &&
                                string.IsNullOrEmpty(item.NrLargura) &&
                                string.IsNullOrEmpty(item.NrCor) &&
                                string.IsNullOrEmpty(item.NrPorosidadeGurley) &&
                                string.IsNullOrEmpty(item.NrAbsorcaoCoob) &&
                                string.IsNullOrEmpty(item.NrGrMP) &&
                                string.IsNullOrEmpty(item.NrGrSA) &&
                                string.IsNullOrEmpty(item.DsDesvio) &&
                                string.IsNullOrEmpty(item.DsStatus) &&
                                string.IsNullOrEmpty(item.DsStatusSiliconizacao))
                            {
                                continue;
                            }

                            string comando = "INSERT INTO public.\"T0302_SemiAcabado\" (\"IdRecebimento\"" +
                                ",\"NrReleaseDireita\"" +
                                ",\"NrReleaseMeio\"" +
                                ",\"NrReleaseEsquerda\"" +
                                ",\"NrReleaseMedia\"" +
                                ",\"NrAdesao\"" +
                                ",\"NrMigracao\"" +
                                ",\"StAlvejante\"" +
                                ",\"NrLargura\"" +
                                ",\"NrCor\"" +
                                ",\"NrPorosidadeGurley\"" +
                                ",\"NrAbsorcaoCoob\"" +
                                ",\"NrGrMP\"" +
                                ",\"NrGrSA\"" +
                                ",\"DsDesvio\"" +
                                ",\"DsStatus\"" +
                                ",\"DsStatusSiliconizacao\")" +
                                             "SELECT SLC.\"IdRecebimento\", @NrReleaseDireita, @NrReleaseMeio, @NrReleaseEsquerda, @NrReleaseMedia, @NrAdesao, @NrMigracao, @StAlvejante, @NrLargura, @NrCor, @NrPorosidadeGurley, @NrAbsorcaoCoob, @NrGrMP, @NrGrSA, @DsDesvio, @DsStatus, @DsStatusSiliconizacao " +
                                             "FROM \"T04_Siliconizacao\" SLC " +
                                             "WHERE SLC.\"IdRecebimento\" = @IdRecebimento";

                            using (var cmdInsert = new Npgsql.NpgsqlCommand(comando, conn))
                            {
                                cmdInsert.Parameters.AddWithValue("IdRecebimento", string.IsNullOrEmpty(item.IdRecebimento) ? DBNull.Value : item.IdRecebimento);
                                cmdInsert.Parameters.AddWithValue("NrReleaseDireita", double.TryParse(item.NrReleaseDireita, out double nrReleaseDireita) ? nrReleaseDireita : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrReleaseMeio", double.TryParse(item.NrReleaseMeio, out double nrReleaseMeio) ? nrReleaseMeio : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrReleaseEsquerda", double.TryParse(item.NrReleaseEsquerda, out double nrReleaseEsquerda) ? nrReleaseEsquerda : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrReleaseMedia", double.TryParse(item.NrReleaseMedia, out double nrReleaseMedia) ? nrReleaseMedia : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrAdesao", double.TryParse(item.NrAdesao, out double nrAdesao) ? nrAdesao : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrMigracao", double.TryParse(item.NrMigracao, out double nrMigracao) ? nrMigracao : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("StAlvejante", bool.TryParse(item.StAlvejante, out bool stAlvejante) ? stAlvejante : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrLargura", double.TryParse(item.NrLargura, out double nrLargura) ? nrLargura : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrCor", double.TryParse(item.NrCor, out double nrCor) ? nrCor : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrPorosidadeGurley", double.TryParse(item.NrPorosidadeGurley, out double nrPorosidadeGurley) ? nrPorosidadeGurley : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrAbsorcaoCoob", double.TryParse(item.NrAbsorcaoCoob, out double nrAbsorcaoCoob) ? nrAbsorcaoCoob : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrGrMP", double.TryParse(item.NrGrMP, out double nrGrMP) ? nrGrMP : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrGrSA", double.TryParse(item.NrGrSA, out double nrGrSA) ? nrGrSA : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("DsDesvio", string.IsNullOrEmpty(item.DsDesvio) ? DBNull.Value : item.DsDesvio);
                                cmdInsert.Parameters.AddWithValue("DsStatus", string.IsNullOrEmpty(item.DsStatus) ? DBNull.Value : item.DsStatus);
                                cmdInsert.Parameters.AddWithValue("DsStatusSiliconizacao", string.IsNullOrEmpty(item.DsStatusSiliconizacao) ? DBNull.Value : item.DsStatusSiliconizacao);

                                cmdInsert.ExecuteNonQuery();
                            }

                            string updateQuery = "UPDATE \"T0302_SemiAcabado\" SET \"DsStatusSiliconizacao\" = @DsStatus WHERE \"IdRecebimento\" = @IdRecebimento";

                            using (var cmdUpdate = new Npgsql.NpgsqlCommand(updateQuery, conn))
                            {
                                cmdUpdate.Parameters.AddWithValue("IdRecebimento", string.IsNullOrEmpty(item.IdRecebimento) ? DBNull.Value : item.IdRecebimento);
                                cmdUpdate.Parameters.AddWithValue("DsStatus", string.IsNullOrEmpty(item.DsStatus) ? DBNull.Value : item.DsStatus);
                                cmdUpdate.ExecuteNonQuery();
                            }

                            string updateQuery2 = "UPDATE \"T04_Siliconizacao\" SET \"DsStatusSiliconizacao\" = @DsStatus WHERE \"IdRecebimento\" = @IdRecebimento";

                            using (var cmdUpdate = new Npgsql.NpgsqlCommand(updateQuery2, conn))
                            {
                                cmdUpdate.Parameters.AddWithValue("IdRecebimento", string.IsNullOrEmpty(item.IdRecebimento) ? DBNull.Value : item.IdRecebimento);
                                cmdUpdate.Parameters.AddWithValue("DsStatus", string.IsNullOrEmpty(item.DsStatus) ? DBNull.Value : item.DsStatus);
                                cmdUpdate.ExecuteNonQuery();
                            }
                        }
                    }

                    CF0308 main = new CF0308();
                    main.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao inserir dados: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("A fonte de dados não é uma ObservableCollection ou tipo esperado. Por favor, verifique!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0308 main = new CF0308();
            main.Show();
            Close();
        }
    }
}