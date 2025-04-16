using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;
using Npgsql;

namespace LinerApp.Funcionalidades
{
    public partial class CF0405 : Window
    {
        private string usuarioId;
        private Consultar db;

        public CF0405()
        {
            InitializeComponent();
            DtSiliconizacao.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            db = new Consultar();
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string idMaquina = IdMaquina.Text;
                string dtSiliconizacaoStr = DtSiliconizacao.Text;
                string nmOperador = NmOperador.Text;
                string cdOS = CdOS.Text;
                string nrMetrosLinear = NrMetrosLinear.Text;
                double nrMetrosLinearDouble = Convert.ToDouble(nrMetrosLinear);
                string nrBobinaProduzida = NrBobinaProduzida.Text;
                string cdBobinaSiliconada = CdOS.Text + NrBobinaProduzida.Text;
                string idLoteEntradaLiner = IdRecebimento.Text;

                DateTime? dtSiliconizacao = null;
                if (DateTime.TryParse(dtSiliconizacaoStr, out DateTime parsedDate))
                {
                    dtSiliconizacao = parsedDate;
                }

                string query = "SELECT PNJ.\"CdInternoMaterial\", SLC.\"IdMaquina\", TO_CHAR(SLC.\"DtSiliconizacao\", 'DD/MM/YY') \"DtSiliconizacao\", SLC.\"NmOperador\", SLC.\"CdOS\", PNJ.\"NrMetrosLinear\", SLC.\"CdBobinaSiliconada\", SLC.\"NrBobinaProduzida\", SLC.\"NrMetrosQuadrados\", PNJ.\"NmCliente\", SLC.\"IdRecebimento\" FROM \"T01_Planejamento\" PNJ LEFT JOIN \"T04_Siliconizacao\" SLC ON SLC.\"CdOS\" = PNJ.\"CdOS\" WHERE PNJ.\"CdOS\" = @CdOS;";

                var parametros = new Dictionary<string, object>
                {
                    { "@CdOS", cdOS }
                };

                DataTable tabela = db.ExecutarConsultaComParametros(query, parametros);

                string nmCliente = tabela.Rows[0]["NmCliente"].ToString();
                double nrMetrosQuadradosDouble = nrMetrosLinearDouble * 1.2;
                string nrMetrosQuadrados = nrMetrosQuadradosDouble.ToString("F0");
                string cdInterno = tabela.Rows[0]["CdInternoMaterial"].ToString();

                using (var conn = new NpgsqlConnection("Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres"))
                {
                    conn.Open();

                    string comando = "INSERT INTO public.\"T04_Siliconizacao\" (\"CdInterno\", \"IdMaquina\", \"DtSiliconizacao\", \"NmOperador\", \"CdOS\", \"NrMetrosLinear\", \"CdBobinaSiliconada\", \"NrBobinaProduzida\", \"NrMetrosQuadrados\", \"IdRecebimento\", \"NmCliente\")" +
                                     "VALUES (@CdInterno, @IdMaquina, @DtSiliconizacao, @NmOperador, @CdOS, @NrMetrosLinear, @CdBobinaSiliconada, @NrBobinaProduzida, @NrMetrosQuadrados, @IdRecebimento, @NmCliente)";

                    using (var cmdInsert = new NpgsqlCommand(comando, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("CdInterno", string.IsNullOrEmpty(cdInterno) ? DBNull.Value : cdInterno);
                        cmdInsert.Parameters.AddWithValue("IdMaquina", string.IsNullOrEmpty(idMaquina) ? DBNull.Value : idMaquina);
                        cmdInsert.Parameters.AddWithValue("NmOperador", string.IsNullOrEmpty(nmOperador) ? DBNull.Value : nmOperador);
                        cmdInsert.Parameters.AddWithValue("CdOS", string.IsNullOrEmpty(cdOS) ? DBNull.Value : cdOS);
                        cmdInsert.Parameters.AddWithValue("CdBobinaSiliconada", string.IsNullOrEmpty(cdBobinaSiliconada) ? DBNull.Value : cdBobinaSiliconada);
                        cmdInsert.Parameters.AddWithValue("NrBobinaProduzida", string.IsNullOrEmpty(nrBobinaProduzida) ? DBNull.Value : nrBobinaProduzida);
                        cmdInsert.Parameters.AddWithValue("IdRecebimento", string.IsNullOrEmpty(idLoteEntradaLiner) ? DBNull.Value : idLoteEntradaLiner);
                        cmdInsert.Parameters.AddWithValue("NmCliente", string.IsNullOrEmpty(nmCliente) ? DBNull.Value : nmCliente);
                        cmdInsert.Parameters.AddWithValue("NrMetrosLinear", string.IsNullOrEmpty(nrMetrosLinear) ? DBNull.Value : nrMetrosLinear);
                        cmdInsert.Parameters.AddWithValue("NrMetrosQuadrados", string.IsNullOrEmpty(nrMetrosQuadrados) ? DBNull.Value : nrMetrosQuadrados);
                        cmdInsert.Parameters.AddWithValue("DtSiliconizacao", dtSiliconizacao.HasValue ? (object)dtSiliconizacao.Value : DBNull.Value);

                        cmdInsert.ExecuteNonQuery();
                    }
                }

                CF0401 main = new CF0401(usuarioId);
                main.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inserir dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0401 main = new CF0401(usuarioId);
            main.Show();
            this.Close();
        }
    }
}