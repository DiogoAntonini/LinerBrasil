using System.Data;
using System.Windows;
using System.Windows.Controls;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0404 : Window
    {
        private string usuarioId;
        private Consultar db;

        public CF0404()
        {
            InitializeComponent();
            db = new Consultar(); ;
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "SELECT \r\nSLC.\"DtSiliconizacao\"\r\n, SLC.\"IdMaquina\"\r\n, SLC.\"NmCliente\"\r\n, SLC.\"CdOS\"\r\n, SLC.\"NrMetrosLinear\"\r\n, SLC.\"NmOperador\"\r\n, SLC.\"IdRecebimento\"\r\n, SLC.\"NmCliente\"\r\n, RCB.\"NmFornecedor\"\r\n, SLC.\"CdInterno\"\r\n, SLC.\"NrBobinaProduzida\"\r\nFROM \r\n\"T04_Siliconizacao\" SLC \r\nLEFT JOIN\r\n\"T02_Recebimento\" RCB\r\nON \r\nRCB.\"IdRecebimento\" = SLC.\"IdRecebimento\"\r\nWHERE \r\nSLC.\"IdRecebimento\" = @IdRecebimento;";

                DataTable dados = null;

                string txtMValue = txtM.Text.Trim();

                using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("IdRecebimento", txtMValue);
                        using (var reader = cmd.ExecuteReader())
                        {
                            dados = new DataTable();
                            dados.Load(reader);
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
                                LayoutLancamentoProdSilicon etiqueta = new LayoutLancamentoProdSilicon();

                                etiqueta.DtSiliconizacao.Text = row["DtSiliconizacao"]?.ToString() ?? string.Empty;
                                etiqueta.IdMaquina.Text = row["IdMaquina"]?.ToString() ?? string.Empty;
                                etiqueta.NmCliente.Text = row["NmCliente"]?.ToString() ?? string.Empty;
                                etiqueta.CdOS.Text = row["CdOS"]?.ToString() ?? string.Empty;
                                etiqueta.NrMetrosLinear.Text = row["NrMetrosLinear"]?.ToString() ?? string.Empty;
                                etiqueta.NmOperador.Text = row["NmOperador"]?.ToString() ?? string.Empty;
                                etiqueta.IdRecebimento.Text = row["IdRecebimento"]?.ToString() ?? string.Empty;
                                etiqueta.NmFornecedor.Text = row["NmFornecedor"]?.ToString() ?? string.Empty;
                                etiqueta.NmCliente.Text = row["NmCliente"]?.ToString() ?? string.Empty;
                                etiqueta.CdInterno.Text = row["CdInterno"]?.ToString() ?? string.Empty;
                                etiqueta.NrBobinaProduzida.Text = row["NrBobinaProduzida"]?.ToString() ?? string.Empty;

                                printDialog.PrintVisual(etiqueta.grdImpressao, "PRODUÇÃO FORA DO SISTEMA");
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

            CF0401 siliconizacao = new CF0401(usuarioId);
            siliconizacao.Show();
            Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {

            CF0401 siliconizacao = new CF0401(usuarioId);
            siliconizacao.Show();
            this.Close();

        }
    }
}