using System.Data;
using System.Windows;
using System.Windows.Input;
using LinerApp.Funcionalidades;
using LinerApp.Funcionalidades.db;

namespace LinerBrasil
{
    public partial class CF0001 : Window
    {
        private readonly Consultar db;

        public CF0001()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            db = new Consultar();
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = UsuarioTextBox.Text;
            string senha = SenhaPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.");
                return;
            }

            try
            {
                string query = "SELECT \"DsLogin\", \"DsSenha\", \"IdUsuario\" FROM \"T00_Configuracoes\" WHERE \"DsLogin\" = @login;";
                var parametros = new Dictionary<string, object>
                {
                    { "@login", login }
                };

                DataTable tabela = await Task.Run(() => db.ExecutarConsultaComParametros(query, parametros));

                if (tabela.Rows.Count > 0)
                {
                    string senhaSalva = tabela.Rows[0]["DsSenha"].ToString();

                    if (senha == senhaSalva)
                    {
                        string usuarioId = tabela.Rows[0]["IdUsuario"].ToString();
                        CF0002 modulos = new CF0002(usuarioId);
                        modulos.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Senha incorreta. Por favor, tente novamente.");
                    }
                }
                else
                {
                    MessageBox.Show("Usuário não encontrado. Por favor, verifique o login.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao tentar fazer login: {ex.Message}");
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonLogin_Click(sender, new RoutedEventArgs());
            }
        }
    }
}