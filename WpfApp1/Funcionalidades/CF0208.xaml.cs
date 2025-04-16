using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0208 : Window
    {
        private readonly string usuarioId;
        private readonly Consultar db;

        public class MateriaModel
        {
            public string DsUnidade { get; set; }
            public string NmDescricao { get; set; }
            public int IdMateriaRecebimento { get; set; }
            public string StAnalise { get; set; }
        }

        public CF0208(string usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            db = new Consultar();
            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                string query = "SELECT CMR.\"IdMateriaRecebimento\", CMR.\"NmDescricao\", CMR.\"DsUnidade\", CMR.\"StAnalise\" FROM \"T0201_CadastroMateriaRecebimento\" CMR;";
                DataTable tabela = await Task.Run(() => db.ExecutarConsulta(query));

                List<MateriaModel> materia = new List<MateriaModel>();

                foreach (DataRow row in tabela.Rows)
                {
                    materia.Add(new MateriaModel
                    {
                        NmDescricao = row["NmDescricao"].ToString(),
                        IdMateriaRecebimento = int.Parse(row["IdMateriaRecebimento"].ToString()),
                        StAnalise = row["StAnalise"].ToString(),
                        DsUnidade = row["DsUnidade"].ToString()
                    });
                }

                dtgGrade.ItemsSource = new ObservableCollection<MateriaModel>(materia);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var row = button.DataContext as MateriaModel;
                if (row != null)
                {
                    try
                    {
                        await DeleteMateriaAsync(row.IdMateriaRecebimento);

                        var itemsSource = dtgGrade.ItemsSource as ObservableCollection<MateriaModel>;
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

        private async Task DeleteMateriaAsync(int idMateriaRecebimento)
        {
            using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
            {
                await conn.OpenAsync();

                string comando = @"DELETE FROM ""T0201_CadastroMateriaRecebimento"" WHERE ""IdMateriaRecebimento"" = @IdMateriaRecebimento;";
                using (var cmd = new NpgsqlCommand(comando, conn))
                {
                    cmd.Parameters.AddWithValue("IdMateriaRecebimento", idMateriaRecebimento);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0201 main = new CF0201(usuarioId);
            main.Show();
            this.Close();
        }
    }
}