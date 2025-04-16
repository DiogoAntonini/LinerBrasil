using System.Data;
using System.Windows;
using LinerApp.Funcionalidades.db;

namespace LinerApp.Funcionalidades
{
    public partial class CF0002 : Window
    {
        private readonly string usuarioId;
        private readonly Consultar db;

        public CF0002(string usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            db = new Consultar();
            CarregarPerfilDoUsuario();
        }

        private async void CarregarPerfilDoUsuario()
        {
            string queryperfil = "SELECT \"DsPerfil\" FROM \"T00_Configuracoes\" WHERE \"IdUsuario\" = @usuario;";

            var parametros = new Dictionary<string, object>
            {
                { "@usuario", usuarioId }
            };

            try
            {
                DataTable tabela = await Task.Run(() => db.ExecutarConsultaComParametros(queryperfil, parametros));

                if (tabela.Rows.Count > 0)
                {
                    string perfil = tabela.Rows[0]["DsPerfil"].ToString();

                    btnPlanejamento.Visibility = Visibility.Hidden;
                    btnRecebimento.Visibility = Visibility.Hidden;
                    btnLaboratorio.Visibility = Visibility.Hidden;
                    btnSiliconizacao.Visibility = Visibility.Hidden;
                    btnCorte.Visibility = Visibility.Hidden;
                    btnConfiguracoes.Visibility = Visibility.Hidden;

                    switch (perfil)
                    {
                        case "Administrador":
                            SetButtonVisibility(Visibility.Visible, btnPlanejamento, btnRecebimento, btnLaboratorio, btnSiliconizacao, btnCorte, btnConfiguracoes);
                            break;

                        case "Planejamento":
                            SetButtonVisibility(Visibility.Visible, btnPlanejamento, btnConfiguracoes);
                            SetButtonMargin(new Thickness(300, 230, 0, 0), btnPlanejamento);
                            SetButtonMargin(new Thickness(700, 230, 0, 0), btnConfiguracoes);
                            break;

                        case "Recebimento":
                            SetButtonVisibility(Visibility.Visible, btnRecebimento, btnConfiguracoes);
                            SetButtonMargin(new Thickness(300, 230, 0, 0), btnRecebimento);
                            SetButtonMargin(new Thickness(700, 230, 0, 0), btnConfiguracoes);
                            break;

                        case "Laboratório":
                            SetButtonVisibility(Visibility.Visible, btnLaboratorio, btnConfiguracoes);
                            SetButtonMargin(new Thickness(300, 230, 0, 0), btnLaboratorio);
                            SetButtonMargin(new Thickness(700, 230, 0, 0), btnConfiguracoes);
                            break;

                        case "Siliconizacao":
                            SetButtonVisibility(Visibility.Visible, btnSiliconizacao, btnConfiguracoes);
                            SetButtonMargin(new Thickness(300, 230, 0, 0), btnSiliconizacao);
                            SetButtonMargin(new Thickness(700, 230, 0, 0), btnConfiguracoes);
                            break;

                        case "Corte":
                            SetButtonVisibility(Visibility.Visible, btnCorte, btnConfiguracoes);
                            SetButtonMargin(new Thickness(300, 230, 0, 0), btnCorte);
                            SetButtonMargin(new Thickness(700, 230, 0, 0), btnConfiguracoes);
                            break;
                    }
                }
                //else
                //{
                //    MessageBox.Show("Perfil do usuário não encontrado.");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao carregar o perfil do usuário: {ex.Message}");
            }
        }

        private void SetButtonVisibility(Visibility visibility, params UIElement[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Visibility = visibility;
            }
        }

        private void SetButtonMargin(Thickness margin, params FrameworkElement[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Margin = margin;
            }
        }

        private void btnSiliconizacao_Click(object sender, RoutedEventArgs e)
        {
            CF0401 siliconizacao = new CF0401(usuarioId);
            siliconizacao.Show();
            this.Close();
        }

        private void btnRecebimento_Click(object sender, RoutedEventArgs e)
        {
            CF0201 recebimento = new CF0201(usuarioId);
            recebimento.Show();
            this.Close();
        }

        private void btnPlanejamento_Click(object sender, RoutedEventArgs e)
        {
            CF0101 planejamento = new CF0101(usuarioId);
            planejamento.Show();
            this.Close();
        }

        private void btnLaboratorio_Click(object sender, RoutedEventArgs e)
        {
            CF0301 modlaboratorio = new CF0301();
            modlaboratorio.Show();
            this.Close();
        }

        private void btnCorte_Click(object sender, RoutedEventArgs e)
        {
            //CF0501 recebimento = new CF0501();
            //recebimento.Show();
            //this.Close();
        }

        private void btnConfiguracoes_Click(object sender, RoutedEventArgs e)
        {
            CF0003 configuracoes = new CF0003(usuarioId);
            configuracoes.Show();
            this.Close();
        }
    }
}