using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0207 : Window
    {
        private readonly string usuarioId;
        private readonly Consultar db;

        public class MateriaModel
        {
            public string DsUnidade { get; set; }
            public string DsUnidadeOriginal { get; set; }

            public string NmDescricao { get; set; }
            public string NmDescricaoOriginal { get; set; }

            public string IdMateriaRecebimento { get; set; }
            public string IdMateriaRecebimentoOriginal { get; set; }

            public string StAnalise { get; set; }
            public string StAnaliseOriginal { get; set; }

        }

        public CF0207(string usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            db = new Consultar();
            CarregarDadosAsync();
        }

        private async void CarregarDadosAsync()
        {
            string query = "SELECT CMR.\"NmDescricao\", CMR.\"DsUnidade\", CMR.\"IdMateriaRecebimento\", CMR.\"StAnalise\" FROM \"T0201_CadastroMateriaRecebimento\" CMR;";

            try
            {
                DataTable tabela = await Task.Run(() => db.ExecutarConsulta(query));
                List<MateriaModel> materia = new List<MateriaModel>();

                foreach (DataRow row in tabela.Rows)
                {
                    materia.Add(new MateriaModel
                    {
                        DsUnidade = row["DsUnidade"].ToString(),
                        DsUnidadeOriginal = row["DsUnidade"].ToString(),
                        NmDescricao = row["NmDescricao"].ToString(),
                        NmDescricaoOriginal = row["NmDescricao"].ToString(),
                        IdMateriaRecebimento = row["IdMateriaRecebimento"].ToString(),
                        IdMateriaRecebimentoOriginal = row["IdMateriaRecebimento"].ToString(),
                        StAnalise = row["StAnalise"].ToString(),
                        StAnaliseOriginal = row["StAnalise"].ToString()
                    });
                }

                dtgGrade.ItemsSource = new ObservableCollection<MateriaModel>(materia);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = (ObservableCollection<MateriaModel>)dtgGrade.ItemsSource;

            if (itemsSource != null)
            {
                try
                {
                    await AtualizarDadosAsync(itemsSource);
                    CF0201 main = new CF0201(usuarioId);
                    main.Show();
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

        private async Task AtualizarDadosAsync(ObservableCollection<MateriaModel> itemsSource)
        {
            using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
            {
                await conn.OpenAsync();

                foreach (var item in itemsSource)
                {
                    if (item.DsUnidade != item.DsUnidadeOriginal ||
                        item.StAnalise != item.StAnaliseOriginal ||
                        item.NmDescricao != item.NmDescricaoOriginal)
                    {
                        string comando = "UPDATE public.\"T0201_CadastroMateriaRecebimento\" SET " +
                                         "\"DsUnidade\" = @DsUnidade, " +
                                         "\"StAnalise\" = @StAnalise, " +
                                         "\"NmDescricao\" = @NmDescricao " +
                                         "WHERE \"IdMateriaRecebimento\" = @IdMateriaRecebimento";

                        using (var cmd = new NpgsqlCommand(comando, conn))
                        {
                            cmd.Parameters.AddWithValue("DsUnidade", string.IsNullOrEmpty(item.DsUnidade) ? DBNull.Value : item.DsUnidade);
                            cmd.Parameters.AddWithValue("NmDescricao", string.IsNullOrEmpty(item.NmDescricao) ? DBNull.Value : item.NmDescricao);
                            cmd.Parameters.AddWithValue("StAnalise", bool.TryParse(item.StAnalise, out bool stAnalise) ? stAnalise : DBNull.Value);
                            cmd.Parameters.AddWithValue("IdMateriaRecebimento", int.TryParse(item.IdMateriaRecebimento, out int idMateriaRecebimento) ? idMateriaRecebimento : (object)DBNull.Value);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0201 main = new CF0201(usuarioId);
            main.Show();
            Close();
        }
    }
}