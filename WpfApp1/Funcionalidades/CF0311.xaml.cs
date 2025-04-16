using System.Data;
using System.Windows;
using System.Windows.Controls;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0311 : Window
    {
        private Consultar db;

        public CF0311()
        {
            InitializeComponent();
            db = new Consultar();
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryAprovado = "SELECT RCB.\"IdRecebimento\", AMP.\"DsMaterialLiner\" , AMP.\"DsFornecedor\" , AMP.\"DsMetodoLiner\" , AMP.\"DsCaracteristicaEnsaio\" , AMP.\"DsUnidade\" , AMP.\"NrMinimo\" , AMP.\"NrMaximo\" , AMP.\"NrValor\" , AMP.\"DsStatusAmostra\", AMP.\"NmResponsavel\" FROM \"T02_Recebimento\" RCB LEFT JOIN \"T03_AmostraMateriaPrima\" AMP ON AMP.\"IdRecebimento\" = RCB.\"IdRecebimento\" WHERE AMP.\"DsStatusAmostra\" = 'APROVADO' AND AMP.\"StImpressao\" = FALSE;";
                string queryReprovado = "SELECT RCB.\"IdRecebimento\", AMP.\"DsMaterialLiner\" , AMP.\"DsFornecedor\" , AMP.\"DsMetodoLiner\" , AMP.\"DsCaracteristicaEnsaio\" , AMP.\"DsUnidade\" , AMP.\"NrMinimo\" , AMP.\"NrMaximo\" , AMP.\"NrValor\" , AMP.\"DsStatusAmostra\", AMP.\"NmResponsavel\" FROM \"T02_Recebimento\" RCB LEFT JOIN \"T03_AmostraMateriaPrima\" AMP ON AMP.\"IdRecebimento\" = RCB.\"IdRecebimento\" WHERE AMP.\"DsStatusAmostra\" = 'REPROVADO' AND AMP.\"StImpressao\" = FALSE;";

                DataTable dados = null;

                string txtMValue = txtM.Text.Trim();

                if (string.IsNullOrEmpty(txtMValue))
                {
                    switch (StatusComboBox.SelectedItem is ComboBoxItem selectedItem ? selectedItem.Content.ToString() : "")
                    {
                        case "APROVADO":
                            dados = db.ExecutarConsulta(queryAprovado);
                            break;
                        case "REPROVADO":
                            dados = db.ExecutarConsulta(queryReprovado);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    string queryIndividual = "SELECT AMP.\"StAlvejante\", AMP.\"NrGramatura\", AMP.\"NrLargura\", RCB.\"NrPesoBobina\", RCB.\"IdRecebimento\", RCB.\"NrBobina\", AMP.\"NmResponsavel\", AMP.\"DsStatusAmostra\" FROM \"T03_AmostraMateriaPrima\" AMP INNER JOIN \"T02_Recebimento\" RCB ON RCB.\"IdRecebimento\" = AMP.\"IdRecebimento\" WHERE RCB.\"IdRecebimento\" = @IdRecebimento;";

                    using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
                    {
                        conn.Open();

                        using (var cmd = new NpgsqlCommand(queryIndividual, conn))
                        {
                            cmd.Parameters.AddWithValue("IdRecebimento", txtMValue);
                            using (var reader = cmd.ExecuteReader())
                            {
                                dados = new DataTable();
                                dados.Load(reader);
                            }
                        }
                    }
                }

                if (dados != null && dados.Rows.Count > 0)
                {
                    using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
                    {
                        conn.Open();

                        PrintDialog printDialog = new PrintDialog();

                        if (printDialog.ShowDialog() == true)
                        {
                            foreach (DataRow row in dados.Rows)
                            {
                                LayoutEtiquetaAprovacao etiqueta = new LayoutEtiquetaAprovacao();

                                etiqueta.StAlvejante.Text = row["StAlvejante"]?.ToString() ?? string.Empty;
                                etiqueta.NrGramatura.Text = row["NrGramatura"]?.ToString() ?? string.Empty;
                                etiqueta.NrLargura.Text = row["NrLargura"]?.ToString() ?? string.Empty;
                                etiqueta.IdRecebimento.Text = row["IdRecebimento"]?.ToString() ?? string.Empty;
                                etiqueta.NrBobina.Text = row["NrBobina"]?.ToString() ?? string.Empty;
                                etiqueta.NmResponsavel.Text = row["NmResponsavel"]?.ToString() ?? string.Empty;
                                etiqueta.DsStatusAmostra.Text = row["DsStatusAmostra"]?.ToString() ?? string.Empty;
                                etiqueta.HtImpressao.Text = DateTime.Now.ToString("HH:mm:ss");
                                etiqueta.DtImpressao.Text = DateTime.Now.ToShortDateString();
                                DateTime dataHoje = DateTime.Now;

                                printDialog.PrintVisual(etiqueta.grdImpressao, "Etiqueta de Aprovação");

                                string comando = @"
                                UPDATE ""T03_AmostraMateriaPrima""
                                SET ""StImpressao"" = 'TRUE'
                                WHERE ""IdRecebimento"" = @IdRecebimento;";

                                using (var cmd = new NpgsqlCommand(comando, conn))
                                {
                                    cmd.Parameters.AddWithValue("IdRecebimento", row["IdRecebimento"]?.ToString() ?? string.Empty);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum dado encontrado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao executar a consulta: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CF0302 materiaPrima = new CF0302();
            materiaPrima.Show();
            Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0302 materiaPrima = new CF0302();
            materiaPrima.Show();
            Close();
        }
    }
}