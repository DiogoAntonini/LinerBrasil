using System.Windows;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0406 : Window
    {
        private string usuarioId;
        private Consultar db;

        public CF0406()
        {
            InitializeComponent();
            DtInicial.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            DtFinal.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            db = new Consultar();
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idMaquina = Convert.ToInt32(IdMaquina.Text);
                string dtInicial = DtInicial.Text;
                string dtFinal = DtFinal.Text;
                string cdOS = CdOS.Text;
                int idTipoParada = Convert.ToInt32(IdTipoParada.Text);
                string dsMotivo = DsMotivo.Text;

                DateTime? dtInicialDate = string.IsNullOrEmpty(dtInicial) ? (DateTime?)null : DateTime.Parse(dtInicial);
                DateTime? dtFinalDate = string.IsNullOrEmpty(dtFinal) ? (DateTime?)null : DateTime.Parse(dtFinal);

                using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
                {
                    conn.Open();

                    string comando = "INSERT INTO public.\"T0401_ParadasSiliconizacao\" (\"IdMaquina\", \"DtInicial\", \"DtFinal\", \"CdOS\", \"IdTipoParada\", \"DsMotivo\")" +
                                     "VALUES (@IdMaquina, @DtInicial, @DtFinal, @CdOS, @IdTipoParada, @DsMotivo)";

                    using (var cmdInsert = new NpgsqlCommand(comando, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("IdMaquina", idMaquina == 0 ? DBNull.Value : (object)idMaquina);
                        cmdInsert.Parameters.AddWithValue("DtInicial", dtInicialDate.HasValue ? (object)dtInicialDate.Value : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("DtFinal", dtFinalDate.HasValue ? (object)dtFinalDate.Value : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("CdOS", string.IsNullOrEmpty(cdOS) ? DBNull.Value : cdOS);
                        cmdInsert.Parameters.AddWithValue("IdTipoParada", idTipoParada == 0 ? DBNull.Value : (object)idTipoParada);
                        cmdInsert.Parameters.AddWithValue("DsMotivo", string.IsNullOrEmpty(dsMotivo) ? DBNull.Value : dsMotivo);

                        cmdInsert.ExecuteNonQuery();
                    }
                }

                CF0401 main = new CF0401(usuarioId);
                main.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inserir dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0401 main = new CF0401(usuarioId);
            main.Show();
            this.Close();
        }
    }
}