using System.Data;
using Npgsql;

namespace LinerApp.Funcionalidades.db
{
    public class Consultar
    {
        private string connectionString = "Host=LinerPC;Port=5432;Username=postgres;Password=19111965;Database=postgres";

        public DataTable ExecutarConsultaComParametros(string query, Dictionary<string, object> parametros)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    foreach (var param in parametros)
                    {
                        var parameter = new NpgsqlParameter(param.Key, param.Value ?? DBNull.Value);

                        if (param.Value is string stringValue)
                        {
                            if (param.Key == "@usuario" && int.TryParse(stringValue, out int usuarioId))
                            {
                                parameter.Value = usuarioId;
                                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer;
                            }
                            else
                            {
                                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar;
                            }
                        }
                        else if (param.Value is int)
                        {
                            parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer;
                        }
                        else if (param.Value is DateTime dateValue)
                        {
                            parameter.Value = dateValue.Date;
                            parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date;
                        }

                        command.Parameters.Add(parameter);
                    }

                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable ExecutarConsulta(string query)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                    using (var comando = new NpgsqlCommand(query, conexao))
                    {
                        using (var reader = comando.ExecuteReader())
                        {
                            var tabela = new DataTable();
                            tabela.Load(reader);
                            return tabela;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
