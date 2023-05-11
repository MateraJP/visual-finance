using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace VisualFinanceiro.Auth.Implementations
{
    internal class ConnectionFactory : IConnectionFactory
    {
        private readonly SqlConnection conn;
        public ConnectionFactory(string connectionString)
        {
            conn = new SqlConnection(connectionString);
        }
        public ConnectionFactory(IConfiguration configuration)
        {
            conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IDbConnection> getConnection()
        {
            await PrepConn();
            return conn;
        }

        private async Task PrepConn(int tentativas = 1)
        {
            if (conn.State == ConnectionState.Open)
                return;

            if (tentativas > 3)
                throw new Exception("Não foi possível connectar-se ao banco, atingiu o numero máximo de tentativas");

            if (conn.State == ConnectionState.Connecting)
            {
                await Task.Delay(100);
                await PrepConn(tentativas + 1);
            }
            else if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
        }
    }
}
