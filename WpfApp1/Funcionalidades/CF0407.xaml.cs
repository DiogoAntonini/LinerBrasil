using System.Windows;

namespace LinerApp.Funcionalidades
{
    public partial class CF0407 : Window
    {
        private string usuarioId;

        public CF0407(string idParada, string idMaquina, string dtInicial, string cdOS, string idTipoParada, string dtFinal, string dsMotivo)
        {
            InitializeComponent();

            IdMaquina.Text = idMaquina;
            DtInicial.Text = dtInicial;
            CdOS.Text = cdOS;
            IdTipoParada.Text = idTipoParada;
            DtFinal.Text = dtFinal;
            DsMotivo.Text = dsMotivo;
        }

        public CF0407()
        {
            InitializeComponent();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            CF0401 siliconizacao = new CF0401(usuarioId);
            siliconizacao.Show();
            Close();
        }
    }
}