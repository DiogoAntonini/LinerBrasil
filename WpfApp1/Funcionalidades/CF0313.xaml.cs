using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0313 : Window
    {
        private Consultar db;
        private string idLoteFornecedor;

        public class DadoEntrada
        {
            public int IdRecebimento { get; set; }
            public string DsMaterialLiner { get; set; }
            public string DsFornecedor { get; set; }
            public string DsMetodoLiner { get; set; }
            public string DsCaracteristicaEnsaio { get; set; }
            public string DsUnidade { get; set; }
            public string NrMinimo { get; set; }
            public string NrMaximo { get; set; }
            public string NrValor { get; set; }
            public string DsStatusAmostra { get; set; }
            public string StImpressao { get; set; }
            public string NmResponsavel { get; set; }
        }

        public CF0313(string idLoteFornecedor)
        {
            InitializeComponent();
            this.idLoteFornecedor = idLoteFornecedor;
            db = new Consultar();

            string query = "SELECT RCB.\"IdRecebimento\", RCB.\"DsMaterial\" \"DsMaterialLiner\", AMT.\"DsFornecedor\", AMT.\"DsMetodoLiner\", AMT.\"DsCaracteristicaEnsaio\", AMT.\"DsUnidade\", AMT.\"NrMinimo\", AMT.\"NrMaximo\", AMT.\"NrValor\", AMT.\"StImpressao\", AMT.\"NmResponsavel\", AMT.\"DsStatusAmostra\" FROM \"T02_Recebimento\" RCB LEFT JOIN \"T03_AmostraMateriaPrima\" AMT ON AMT.\"IdRecebimento\" = RCB.\"IdRecebimento\" WHERE RCB.\"DsStatus\" = 'Laboratório' AND RCB.\"IdLoteFornecedor\" = @IdLoteFornecedor";

            var parametros = new Dictionary<string, object>
            {
                { "@IdLoteFornecedor", this.idLoteFornecedor }
            };

            DataTable tabela = db.ExecutarConsultaComParametros(query, parametros);

            ObservableCollection<DadoEntrada> materiaprima = new ObservableCollection<DadoEntrada>();

            foreach (DataRow row in tabela.Rows)
            {
                materiaprima.Add(new DadoEntrada
                {
                    IdRecebimento = Convert.ToInt32(row["IdRecebimento"].ToString()),
                    DsFornecedor = row["DsFornecedor"].ToString(),
                    DsMaterialLiner = row["DsMaterialLiner"].ToString(),
                    DsMetodoLiner = row["DsMetodoLiner"].ToString(),
                    DsCaracteristicaEnsaio = row["DsCaracteristicaEnsaio"].ToString(),
                    DsUnidade = row["DsUnidade"].ToString(),
                    NrMinimo = row["NrMinimo"].ToString(),
                    NrMaximo = row["NrMaximo"].ToString(),
                    NrValor = row["NrValor"].ToString(),
                    DsStatusAmostra = row["DsStatusAmostra"].ToString(),
                    NmResponsavel = row["NmResponsavel"].ToString(),
                    StImpressao = row["StImpressao"].ToString()
                });
            }

            dtgGrade.ItemsSource = materiaprima;
        }

        private void txtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textoPesquisa = txtPesquisa.Text.ToLower();

            FiltrarDataGrid(dtgGrade, textoPesquisa);
        }

        private void FiltrarDataGrid(DataGrid dataGrid, string textoPesquisa)
        {
            DataView dataView = dataGrid.ItemsSource as DataView;

            if (dataView != null)
            {
                if (string.IsNullOrEmpty(textoPesquisa))
                {
                    dataView.RowFilter = "";
                }
                else
                {
                    string filtro = "";
                    foreach (DataColumn column in dataView.Table.Columns)
                    {
                        if (filtro != "")
                            filtro += " OR ";

                        filtro += $"CONVERT([{column.ColumnName}], System.String) LIKE '%{textoPesquisa}%'";
                    }

                    dataView.RowFilter = filtro;
                }
            }
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
                            string queryLoteFornecedor = "SELECT \"IdLoteFornecedor\" FROM \"T02_Recebimento\" WHERE \"IdRecebimento\" = @IdRecebimento";
                            string idLoteFornecedor = null;

                            using (var cmdLoteFornecedor = new Npgsql.NpgsqlCommand(queryLoteFornecedor, conn))
                            {
                                cmdLoteFornecedor.Parameters.AddWithValue("IdRecebimento", item.IdRecebimento);
                                idLoteFornecedor = (string)cmdLoteFornecedor.ExecuteScalar();
                            }

                            string comando = "INSERT INTO \"T03_AmostraMateriaPrima\" (\"IdRecebimento\", \"DsMaterialLiner\", \"DsFornecedor\", \"DsMetodoLiner\", \"DsCaracteristicaEnsaio\", \"DsUnidade\", \"NrMinimo\", \"NrMaximo\", \"NrValor\", \"StImpressao\", \"NmResponsavel\", \"DsStatusAmostra\")" +
                                             "SELECT RCB.\"IdRecebimento\", @DsMaterialLiner, @DsFornecedor, @DsMetodoLiner, @DsCaracteristicaEnsaio, @DsUnidade, @NrMinimo, @NrMaximo, @NrValor, @StImpressao, @NmResponsavel, @DsStatusAmostra " +
                                             "FROM \"T02_Recebimento\" RCB " +
                                             "WHERE RCB.\"IdLoteFornecedor\" = @IdLoteFornecedor AND NOT EXISTS(SELECT 1 FROM \"T03_AmostraMateriaPrima\" AMP WHERE AMP.\"IdRecebimento\" = RCB.\"IdRecebimento\")";

                            using (var cmdInsert = new Npgsql.NpgsqlCommand(comando, conn))
                            {
                                cmdInsert.Parameters.AddWithValue("IdRecebimento", item.IdRecebimento);
                                cmdInsert.Parameters.AddWithValue("DsMaterialLiner", string.IsNullOrEmpty(item.DsMaterialLiner) ? DBNull.Value : item.DsMaterialLiner);
                                cmdInsert.Parameters.AddWithValue("DsFornecedor", string.IsNullOrEmpty(item.DsFornecedor) ? DBNull.Value : item.DsFornecedor);
                                cmdInsert.Parameters.AddWithValue("DsMetodoLiner", string.IsNullOrEmpty(item.DsMetodoLiner) ? DBNull.Value : item.DsMetodoLiner);
                                cmdInsert.Parameters.AddWithValue("DsCaracteristicaEnsaio", string.IsNullOrEmpty(item.DsCaracteristicaEnsaio) ? DBNull.Value : item.DsCaracteristicaEnsaio);
                                cmdInsert.Parameters.AddWithValue("DsUnidade", string.IsNullOrEmpty(item.DsUnidade) ? DBNull.Value : item.DsUnidade);
                                cmdInsert.Parameters.AddWithValue("NrMinimo", double.TryParse(item.NrMinimo, out double nrMinimo) ? nrMinimo : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrMaximo", double.TryParse(item.NrMaximo, out double nrMaximo) ? nrMaximo : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NrValor", double.TryParse(item.NrValor, out double nrValor) ? nrValor : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("StImpressao", bool.TryParse(item.StImpressao, out bool stImpressao) ? stImpressao : DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("NmResponsavel", string.IsNullOrEmpty(item.NmResponsavel) ? DBNull.Value : item.NmResponsavel);
                                cmdInsert.Parameters.AddWithValue("DsStatusAmostra", string.IsNullOrEmpty(item.DsStatusAmostra) ? DBNull.Value : item.DsStatusAmostra);
                                cmdInsert.Parameters.AddWithValue("IdLoteFornecedor", idLoteFornecedor);

                                cmdInsert.ExecuteNonQuery();
                            }

                            string updateQuery = "UPDATE \"T02_Recebimento\" SET \"DsStatus\" = 'Aprovado' WHERE \"IdLoteFornecedor\" = @IdLoteFornecedor";

                            using (var cmdUpdate = new Npgsql.NpgsqlCommand(updateQuery, conn))
                            {
                                cmdUpdate.Parameters.AddWithValue("IdLoteFornecedor", idLoteFornecedor);
                                cmdUpdate.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao inserir dados: " + ex.Message);
                }
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