using System.Data;
using System.Windows;
using System.Windows.Controls;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0315 : Window
    {
        public CF0315()
        {
            InitializeComponent();
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dados = null;

                string DsDyeTesteValue = DsDyeTeste.Text.Trim();
                string DsCorValue = DsCor.Text.Trim();
                string StAlvejanteValue = StAlvejante.Text.Trim();
                string NrGramaturaValue = NrGramatura.Text.Trim();
                string NrReleaseValue = NrRelease.Text.Trim();
                string NrMigracaoValue = NrMigracao.Text.Trim();
                string NrAdesaoValue = NrAdesao.Text.Trim();
                string NrLarguraValue = NrLargura.Text.Trim();
                string DsStatusSiliconizacaoValue = DsStatusSiliconizacao.Text.Trim();
                string IdRecebimentoValue = IdRecebimento.Text.Trim();
                string NmOperadorValue = NmOperador.Text.Trim();
                string DtAtualValue = DateTime.Now.ToShortDateString();
                string HrAtualValue = DateTime.Now.ToString("HH:mm:ss");

                dados = new DataTable();
                dados.Columns.Add("DsDyeTeste");
                dados.Columns.Add("DsCor");
                dados.Columns.Add("StAlvejante");
                dados.Columns.Add("NrGramatura");
                dados.Columns.Add("NrRelease");
                dados.Columns.Add("NrMigracao");
                dados.Columns.Add("NrAdesao");
                dados.Columns.Add("NrLargura");
                dados.Columns.Add("DsStatusSiliconizacao");
                dados.Columns.Add("IdRecebimento");
                dados.Columns.Add("NmOperador");
                dados.Columns.Add("DtAtual");
                dados.Columns.Add("HrAtual");

                dados.Rows.Add(DsDyeTesteValue, DsCorValue, StAlvejanteValue, NrGramaturaValue, NrReleaseValue,
                               NrMigracaoValue, NrAdesaoValue, NrLarguraValue, DsStatusSiliconizacaoValue,
                               IdRecebimentoValue, NmOperadorValue, DtAtualValue, HrAtualValue);

                if (dados.Rows.Count > 0)
                {
                    PrintDialog printDialog = new PrintDialog();

                    if (printDialog.ShowDialog() == true)
                    {
                        foreach (DataRow row in dados.Rows)
                        {
                            LayoutEtiquetaAprovacaoSemiAcab etiqueta = new LayoutEtiquetaAprovacaoSemiAcab();

                            etiqueta.DsDyeTeste.Text = row["DsDyeTeste"].ToString();
                            etiqueta.DsCor.Text = row["DsCor"].ToString();
                            etiqueta.StAlvejante.Text = row["StAlvejante"].ToString();
                            etiqueta.NrGramatura.Text = row["NrGramatura"].ToString();
                            etiqueta.NrRelease.Text = row["NrRelease"].ToString();
                            etiqueta.NrMigracao.Text = row["NrMigracao"].ToString();
                            etiqueta.NrAdesao.Text = row["NrAdesao"].ToString();
                            etiqueta.NrLargura.Text = row["NrLargura"].ToString();
                            etiqueta.DsStatusSiliconizacao.Text = row["DsStatusSiliconizacao"].ToString();
                            etiqueta.IdRecebimento.Text = row["IdRecebimento"].ToString();
                            etiqueta.NmOperador.Text = row["NmOperador"].ToString();
                            etiqueta.DtAtual.Text = row["DtAtual"].ToString();
                            etiqueta.HrAtual.Text = row["HrAtual"].ToString();

                            printDialog.PrintVisual(etiqueta.grdImpressao, "Etiqueta de apuração bobina");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao executar a consulta: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CF0308 main = new CF0308();
            main.Show();
            Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0308 main = new CF0308();
            main.Show();
            this.Close();
        }
    }
}