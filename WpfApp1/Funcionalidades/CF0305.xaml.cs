using System.Collections.ObjectModel;
using System.Windows;

namespace LinerApp.Funcionalidades
{
    public partial class CF0305 : Window
    {

        public class DadoEntrada
        {
            public string DsCaracteristica { get; set; }
            public string NrMaximo { get; set; }
            public string NrMinimo { get; set; }
            public string DsTarget { get; set; }
        }

        public CF0305()
        {
            InitializeComponent();

            ObservableCollection<DadoEntrada> listaDeDados = new ObservableCollection<DadoEntrada> { };

            dtgGrade.ItemsSource = listaDeDados;
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
                            string comando = "INSERT INTO public.\"T09_Caracteristica\" (\"DsCaracteristica\", \"DsTarget\", \"NrMaximo\", \"NrMinimo\")" +
                                             "VALUES (@DsCaracteristica, @DsTarget, @NrMaximo, @NrMinimo)";

                            using (var cmdInsert = new Npgsql.NpgsqlCommand(comando, conn))
                            {
                                cmdInsert.Parameters.AddWithValue("DsCaracteristica", string.IsNullOrEmpty(item.DsCaracteristica) ? DBNull.Value : item.DsCaracteristica);
                                cmdInsert.Parameters.AddWithValue("DsTarget", string.IsNullOrEmpty(item.DsTarget) ? DBNull.Value : item.DsTarget);
                                cmdInsert.Parameters.AddWithValue("NrMaximo", string.IsNullOrEmpty(item.NrMaximo) ? DBNull.Value : item.NrMaximo);
                                cmdInsert.Parameters.AddWithValue("NrMinimo", string.IsNullOrEmpty(item.NrMinimo) ? DBNull.Value : item.NrMinimo);

                                cmdInsert.ExecuteNonQuery();
                            }
                        }
                    }

                    CF0304 cadastro = new CF0304();
                    cadastro.Show();
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao inserir dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("A fonte de dados não é uma ObservableCollection ou tipo esperado. Por favor, verifique!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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