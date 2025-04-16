using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;
using static LinerApp.Funcionalidades.CF0402;

namespace LinerApp.Funcionalidades
{
    public partial class CF0403 : Window
    {
        private string usuarioId;
        private Consultar db;

        public CF0403()
        {
            InitializeComponent();
            db = new Consultar();

            string query = "SELECT SLC.\"CdInterno\", SLC.\"IdMaquina\", TO_CHAR(SLC.\"DtSiliconizacao\", 'DD/MM/YY') \"DtSiliconizacao\", SLC.\"NmOperador\", SLC.\"CdOS\", SLC.\"NrMetrosLinear\", SLC.\"CdBobinaSiliconada\", SLC.\"NrBobinaProduzida\", SLC.\"NrMetrosQuadrados\", SLC.\"NmCliente\", SLC.\"IdRecebimento\" FROM \"T04_Siliconizacao\" SLC WHERE SLC.\"DsStatusSiliconizacao\" IN ('APROVADO', 'SEMI-APROVADO', 'LIBERAÇÃO PARCIAL');";

            DataTable tabela = db.ExecutarConsulta(query);

            List<SiliconizacaoModel> siliconizacao = new List<SiliconizacaoModel>();

            foreach (DataRow row in tabela.Rows)
            {
                siliconizacao.Add(new SiliconizacaoModel
                {
                    NmCliente = row["NmCliente"].ToString(),
                    CdInterno = row["CdInterno"].ToString(),
                    IdMaquina = row["IdMaquina"].ToString(),
                    DtSiliconizacao = row["DtSiliconizacao"].ToString(),
                    CdOS = row["CdOS"].ToString(),
                    IdRecebimento = row["IdRecebimento"].ToString(),
                    NrMetrosLinear = row["NrMetrosLinear"].ToString(),
                    NrBobinaProduzida = row["NrBobinaProduzida"].ToString(),
                    NmOperador = row["NmOperador"].ToString(),
                    CdBobinaSiliconada = row["CdBobinaSiliconada"].ToString(),
                    NrMetrosQuadrados = row["NrMetrosQuadrados"].ToString(),
                    IsSelected = false
                });
            }

            dtgGrade.ItemsSource = new ObservableCollection<SiliconizacaoModel>(siliconizacao);
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = (ObservableCollection<SiliconizacaoModel>)dtgGrade.ItemsSource;

            if (itemsSource != null)
            {
                try
                {
                    using (var conn = new Npgsql.NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
                    {
                        conn.Open();

                        var selectedItems = itemsSource.Where(i => i.IsSelected).ToList();

                        if (selectedItems.Any())
                        {
                            foreach (var item in selectedItems)
                            {
                                string comando = @"
                                                UPDATE public.""T04_Siliconizacao""
                                                SET ""DsStatusSiliconizacao"" = 'Corte'
                                                WHERE ""IdRecebimento"" = @IdRecebimento;";

                                using (var cmd = new Npgsql.NpgsqlCommand(comando, conn))
                                {
                                    cmd.Parameters.AddWithValue("IdRecebimento", string.IsNullOrEmpty(item.IdRecebimento) ? DBNull.Value : item.IdRecebimento);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            CF0401 siliconizacao = new CF0401(usuarioId);
                            siliconizacao.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Nenhum registro foi selecionado para enviar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
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
            CF0401 siliconizacao = new CF0401(usuarioId);
            siliconizacao.Show();
            this.Close();
        }
    }
}