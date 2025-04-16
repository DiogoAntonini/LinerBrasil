using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0306 : Window
    {
        private Consultar db;

        public class CaracteristicaModel
        {
            public string DsCaracteristica { get; set; }
            public string DsCaracteristicaOriginal { get; set; }

            public string NrMaximo { get; set; }
            public string NrMaximoOriginal { get; set; }

            public string NrMinimo { get; set; }
            public string NrMinimoOriginal { get; set; }

            public string DsTarget { get; set; }
            public string DsTargetOriginal { get; set; }
        }

        public CF0306()
        {
            InitializeComponent();
            db = new Consultar();

            string query = "SELECT CRT.\"DsCaracteristica\",  CRT.\"DsTarget\", CRT.\"NrMaximo\", CRT.\"NrMinimo\" " +
                           "FROM \"T09_Caracteristica\" CRT;";

            DataTable tabela = db.ExecutarConsulta(query);

            List<CaracteristicaModel> caracteristicas = new List<CaracteristicaModel>();

            foreach (DataRow row in tabela.Rows)
            {
                caracteristicas.Add(new CaracteristicaModel
                {
                    DsCaracteristica = row["DsCaracteristica"].ToString(),
                    DsCaracteristicaOriginal = row["DsCaracteristica"].ToString(),
                    DsTarget = row["DsTarget"].ToString(),
                    DsTargetOriginal = row["DsTarget"].ToString(),
                    NrMaximo = row["NrMaximo"].ToString(),
                    NrMaximoOriginal = row["NrMaximo"].ToString(),
                    NrMinimo = row["NrMinimo"].ToString(),
                    NrMinimoOriginal = row["NrMinimo"].ToString()
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
                            if (item.DsCaracteristica != item.DsCaracteristicaOriginal ||
                                item.NrMaximo != item.NrMaximoOriginal ||
                                item.NrMinimo != item.NrMinimoOriginal ||
                                item.DsTarget != item.DsTargetOriginal)
                            {
                                string comando = "UPDATE public.\"T09_Caracteristica\" SET " +
                                                 "\"DsCaracteristica\" = @DsCaracteristica, " +
                                                 "\"NrMaximo\" = @NrMaximo, " +
                                                 "\"NrMinimo\" = @NrMinimo, " +
                                                 "\"DsTarget\" = @DsTarget " +
                                                 "WHERE \"DsCaracteristica\" = @DsCaracteristica";

                                using (var cmd = new Npgsql.NpgsqlCommand(comando, conn))
                                {
                                    cmd.Parameters.AddWithValue("DsCaracteristica", string.IsNullOrEmpty(item.DsCaracteristica) ? DBNull.Value : item.DsCaracteristica);
                                    cmd.Parameters.AddWithValue("NrMaximo", string.IsNullOrEmpty(item.NrMaximo) ? DBNull.Value : item.NrMaximo);
                                    cmd.Parameters.AddWithValue("NrMinimo", string.IsNullOrEmpty(item.NrMinimo) ? DBNull.Value : item.NrMinimo);
                                    cmd.Parameters.AddWithValue("DsTarget", string.IsNullOrEmpty(item.DsTarget) ? DBNull.Value : item.DsTarget);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    CF0304 caracteristica = new CF0304();
                    caracteristica.Show();
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
            CF0304 caracteristica = new CF0304();
            caracteristica.Show();
            this.Close();
        }
    }
}