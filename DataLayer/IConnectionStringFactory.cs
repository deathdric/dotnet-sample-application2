using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Application2.DataLayer
{
    public interface IConnectionStringFactory
    {
        Task<string> BuildConnectionStringAsync(String connectionStringName);
    }

    public class DefaultConnectionStringFactory : IConnectionStringFactory
    {
        private readonly IConfiguration _config;

        public DefaultConnectionStringFactory(IConfiguration config)
        {
            this._config = config;
        }

        public async Task<string> BuildConnectionStringAsync(string connectionStringName)
        {
            var connectionString = _config["ConnectionStrings:" + connectionStringName];
            if (connectionString == null)
            {
                throw new InvalidOperationException("Connection string not found : " + connectionStringName);
            }
            return connectionString;
        }
    }

}