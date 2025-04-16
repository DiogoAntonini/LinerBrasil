using System.Windows;

namespace LinerApp.Funcionalidades
{
    public partial class CF0301 : Window
    {
        private string usuarioId;

        public CF0301()
        {
            InitializeComponent();
        }

        private void btnMateriaPrima_Click(object sender, RoutedEventArgs e)
        {
            CF0302 materiaprima = new CF0302();
            materiaprima.Show();
            Close();
        }

        private void btnProducao_Click(object sender, RoutedEventArgs e)
        {
            CF0308 producao = new CF0308();
            producao.Show();
            Close();
        }

        private void btnCadastro_Click(object sender, RoutedEventArgs e)
        {
            CF0304 cadastro = new CF0304();
            cadastro.Show();
            Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0002 modulos = new CF0002(usuarioId);
            modulos.Show();
            Close();
        }
    }
}