using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using LinerApp.Funcionalidades;
using LinerApp.Funcionalidades.db;

namespace LinerApp
{
    public partial class CF0401 : Window
    {
        private string usuarioId;
        private Consultar db;
        private DataTable tabelaProducao;
        private DataTable tabelaParadas;

        public CF0401(string usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            db = new Consultar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnProducao_Click(this, new RoutedEventArgs());
        }

        private void btnProducao_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rctProducao = (Rectangle)btnProducao.Template.FindName("rctProducao", btnProducao);
            Rectangle rctParadas = (Rectangle)btnParadas.Template.FindName("rctParadas", btnParadas);

            if (rctProducao != null)
            {
                rctProducao.Visibility = Visibility.Visible;
                btnEnviarLaboratorio.Visibility = Visibility.Visible;
                btnAdicionar.Visibility = Visibility.Visible;
                btnEnviarCorte.Visibility = Visibility.Visible;
                btnImprimirEtiqueta.Visibility = Visibility.Visible;
                btnImprimirEtiqueta.Margin = new Thickness(1097, 213, 0, 0);
                rctLinha1.Visibility = Visibility.Visible;
                rctLinha2.Visibility = Visibility.Visible;
                rctLinha3.Visibility = Visibility.Visible;
                rctLinha4.Visibility = Visibility.Visible;
                rctLinha5.Visibility = Visibility.Visible;
                rctLinha6.Visibility = Visibility.Visible;
                rctLinha7.Visibility = Visibility.Visible;
                dtgGradeProducao.Visibility = Visibility.Visible;
                dtgGradeParadas.Visibility = Visibility.Hidden;
                btnAdicionarParada.Visibility = Visibility.Hidden;
            }

            if (rctParadas != null)
            {
                rctParadas.Visibility = Visibility.Hidden;
            }

            string query = "SELECT SLC.\"CdInterno\", SLC.\"IdMaquina\", TO_CHAR(SLC.\"DtSiliconizacao\", 'DD/MM/YY') \"DtSiliconizacao\", SLC.\"NmOperador\", SLC.\"CdOS\", SLC.\"NrMetrosLinear\", SLC.\"CdBobinaSiliconada\", SLC.\"NrBobinaProduzida\", SLC.\"NrMetrosQuadrados\", SLC.\"NmCliente\", SLC.\"IdRecebimento\" FROM \"T04_Siliconizacao\" SLC WHERE SLC.\"DsStatusSiliconizacao\" IN ('Pendente', 'APROVADO');";

            tabelaProducao = db.ExecutarConsulta(query);

            dtgGradeProducao.ItemsSource = tabelaProducao.DefaultView;
        }

        private void btnParadas_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rctProducao = (Rectangle)btnProducao.Template.FindName("rctProducao", btnProducao);
            Rectangle rctParadas = (Rectangle)btnParadas.Template.FindName("rctParadas", btnParadas);

            if (rctProducao != null)
            {
                rctProducao.Visibility = Visibility.Hidden;
                btnEnviarLaboratorio.Visibility = Visibility.Visible;
                btnAdicionar.Visibility = Visibility.Visible;
                btnEnviarCorte.Visibility = Visibility.Visible;
                btnImprimirEtiqueta.Visibility = Visibility.Visible;
                btnImprimirEtiqueta.Margin = new Thickness(1097, 144, 0, 0);
                rctLinha1.Visibility = Visibility.Visible;
                rctLinha2.Visibility = Visibility.Visible;
                rctLinha3.Visibility = Visibility.Visible;
                rctLinha4.Visibility = Visibility.Visible;
                rctLinha5.Visibility = Visibility.Visible;
                rctLinha6.Visibility = Visibility.Visible;
                rctLinha7.Visibility = Visibility.Visible;
                dtgGradeProducao.Visibility = Visibility.Visible;
                dtgGradeParadas.Visibility = Visibility.Hidden;
                btnAdicionarParada.Visibility = Visibility.Hidden;
            }

            if (rctParadas != null)
            {
                rctParadas.Visibility = Visibility.Visible;
                btnEnviarLaboratorio.Visibility = Visibility.Hidden;
                btnAdicionar.Visibility = Visibility.Hidden;
                btnEnviarCorte.Visibility = Visibility.Hidden;
                btnImprimirEtiqueta.Visibility = Visibility.Hidden;
                rctLinha1.Visibility = Visibility.Visible;
                rctLinha2.Visibility = Visibility.Visible;
                rctLinha3.Visibility = Visibility.Visible;
                rctLinha4.Visibility = Visibility.Hidden;
                rctLinha5.Visibility = Visibility.Hidden;
                rctLinha6.Visibility = Visibility.Hidden;
                rctLinha7.Visibility = Visibility.Visible;
                dtgGradeProducao.Visibility = Visibility.Hidden;
                dtgGradeParadas.Visibility = Visibility.Visible;
                btnAdicionarParada.Visibility = Visibility.Visible;
                btnAdicionarParada.Margin = new Thickness(1097, 144, 0, 0);
            }

            string query = "SELECT PRS.\"IdParada\", PRS.\"IdMaquina\", TO_CHAR(PRS.\"DtInicial\", 'DD/MM/YY HH:mm') \"DtInicial\" , PRS.\"CdOS\", PRS.\"IdTipoParada\", TO_CHAR(PRS.\"DtFinal\", 'DD/MM/YY HH:mm') \"DtFinal\", PRS.\"DsMotivo\" FROM \"T0401_ParadasSiliconizacao\" PRS;";

            tabelaParadas = db.ExecutarConsulta(query);

            dtgGradeParadas.ItemsSource = tabelaParadas.DefaultView;
        }

        private void DropDownMaquinas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = DropDownMaquinas.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                string selectedMachine = selectedItem.Content.ToString();

                if (tabelaProducao != null)
                {
                    DataView producaoView = tabelaProducao.DefaultView;
                    if (selectedMachine == "TODAS")
                    {
                        producaoView.RowFilter = "";
                    }
                    else if (selectedMachine == "Máquina 1")
                    {
                        producaoView.RowFilter = "IdMaquina = 1";
                    }
                    else if (selectedMachine == "Máquina 9")
                    {
                        producaoView.RowFilter = "IdMaquina = 9";
                    }
                    else if (selectedMachine == "Máquina 11")
                    {
                        producaoView.RowFilter = "IdMaquina = 11";
                    }
                    else if (selectedMachine == "Máquina 21")
                    {
                        producaoView.RowFilter = "IdMaquina = 21";
                    }
                    dtgGradeProducao.ItemsSource = producaoView;
                }

                if (tabelaParadas != null)
                {
                    DataView paradasView = tabelaParadas.DefaultView;
                    if (selectedMachine == "TODAS")
                    {
                        paradasView.RowFilter = "";
                    }
                    else if (selectedMachine == "Máquina 1")
                    {
                        paradasView.RowFilter = "IdMaquina = 1";
                    }
                    else if (selectedMachine == "Máquina 9")
                    {
                        paradasView.RowFilter = "IdMaquina = 9";
                    }
                    else if (selectedMachine == "Máquina 11")
                    {
                        paradasView.RowFilter = "IdMaquina = 11";
                    }
                    else if (selectedMachine == "Máquina 21")
                    {
                        paradasView.RowFilter = "IdMaquina = 21";
                    }
                    dtgGradeParadas.ItemsSource = paradasView;
                }
            }
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            CF0405 adicionar = new CF0405();
            adicionar.ShowDialog();
            this.Close();
        }

        private void btnEnviarLaboratorio_Click(object sender, RoutedEventArgs e)
        {
            CF0402 enviarParaLaboratorio = new CF0402();
            enviarParaLaboratorio.ShowDialog();
            this.Close();
        }

        private void btnCorte_Click(object sender, RoutedEventArgs e)
        {
            CF0403 enviarParaCorte = new CF0403();
            enviarParaCorte.ShowDialog();
            this.Close();
        }

        private void btnImpressao_Click(object sender, RoutedEventArgs e)
        {
            CF0404 imprimirEtiqueta = new CF0404();
            imprimirEtiqueta.ShowDialog();
            this.Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0002 modulos = new CF0002(usuarioId);
            modulos.Show();
            this.Close();
        }

        private void btnAdicionarParada_Click(object sender, RoutedEventArgs e)
        {
            CF0406 adicionar = new CF0406();
            adicionar.ShowDialog();
            this.Close();
        }

        private void btnAlterarParada_Click(object sender, RoutedEventArgs e)
        {
            CF0407 modulos = new CF0407();
            modulos.ShowDialog();
            this.Close();
        }

        private void dtgGradeParadas_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = dtgGradeParadas.SelectedItem;

            if (item != null)
            {
                var row = (DataRowView)item;
                var idParada = row["IdParada"].ToString();
                var idMaquina = row["IdMaquina"].ToString();
                var dtInicial = row["DtInicial"].ToString();
                var cdOS = row["CdOS"].ToString();
                var idTipoParada = row["IdTipoParada"].ToString();
                var dtFinal = row["DtFinal"].ToString();
                var dsMotivo = row["DsMotivo"].ToString();

                CF0407 view = new CF0407(idParada, idMaquina, dtInicial, cdOS, idTipoParada, dtFinal, dsMotivo);
                view.ShowDialog();
                this.Close();
            }
        }
    }
}