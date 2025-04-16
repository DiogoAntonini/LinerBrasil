using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0314 : Window
    {
        private Consultar db;

        public CF0314()
        {
            InitializeComponent();
            db = new Consultar();
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            string idLoteFornecedor = txtLoteFornecedor.Text.Trim();

            string queryLoteFornecedor = "SELECT \"IdLoteFornecedor\" FROM \"T02_Recebimento\" WHERE \"IdLoteFornecedor\" = @IdLoteFornecedor";

            if (!string.IsNullOrEmpty(idLoteFornecedor))
            {
                var parametros = new Dictionary<string, object>
                {
                    { "@IdLoteFornecedor", idLoteFornecedor }
                };

                var loteFornecedor = db.ExecutarConsultaComParametros(queryLoteFornecedor, parametros);

                if (loteFornecedor == null || loteFornecedor.Rows.Count == 0)
                {
                    MessageBox.Show("O Lote do Fornecedor informado não foi encontrado. Por favor, verifique!");
                }
                else
                {
                    CF0313 materiaPrima = new CF0313(idLoteFornecedor);
                    materiaPrima.ShowDialog();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Por favor, informe o Lote do Fornecedor.");
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0302 materiaPrima = new CF0302();
            materiaPrima.Show();
            Close();
        }
    }
}