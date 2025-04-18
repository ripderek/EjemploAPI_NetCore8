using Microsoft.Data.SqlClient;

namespace EjemploAPI.Connection
{
    public class SQLServerConnect
    {
        private readonly IConfiguration _configuration;

        public SQLServerConnect(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
