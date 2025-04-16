using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0304 : Window
    {
        private Consultar db;

        public CF0304()
        {
            InitializeComponent();
            db = new Consultar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnCaracteristicas_Click(this, new RoutedEventArgs());
        }

        private void btnCaracteristicas_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT CRT.\"DsCaracteristica\", CRT.\"DsTarget\", CRT.\"NrMaximo\", CRT.\"NrMinimo\" FROM \"T09_Caracteristica\" CRT;";

            DataTable tabela = db.ExecutarConsulta(query);
            dtgGrade.ItemsSource = tabela.DefaultView;
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            CF0305 incluir = new CF0305();
            incluir.ShowDialog();
            this.Close();
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            CF0306 alterar = new CF0306();
            alterar.ShowDialog();
            this.Close();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            CF0307 excluir = new CF0307();
            excluir.ShowDialog();
            this.Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0301 moduloLaboratorio = new CF0301();
            moduloLaboratorio.Show();
            this.Close();
        }
    }
}