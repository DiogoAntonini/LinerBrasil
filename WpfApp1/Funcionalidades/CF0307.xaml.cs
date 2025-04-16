using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0307 : Window
    {
        private Consultar db;

        public class CaracteristicaModel
        {
            public string DsCaracteristica { get; set; }
            public string NrMaximo { get; set; }
            public string NrMinimo { get; set; }
            public string DsTarget { get; set; }
        }

        public CF0307()
        {
            InitializeComponent();
            db = new Consultar();

            string query = "SELECT CRT.\"DsCaracteristica\", CRT.\"DsTarget\", CRT.\"NrMaximo\", CRT.\"NrMinimo\" " +
                           "FROM \"T09_Caracteristica\" CRT";

            DataTable tabela = db.ExecutarConsulta(query);

            List<CaracteristicaModel> caracteristicas = new List<CaracteristicaModel>();

            foreach (DataRow row in tabela.Rows)
            {
                caracteristicas.Add(new CaracteristicaModel
                {
                    DsCaracteristica = row["DsCaracteristica"].ToString(),
                    DsTarget = row["DsTarget"].ToString(),
                    NrMaximo = row["NrMaximo"].ToString(),
                    NrMinimo = row["NrMinimo"].ToString()
                });
            }

            dtgGrade.ItemsSource = new ObservableCollection<CaracteristicaModel>(caracteristicas);
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var row = button.DataContext as CaracteristicaModel;
                if (row != null)
                {
                    try
                    {
                        using (var conn = new Npgsql.NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
                        {
                            conn.Open();

                            string comando = @"DELETE FROM public.""T09_Caracteristica"" WHERE ""DsCaracteristica"" = @DsCaracteristica;";
                            using (var cmd = new Npgsql.NpgsqlCommand(comando, conn))
                            {
                                cmd.Parameters.AddWithValue("DsCaracteristica", row.DsCaracteristica);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        var itemsSource = dtgGrade.ItemsSource as ObservableCollection<CaracteristicaModel>;
                        if (itemsSource != null)
                        {
                            itemsSource.Remove(row);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao excluir registro: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0304 especificacao = new CF0304();
            especificacao.Show();
            this.Close();
        }
    }
}